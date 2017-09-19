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

  float _moveSpeed = 0.5f;
  float _rotationSpeed = 10.0f;

  float _acceleration = 0.0f;

  Vector3 _cameraPosition = Vector3.zero;
  Vector3 _position = Vector3.zero;
  Vector3 _eulerAngles = Vector3.zero;
  void Update()
  {
    _isMoving = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow);

    if (Input.GetKey(KeyCode.LeftArrow))
    {
      _eulerAngles.z++;
    } 
    else if (Input.GetKey(KeyCode.RightArrow))
    {
      _eulerAngles.z--;
    }

    _acceleration = Input.GetAxis("Vertical");

    AnimationComponent.SetBool("IsMoving", _isMoving);

    float cos = Mathf.Cos(_eulerAngles.z * Mathf.Deg2Rad);
    float sin = Mathf.Sin(_eulerAngles.z * Mathf.Deg2Rad);
    float dx = cos * _rotationSpeed * _acceleration;
    float dy = sin * _rotationSpeed * _acceleration;

    _position.x += dx * (_moveSpeed * Time.smoothDeltaTime);
    _position.y += dy * (_moveSpeed * Time.smoothDeltaTime);

    //transform.eulerAngles = _eulerAngles;
    //transform.position = _position;

    _cameraPosition.x = _position.x;
    _cameraPosition.y = _position.y;
    _cameraPosition.z = -5.0f;

    Camera.main.transform.position = _cameraPosition;

    if (Input.GetKeyDown(KeyCode.X))
    {
      GameObject b = Instantiate(BulletLame, new Vector3(ShotPoint.position.x, ShotPoint.position.y, ShotPoint.position.z), Quaternion.identity);
      b.GetComponent<BulletLame>().Propel(new Vector3(cos, sin, 0.0f), GlobalConstants.BulletLameSpeed);
    }
  }

  void FixedUpdate()
  {
    RigidbodyComponent.MovePosition(RigidbodyComponent.position + Vector2.right * Time.smoothDeltaTime * 4.0f);
  }

  public void SetPlayerPosition(Vector3 newPosition)
  {
    _position = newPosition;
    //transform.position = _position;
    RigidbodyComponent.position = newPosition;
  }

  void OnCollisionEnter2D(Collision2D collision)
  {
    Debug.Log("here");
  }
}
