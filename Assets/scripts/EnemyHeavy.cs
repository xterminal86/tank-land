using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : EnemyBase
{
  public BulletEnemyHeavy BulletPrefab;
  public AudioSource BulletFireSound;

  protected override void Init()
  {
    base.Init();

    _damageIndicatorBar.Setup(GlobalConstants.EnemyHeavyHitpoints, this);

    _defence = GlobalConstants.EnemyHeavyDefence;
    _hitpoints = GlobalConstants.EnemyHeavyHitpoints;
    _moveSpeed = GlobalConstants.EnemyHeavySpeed;
  }

  void OnCollisionEnter2D(Collision2D collision)
  { 
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles") 
      && collision.gameObject.layer != LayerMask.NameToLayer("Enemies"))
    {
      string l = LayerMask.LayerToName(collision.gameObject.layer);

      if (l == "Player")
      {
        AttackSound.Play();

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
  protected override void Update()
  {
    base.Update();

    if (_app.IsGameOver) return;

    // Fire at player

    _timer += Time.smoothDeltaTime;

    if (_timer > GlobalConstants.EnemyHeavyFireTimeout)
    {
      _timer = 0.0f;

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
        BulletFireSound.Play();

        var go = Instantiate(BulletPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);
        var dir = _player.RigidbodyComponent.position - RigidbodyComponent.position;

        go.GetComponent<BulletEnemyHeavy>().Propel(RigidbodyComponent.position, dir.normalized, -1.0f, GlobalConstants.EnemyHeavyBulletSpeed);
      }
    }
  }
}
