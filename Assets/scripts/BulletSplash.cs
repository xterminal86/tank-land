using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : BulletBase 
{
  void OnCollisionEnter2D(Collision2D collision)
  {
    _stopMoving = true;

    AnimationComponent.SetTrigger("bullet-hit");

    Destroy(gameObject, 1.0f);

    if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
    {
      var enemy = collision.gameObject.GetComponent<EnemyBase>();

      float damageDealt = GlobalConstants.BulletSplashDamage * enemy.Defence;

      enemy.ReceiveDamage(damageDealt);
    }
  }
}
