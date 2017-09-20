using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : BulletBase 
{
  public GameObject BulletHitAnimationPrefab;

  void OnCollisionEnter2D(Collision2D collision)
  {
    //AnimationComponent.SetTrigger("bullet-hit");

    var go = Instantiate(BulletHitAnimationPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);

    Destroy(go, 1.0f);

    if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
    {
      var enemy = collision.gameObject.GetComponent<EnemyBase>();

      int damageDealt = (int)((float)GlobalConstants.BulletSplashDamage * enemy.Defence);

      enemy.ReceiveDamage(damageDealt);
    }

    Destroy(gameObject);
  }
}
