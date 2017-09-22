using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMedium : EnemyBase 
{
  protected override void Init()
  {
    base.Init();

    _damageIndicatorBar.Setup(GlobalConstants.EnemyMediumHitpoints, this);

    _defence = GlobalConstants.EnemyMediumDefence;
    _hitpoints = GlobalConstants.EnemyMediumHitpoints;
    _moveSpeed = GlobalConstants.EnemyMediumSpeed;
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

        int damageDealt = (int)((float)GlobalConstants.EnemyMediumDamage * GlobalConstants.TankDefence);

        _player.Push(_direction * GlobalConstants.EnemyPushForceFactor);
        _player.ReceiveDamage(damageDealt);
      }
      else
      {
        _colorLerpParameter = 0.0f;
      }
    }
  }
}
