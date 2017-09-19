using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankPlayer : MonoBehaviour 
{
  public Animator AnimationComponent;
  public Rigidbody2D RigidbodyComponent;

  public Transform ShotPoint;

  public Image BulletTypeSprite;

  public List<GameObject> Bullets;
  public List<Sprite> WeaponIcons;

  GlobalConstants.BulletType _bulletType = GlobalConstants.BulletType.LAME;

  bool _isMoving = false;

  float _acceleration = 0.0f;

  void Awake()
  {
    BulletTypeSprite.sprite = WeaponIcons[(int)_bulletType];
  }

  Vector3 _cameraPosition = Vector3.zero;
  void Update()
  { 
    _cameraPosition.x = transform.position.x;
    _cameraPosition.y = transform.position.y;
    _cameraPosition.z = -5.0f;

    Camera.main.transform.position = _cameraPosition;

    if (Input.GetKeyDown(KeyCode.X))
    {
      GameObject b = Instantiate(Bullets[(int)_bulletType], new Vector3(ShotPoint.position.x, ShotPoint.position.y, ShotPoint.position.z), Quaternion.identity);
      b.GetComponent<BulletBase>().Propel(new Vector3(_cos, _sin, 0.0f), GlobalConstants.BulletSpeedByType[_bulletType]);
    }

    if (Input.GetKeyDown(KeyCode.W))
    {
      int newType = (int)_bulletType + 1;
      if (newType > (int)GlobalConstants.BulletType.MAX - 1)
      {
        newType = (int)GlobalConstants.BulletType.MAX - 1;
      }

      _bulletType = (GlobalConstants.BulletType)newType;

      BulletTypeSprite.sprite = WeaponIcons[(int)_bulletType];
    }
    else if (Input.GetKeyDown(KeyCode.Q))
    {
      int newType = (int)_bulletType - 1;
      if (newType < 0)
      {
        newType = 0;
      }

      _bulletType = (GlobalConstants.BulletType)newType;

      BulletTypeSprite.sprite = WeaponIcons[(int)_bulletType];
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
