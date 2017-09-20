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

  protected TankPlayer _player;

  Vector2 _direction = Vector2.zero;

  protected Color _originalColor = Color.white;

  protected float _hitpoints = 1.0f;
  protected float _defence = 1.0f;
  public float Defence
  {
    get { return _defence; }
  }

  void Awake()
  { 
    Init();
  }

  protected virtual void Init()
  {
    _originalColor = SpriteRendererComponent.color;

    _player = GameObject.Find("tank-player").GetComponent<TankPlayer>();

    _direction = _player.RigidbodyComponent.position - RigidbodyComponent.position;
    _direction.Normalize();
  }

  public void ReceiveDamage(float damageReceived)
  {
    _hitpoints -= damageReceived;

    if (_hitpoints < 0.0f)
    {
      var explosion = Instantiate(DeathAnimation, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);

      Destroy(explosion, 2.0f);

      Destroy(gameObject);
    }
  }

  void FixedUpdate()
  {
    _direction = _player.RigidbodyComponent.position - RigidbodyComponent.position;
    _direction.Normalize();

    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (GlobalConstants.EnemyWeakSpeed * Time.fixedDeltaTime));

    HitpointsText.text = _hitpoints.ToString();
  }
}
