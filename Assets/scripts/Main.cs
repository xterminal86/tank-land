using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour 
{
  public GameObject ObjectsHolder;
  public GameObject EnemiesHolder;

  public GameObject TerrainTile;
  public GameObject Obstacle;
  public GameObject MapBorder;

  public List<GameObject> Enemies;

  public TankPlayer Player;

  public AudioSource GameOverSound;
  public AudioSource RektSound;
  public AudioSource EnemyHitSound;
  public AudioSource PlayerHitSound;

  public List<AudioSource> ShotSounds;

  public Text DebugText;
  public Text ScoreCount;

  public GameObject GameOverForm;
  public GameObject LoadingScreen;

  [HideInInspector]
  public bool IsGameOver = false;

  [HideInInspector]
  public int Score = 0;

  void OnEnable()
  {
    SceneManager.sceneLoaded += SceneLoadedHandler;
  }

  void OnDisable()
  {
    SceneManager.sceneLoaded -= SceneLoadedHandler;
  }

  bool _isLoading = false;
  void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
  {
    _isLoading = true;

    StartCoroutine(BuildMapRoutine());
  }

  List<Vector2> _obstaclesGrid = new List<Vector2>();
  IEnumerator BuildMapRoutine()
  {
    for (int x = 0; x < GlobalConstants.MapSize; x++)
    {
      for (int y = 0; y < GlobalConstants.MapSize; y++)
      {
        Instantiate(TerrainTile, new Vector3(x, y, 0.0f), Quaternion.identity, ObjectsHolder.transform);
      }
    }

    for (float x = 3.0f; x < GlobalConstants.MapSize - 3.0f; x += 3.0f)
    {
      for (float y = 3.0f; y < GlobalConstants.MapSize - 3.0f; y += 3.0f)
      {
        _obstaclesGrid.Add(new Vector2(x, y));
      }
    }

    PlaceBorder();
    PlaceObstacles();
    SetupSpawnZones();

    Player.SetPlayerPosition(new Vector3(GlobalConstants.MapSize / 2.0f, GlobalConstants.MapSize / 2.0f, 0.0f));

    Score = 0;
    ScoreCount.text = Score.ToString();

    _isLoading = false;

    yield return StartCoroutine(WaitForSecondsRoutine(2.0f));

    LoadingScreen.SetActive(false);

    yield return null;
  }
    
  IEnumerator WaitForSecondsRoutine(float timeToWait)
  {
    float timer = 0.0f;
    while (timer < timeToWait)
    {
      timer += Time.smoothDeltaTime;

      yield return null;
    }

    yield return null;
  }

  float _mapBorderOffset = 1.0f; // -0.6f
  void PlaceBorder()
  {
    //float half = _mapBorderOffset / 2.0f;

    for (float i = -_mapBorderOffset; i < GlobalConstants.MapSize + _mapBorderOffset; i += _mapBorderOffset)
    {
      Instantiate(MapBorder, new Vector3(i, -_mapBorderOffset, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
      Instantiate(MapBorder, new Vector3(i, GlobalConstants.MapSize, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
    }

    for (float i = -_mapBorderOffset; i < GlobalConstants.MapSize + _mapBorderOffset; i += _mapBorderOffset)
    {
      Instantiate(MapBorder, new Vector3(-_mapBorderOffset, i, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
      Instantiate(MapBorder, new Vector3(GlobalConstants.MapSize, i, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
    }
  }

  List<Vector2> _emptyCells = new List<Vector2>();
  int[,] _obstaclesPlaced = new int[GlobalConstants.MapSize, GlobalConstants.MapSize];

  int _maxObstacles = 25;
  void PlaceObstacles()
  {
    int mx = GlobalConstants.MapSize / 2;
    int my = GlobalConstants.MapSize / 2;

    if (_maxObstacles > _obstaclesGrid.Count)
    {
      _maxObstacles = _obstaclesGrid.Count;
    }

    for (int i = 0; i < _maxObstacles; i++)
    {
      int index = Random.Range(0, _obstaclesGrid.Count);
      float angle = Random.Range(0.0f, 360.0f);

      Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

      float x = _obstaclesGrid[index].x;
      float y = _obstaclesGrid[index].y;

      // Disable placing on tank starting position (center of map)
      if (x > mx - 3.0f && x < mx + 3.0f && y > my - 3.0f && y < my + 3.0f)           
      {
        continue;
      }

      Instantiate(Obstacle, new Vector3(x, y, 0.0f), rotation, ObjectsHolder.transform);

      _obstaclesPlaced[(int)x, (int)y] = 1;

      _obstaclesGrid.RemoveAt(index);
    }

    for (int x = 0; x < GlobalConstants.MapSize; x++)
    {
      for (int y = 0; y < GlobalConstants.MapSize; y++)
      {
        if (_obstaclesPlaced[x, y] != 1)
        {
          _emptyCells.Add(new Vector2(x, y));
        }
      }
    }
  }

  List<Vector2> _leftSpawnZone = new List<Vector2>();
  List<Vector2> _rightSpawnZone = new List<Vector2>();
  List<Vector2> _topSpawnZone = new List<Vector2>();
  List<Vector2> _bottomSpawnZone = new List<Vector2>();

  Dictionary<int, List<Vector2>> _zoneById = new Dictionary<int, List<Vector2>>();

  void SetupSpawnZones()
  {
    for (int i = 2; i <= GlobalConstants.MapSize - 2; i++)
    {
      if (_obstaclesPlaced[2, i] != 1)
      {
        _leftSpawnZone.Add(new Vector2(2, i));
      }

      if (_obstaclesPlaced[GlobalConstants.MapSize - 2, i] != 1)
      {
        _rightSpawnZone.Add(new Vector2(GlobalConstants.MapSize - 2, i));
      }
    }

    for (int i = 3; i <= GlobalConstants.MapSize - 3; i++)
    {
      if (_obstaclesPlaced[i, 3] != 1)
      {
        _bottomSpawnZone.Add(new Vector2(i, 3));
      }

      if (_obstaclesPlaced[i, GlobalConstants.MapSize - 3] != 1)
      {
        _topSpawnZone.Add(new Vector2(i, GlobalConstants.MapSize - 3));
      }
    }

    _zoneById[0] = _topSpawnZone;
    _zoneById[1] = _rightSpawnZone;
    _zoneById[2] = _bottomSpawnZone;
    _zoneById[3] = _leftSpawnZone;

    //print(_leftSpawnZone.Count);
    //print(_rightSpawnZone.Count);
    //print(_topSpawnZone.Count);
    //print(_bottomSpawnZone.Count);

    //PrintZone("Left Zone", _leftSpawnZone);
    //PrintZone("Right Zone", _rightSpawnZone);
    //PrintZone("Top Zone", _topSpawnZone);
    //PrintZone("Bottom Zone", _bottomSpawnZone);
  }

  void PrintZone(string zoneNameToDisplay, List<Vector2> zone)
  {
    string output = zoneNameToDisplay;

    foreach (var item in zone)
    {
      output += string.Format(" {0} ", item);
    }

    output += "\n";

    Debug.Log(output);
  }

  [HideInInspector]
  public int EnemiesSpawned = 0;

  float _spawnTimer = 0.0f;
  void Update()
  {
    if (_isLoading) return;

    CountEnemies();

    if (Input.GetKeyDown(KeyCode.R))
    {
      SceneManager.LoadScene("main");
      return;
    }

    if (EnemiesSpawned >= GlobalConstants.MaxEnemies || IsGameOver)
    {
      return;
    }

    TryToSpawnEnemies();

    #if UNITY_EDITOR
    DebugText.text = _debugText;
    #endif
  }

  List<int> _activeZones = new List<int>();
  void TryToSpawnEnemies()
  {
    if (_spawnTimer < GlobalConstants.SpawnTimeout)
    {
      _spawnTimer += Time.smoothDeltaTime;
    }
    else
    {
      _spawnTimer = 0.0f;

      _activeZones.Clear();

      int lx = (int)Player.RigidbodyComponent.position.x - 25;
      int hx = (int)Player.RigidbodyComponent.position.x + 25;
      int ly = (int)Player.RigidbodyComponent.position.y - 25;
      int hy = (int)Player.RigidbodyComponent.position.y + 25;

      if (lx >= 2) _activeZones.Add(3);
      if (hx <= GlobalConstants.MapSize - 2) _activeZones.Add(1);
      if (ly >= 2) _activeZones.Add(2);
      if (hy <= GlobalConstants.MapSize - 2) _activeZones.Add(0);

      #if UNITY_EDITOR
      SetDebugText();
      #endif

      int enemyTypeIndex = Random.Range(0, Enemies.Count);

      int zoneIndex = Random.Range(0, _activeZones.Count);
      int cellIndex = Random.Range(0, _zoneById[_activeZones[zoneIndex]].Count);

      int xCoord = (int)_zoneById[_activeZones[zoneIndex]][cellIndex].x;
      int yCoord = (int)_zoneById[_activeZones[zoneIndex]][cellIndex].y;

      Instantiate(Enemies[enemyTypeIndex], new Vector3(xCoord, yCoord, -1.0f), Quaternion.identity, EnemiesHolder.transform);
    }
  }

  string _debugText = string.Empty;
  void SetDebugText()
  {
    _debugText = string.Format("Enemies count: {0}\n", EnemiesSpawned);

    _debugText += string.Format("Active zones: \n");

    foreach (var zone in _activeZones)
    {
      _debugText += string.Format("{0} ", zone);
    }

    _debugText += "\n";

  }

  void CountEnemies()
  {
    EnemiesSpawned = EnemiesHolder.transform.childCount;
  }

  public void RestartGameHandler()
  {
    SceneManager.LoadScene("main");
  }

  public void PlaySound(AudioSource premade)
  {
    GameObject go = new GameObject("snd-" + premade.name);
    AudioSource a = go.AddComponent<AudioSource>();
    a.playOnAwake = false;
    a.volume = premade.volume;
    a.clip = premade.clip;    
    float length = a.clip.length;
    a.Play();
    Destroy(go, length);    
  }
}
