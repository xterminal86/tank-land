using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour 
{
  public Rigidbody2D RigidbodyComponent;
  public Animator AnimationComponent;

  protected bool _stopMoving = false;

  protected float _bulletSpeed = 0.0f;

  protected Vector2 _direction = Vector2.zero;
  public virtual void Propel(Vector2 direction, float angle, float bulletSpeed = 1.0f)
  {
    _direction = direction;
    _bulletSpeed = bulletSpeed;
  }

  void FixedUpdate()
  {
    if (_stopMoving)
    {
      return;
    }

    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_bulletSpeed * Time.fixedDeltaTime));

    if (RigidbodyComponent.position.x > GlobalConstants.MapSize || RigidbodyComponent.position.x < -1.0f ||
        RigidbodyComponent.position.y > GlobalConstants.MapSize || RigidbodyComponent.position.y < -1.0f)
    {
      Destroy(gameObject);
      return;
    }
  }
}
