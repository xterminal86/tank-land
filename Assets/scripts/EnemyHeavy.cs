using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : EnemyBase
{
  public BulletEnemyHeavy BulletPrefab;
  public Transform FireIndicator;

  public PolygonCollider2D ColliderComponent;

  float _fireIndicatorDelta = 0.0f;
  protected override void Init()
  {
    base.Init();

    _damageIndicatorBar.Setup(GlobalConstants.EnemyHeavyHitpoints, this);

    _defence = GlobalConstants.EnemyHeavyDefence;
    _hitpoints = GlobalConstants.EnemyHeavyHitpoints;
    _moveSpeed = GlobalConstants.EnemyHeavySpeed;

    _fireIndicatorDelta = 1.0f / GlobalConstants.EnemyHeavyFireTimeout;
  }

  void OnCollisionEnter2D(Collision2D collision)
  { 
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles") 
      && collision.gameObject.layer != LayerMask.NameToLayer("Enemies"))
    {
      string l = LayerMask.LayerToName(collision.gameObject.layer);

      if (l == "Player")
      {
        _app.PlaySound(_app.PlayerHitSound);

        int damageDealt = (int)((float)GlobalConstants.EnemyHeavyDamage * GlobalConstants.TankDefence);

        _player.Push(_direction * GlobalConstants.EnemyPushForceFactor);
        _player.ReceiveDamage(damageDealt);
      }
      else
      {
        _colorLerpParameter = 0.0f;
      }
    }
  }

  float _timer = 0.0f;
  float _firingRadius = 20.0f;
  float _currentFireReadyIndicatorScale = 0.0f;
  Vector3 _fireIndicatorScale = Vector3.one;
  protected override void Update()
  {
    base.Update();

    if (_app.IsGameOver) return;

    _timer += Time.smoothDeltaTime;

    _currentFireReadyIndicatorScale = _timer * _fireIndicatorDelta;
    _currentFireReadyIndicatorScale = Mathf.Clamp(_currentFireReadyIndicatorScale, 0.0f, 1.0f);
    _fireIndicatorScale.Set(_currentFireReadyIndicatorScale, _currentFireReadyIndicatorScale, _currentFireReadyIndicatorScale);

    FireIndicator.localScale = _fireIndicatorScale;

    // Fire at player

    if (_timer > GlobalConstants.EnemyHeavyFireTimeout)
    {      
      float px = _player.RigidbodyComponent.position.x;
      float py = _player.RigidbodyComponent.position.y;
      float ex = RigidbodyComponent.position.x;
      float ey = RigidbodyComponent.position.y;

      float lx = ex - _firingRadius;
      float ly = ey - _firingRadius;
      float hx = ex + _firingRadius;
      float hy = ey + _firingRadius;

      // If player in range
      if (px > lx && px < hx && py > ly && py < hy)
      {
        _timer = 0.0f;

        _app.PlaySound(_app.ShotSounds[3]);

        var go = Instantiate(BulletPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);
        var dir = _player.RigidbodyComponent.position - RigidbodyComponent.position;
        var bulletCollider = go.GetComponent<BoxCollider2D>();

        // Prevent self shot
        Physics2D.IgnoreCollision(bulletCollider, ColliderComponent);

        go.GetComponent<BulletEnemyHeavy>().Propel(RigidbodyComponent.position, dir.normalized, -1.0f, GlobalConstants.EnemyHeavyBulletSpeed);
      }
    }
  }
}
