using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour 
{
  public GameObject DeathAnimation;
  public Rigidbody2D RigidbodyComponent;
  public SpriteRenderer SpriteRendererComponent;
  public Text HitpointsText;

  public GameObject DamageIndicatorPrefab;
  public AudioSource AttackSound;

  [HideInInspector]
  public RectTransform DamageIndicatorsCanvas;

  protected TankPlayer _player;
  protected Main _app;

  protected Vector2 _direction = Vector2.zero;

  protected Color _originalColor = Color.white;

  protected int _hitpoints = 1;
  public int Hitpoints
  {
    get { return _hitpoints; }
  }
      
  protected float _defence = 1.0f;
  public float Defence
  {
    get { return _defence; }
  }

  void Awake()
  { 
    _app = GameObject.Find("App").GetComponent<Main>();

    DamageIndicatorsCanvas = GameObject.Find("damage-indicators").GetComponent<RectTransform>();

    Init();
  }

  protected DamageIndicator _damageIndicatorBar;
  protected virtual void Init()
  { 
    var go = Instantiate(DamageIndicatorPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity, DamageIndicatorsCanvas);
    _damageIndicatorBar = go.GetComponent<DamageIndicator>();

    _originalColor = SpriteRendererComponent.color;

    _player = GameObject.Find("tank-player").GetComponent<TankPlayer>();

    _direction = _player.RigidbodyComponent.position - RigidbodyComponent.position;
    _direction.Normalize();
  }

  bool _isDestroying = false;
  public void ReceiveDamage(int damageReceived, bool friendlyFire = false)
  {    
    _hitpoints -= damageReceived;

    _damageShowTimeout = 0.0f;
    _showDamageBar = !friendlyFire;

    _damageIndicatorBar.Damage(damageReceived);

    // Excerpt from https://docs.unity3d.com/Manual/ExecutionOrder.html
    //
    // "FixedUpdate: FixedUpdate is often called more frequently than Update. 
    // "It can be called multiple times per frame, if the frame rate is low and it may not be called between frames at 
    // "all if the frame rate is high. All physics calculations and updates occur immediately after FixedUpdate."
    //
    // Collision handling is happening after FixedUpdate, which in turn can be called multiple times per frame.
    // Destroy only marks object for destroy, which will happen only in the next frame.
    // So to prevent entering the condition multiple times, we use a boolean flag.
    if (_hitpoints <= 0 && !_isDestroying)
    {
      _isDestroying = true;

      if (!friendlyFire)
      {        
        if (this is EnemyWeak) _app.Score += GlobalConstants.EnemyWeakScore;
        if (this is EnemyMedium) _app.Score += GlobalConstants.EnemyMediumScore;
        if (this is EnemyHeavy) _app.Score += GlobalConstants.EnemyHeavyScore;

        _app.ScoreCount.text = _app.Score.ToString();
      }

      var explosion = Instantiate(DeathAnimation, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);
      var ps = explosion.GetComponent<ParticleSystem>().main;
      ParticleSystem.MinMaxGradient g = new ParticleSystem.MinMaxGradient(_originalColor);
      ps.startColor = g;

      Destroy(explosion, 2.0f);

      Destroy(_damageIndicatorBar.gameObject);
      Destroy(gameObject);
    }
  }

  protected float _colorLerpParameter = 0.0f;
  protected float _moveSpeed = 1.0f;

  Color _lerpedColor = Color.white;

  float _damageShowTimeout = 0.0f;
  bool _showDamageBar = false;
  protected virtual void Update()
  {
    if (_app.IsGameOver) return;

    _damageShowTimeout += Time.smoothDeltaTime;

    if (_damageShowTimeout > 2.0f)
    {
      _showDamageBar = false;
    }

    if (_damageIndicatorBar != null)
    {
      _damageIndicatorBar.gameObject.SetActive(_showDamageBar);
    }

    _colorLerpParameter += Time.smoothDeltaTime;

    _colorLerpParameter = Mathf.Clamp(_colorLerpParameter, 0.0f, 1.0f);

    _lerpedColor = Color.Lerp(Color.red, _originalColor, _colorLerpParameter);

    SpriteRendererComponent.color = _lerpedColor;
  }

  void FixedUpdate()
  {
    if (_app.IsGameOver)
    {
      return;
    }

    _direction = _player.RigidbodyComponent.position - RigidbodyComponent.position;
    _direction.Normalize();

    _damageIndicatorBar.transform.position = RigidbodyComponent.position;

    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_moveSpeed * Time.fixedDeltaTime));

    HitpointsText.text = _hitpoints.ToString();
  }
}
