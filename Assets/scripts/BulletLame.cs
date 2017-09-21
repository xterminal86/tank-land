using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLame : BulletBase 
{
  public GameObject BulletHitAnimationPrefab;

  void OnCollisionEnter2D(Collision2D collision)
  {     
    if (_isColliding) return;

    _isColliding = true;

    var go = Instantiate(BulletHitAnimationPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -1.0f), Quaternion.identity);

    Destroy(go, 1.0f);

    if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
    {
      var enemy = collision.gameObject.GetComponent<EnemyBase>();

      if (!_appRef.EnemyHitSound.isPlaying)
      {
        _appRef.EnemyHitSound.Play();
      }

      int damageDealt = (int)((float)GlobalConstants.BulletLameDamage * enemy.Defence);

      enemy.ReceiveDamage(damageDealt);
    }
    else
    {
      go.GetComponent<AudioSource>().Play();
    }

    Destroy(gameObject);
  }
}
