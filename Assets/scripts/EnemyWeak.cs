using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeak : EnemyBase
{ 
  protected override void Init()
  {
    base.Init();
  
    _damageIndicatorBar.Setup(GlobalConstants.EnemyWeakHitpoints, this);

    _defence = GlobalConstants.EnemyWeakDefence;
    _hitpoints = GlobalConstants.EnemyWeakHitpoints;
    _moveSpeed = GlobalConstants.EnemyWeakSpeed;
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

        int damageDealt = (int)((float)GlobalConstants.EnemyWeakDamage * GlobalConstants.TankDefence);

        //Debug.Log(_direction + " " + _direction * GlobalConstants.EnemyPushForceFactor + " "  + _player.RigidbodyComponent.velocity + " " + RigidbodyComponent.velocity);

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
