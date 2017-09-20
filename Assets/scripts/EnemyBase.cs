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

  public void ReceiveDamage(int damageReceived)
  {
    _damageShowTimeout = 0.0f;
    _showDamageBar = true;

    _hitpoints -= damageReceived;

    _damageIndicatorBar.Damage(damageReceived);

    if (_hitpoints <= 0)
    {
      var explosion = Instantiate(DeathAnimation, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);

      Destroy(explosion, 2.0f);

      Destroy(_damageIndicatorBar.gameObject);
      Destroy(gameObject);
    }
  }

  protected float _colorLerpParameter = 0.0f;

  Color _lerpedColor = Color.white;

  float _damageShowTimeout = 0.0f;
  bool _showDamageBar = false;
  void Update()
  {
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
    if (_player == null)
    {
      return;
    }

    _direction = _player.RigidbodyComponent.position - RigidbodyComponent.position;
    _direction.Normalize();

    _damageIndicatorBar.transform.position = RigidbodyComponent.position;

    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (GlobalConstants.EnemyWeakSpeed * Time.fixedDeltaTime));

    HitpointsText.text = _hitpoints.ToString();
  }
}
