using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour 
{
  public Animator AnimationComponent;

  protected float _bulletSpeed = 1.0f;
  protected bool _stopMoving = false;

  public void Propel(Vector3 direction, float bulletSpeed = 1.0f)
  {
    _position = transform.localPosition;
    _direction = direction;
    _bulletSpeed = bulletSpeed;
  }

  Vector3 _position = Vector3.zero;
  Vector3 _direction = Vector3.zero;
  void Update()
  {
    if (_position.x > GlobalConstants.MapSize || _position.x < 0.0f ||
        _position.y > GlobalConstants.MapSize || _position.y < 0.0f)
    {
      Destroy(gameObject);
      return;
    }

    if (_stopMoving)
    {
      return;
    }
    
    _position.x += _direction.x * (Time.smoothDeltaTime * _bulletSpeed);
    _position.y += _direction.y * (Time.smoothDeltaTime * _bulletSpeed);

    transform.localPosition = _position;
  }
}
