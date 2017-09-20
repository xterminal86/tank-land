﻿using System.Collections;
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

  public Text EnemiesCount;

  public GameObject GameOverForm;

  [HideInInspector]
  public bool IsGameOver = false;

  List<Vector2> _obstaclesGrid = new List<Vector2>();
  void Awake()
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

    Player.SetPlayerPosition(new Vector3(GlobalConstants.MapSize / 2.0f, GlobalConstants.MapSize / 2.0f, 0.0f));
  }
    
  void PlaceBorder()
  {
    for (float i = -0.6f; i < GlobalConstants.MapSize - 0.6f; i += 0.6f)
    {
      Instantiate(MapBorder, new Vector3(i, -0.6f, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
      Instantiate(MapBorder, new Vector3(i, GlobalConstants.MapSize - 0.6f, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
    }

    for (float i = 0.0f; i < GlobalConstants.MapSize - 0.6f; i += 0.6f)
    {
      Instantiate(MapBorder, new Vector3(-0.6f, i, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
      Instantiate(MapBorder, new Vector3(GlobalConstants.MapSize - 0.6f, i, MapBorder.transform.position.z), Quaternion.identity, ObjectsHolder.transform);
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

  [HideInInspector]
  public int EnemiesSpawned = 0;

  float _spawnTimer = 0.0f;
  void Update()
  {
    if (EnemiesSpawned >= GlobalConstants.MaxEnemies || IsGameOver)
    {
      return;
    }

    CountEnemies();
    TryToSpawnEnemies();
  }

  void TryToSpawnEnemies()
  {
    if (_spawnTimer < GlobalConstants.SpawnTimeout)
    {
      _spawnTimer += Time.smoothDeltaTime;
    }
    else
    {
      _spawnTimer = 0.0f;
      int index = Random.Range(0, Enemies.Count);
      int cellIndex = Random.Range(0, _emptyCells.Count);

      Instantiate(Enemies[index], new Vector3(_emptyCells[cellIndex].x, _emptyCells[cellIndex].y, -1.0f), Quaternion.identity, EnemiesHolder.transform);
    }
  }

  void CountEnemies()
  {
    EnemiesSpawned = EnemiesHolder.transform.childCount;
    EnemiesCount.text = EnemiesSpawned.ToString();
  }

  public void RestartGameHandler()
  {
    SceneManager.LoadScene("main");
  }
}
