using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour 
{
  public Rigidbody2D RigidbodyComponent;
  public Animator AnimationComponent;

  float _viewLimitMinX = 0;
  float _viewLimitMaxX = 0;
  float _viewLimitMinY = 0;
  float _viewLimitMaxY = 0;

  // To prevent multiple collision checks when, e.g., splash bullet hits several objects
  protected bool _isColliding = false;

  protected TankPlayer _playerRef;
  protected Main _appRef;

  protected float _bulletSpeed = 0.0f;

  protected Vector2 _direction = Vector2.zero;
  protected Vector2 _bulletOrigin = Vector2.zero;

  void Awake()
  {
    _appRef = GameObject.Find("App").GetComponent<Main>();
  }

  public virtual void Propel(Vector2 origin, Vector2 direction, float angle, float bulletSpeed = 1.0f)
  {
    _playerRef = GameObject.Find("tank-player").GetComponent<TankPlayer>();

    _bulletOrigin = origin;
    _direction = direction;
    _bulletSpeed = bulletSpeed;
  }

  float _bulletVisibilityOffset = 25.0f;
  void FixedUpdate()
  {    
    if (_appRef.IsGameOver) return;

    _viewLimitMinX = _playerRef.RigidbodyComponent.position.x - _bulletVisibilityOffset;
    _viewLimitMaxX = _playerRef.RigidbodyComponent.position.x + _bulletVisibilityOffset;
    _viewLimitMinY = _playerRef.RigidbodyComponent.position.y - _bulletVisibilityOffset;
    _viewLimitMaxY = _playerRef.RigidbodyComponent.position.y + _bulletVisibilityOffset;

    //Debug.Log(_viewLimitMinX + " " + _viewLimitMaxX + " " + _viewLimitMinY + " " + _viewLimitMaxY);

    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_bulletSpeed * Time.fixedDeltaTime));

    if (RigidbodyComponent.position.x > GlobalConstants.MapSize || RigidbodyComponent.position.x < -1.0f 
     || RigidbodyComponent.position.y > GlobalConstants.MapSize || RigidbodyComponent.position.y < -1.0f
     || RigidbodyComponent.position.x < _viewLimitMinX || RigidbodyComponent.position.x > _viewLimitMaxX
     || RigidbodyComponent.position.y < _viewLimitMinY || RigidbodyComponent.position.y > _viewLimitMaxY) 
    {
      Destroy(gameObject);
      return;
    }
  }
}
