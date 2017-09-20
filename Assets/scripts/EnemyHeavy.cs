using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavy : EnemyBase
{
  protected override void Init()
  {
    base.Init();

    _damageIndicatorBar.Setup(GlobalConstants.EnemyHeavyHitpoints, this);

    _defence = GlobalConstants.EnemyHeavyDefence;
    _hitpoints = GlobalConstants.EnemyHeavyHitpoints;
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
}
