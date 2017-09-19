using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPlayer : MonoBehaviour 
{
  public Animator AnimationComponent;
  public Rigidbody2D RigidbodyComponent;

  public Transform ShotPoint;

  public GameObject BulletLame;
  public GameObject BulletSplash;

  bool _isMoving = false;

  float _acceleration = 0.0f;

  Vector3 _cameraPosition = Vector3.zero;
  void Update()
  { 
    _cameraPosition.x = transform.position.x;
    _cameraPosition.y = transform.position.y;
    _cameraPosition.z = -5.0f;

    Camera.main.transform.position = _cameraPosition;

    if (Input.GetKeyDown(KeyCode.X))
    {
      GameObject b = Instantiate(BulletLame, new Vector3(ShotPoint.position.x, ShotPoint.position.y, ShotPoint.position.z), Quaternion.identity);
      b.GetComponent<BulletLame>().Propel(new Vector3(_cos, _sin, 0.0f), GlobalConstants.BulletLameSpeed);
    }
  }

  float _tankRotation = 0.0f;
  float _cos = 0.0f;
  float _sin = 0.0f;
  Vector2 _direction = Vector2.zero;
  void FixedUpdate()
  {
    _isMoving = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow);

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      _tankRotation += GlobalConstants.TankRotationSpeed;
    } 
    else if (Input.GetKey(KeyCode.RightArrow))
    {
      _tankRotation -= GlobalConstants.TankRotationSpeed;
    }

    _cos = Mathf.Cos(_tankRotation * Mathf.Deg2Rad);
    _sin = Mathf.Sin(_tankRotation * Mathf.Deg2Rad);

    _direction.x = _cos;
    _direction.y = _sin;

    _acceleration = Input.GetAxis("Vertical") * GlobalConstants.TankMoveSpeed;

    AnimationComponent.SetBool("IsMoving", _isMoving);

    RigidbodyComponent.rotation = _tankRotation;
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + _direction * (_acceleration * Time.fixedDeltaTime));
  }

  public void SetPlayerPosition(Vector3 newPosition)
  {
    RigidbodyComponent.position = newPosition;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {    
  }
}
