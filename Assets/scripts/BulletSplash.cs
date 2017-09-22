using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSplash : BulletBase 
{
  public GameObject BulletHitAnimationPrefab;

  void OnCollisionEnter2D(Collision2D collision)
  {
    if (_isColliding) return;

    _isColliding = true;

    var go = Instantiate(BulletHitAnimationPrefab, new Vector3(RigidbodyComponent.position.x, RigidbodyComponent.position.y, -3.0f), Quaternion.identity);

    Destroy(go, 1.0f);

    var objects = Physics2D.OverlapCircleAll(RigidbodyComponent.position, GlobalConstants.BulletSplashRadius);

    AudioSource asc = go.GetComponent<AudioSource>();
    asc.Play();

    int playerLayer = LayerMask.NameToLayer("Player");
    int enemiesLayer = LayerMask.NameToLayer("Enemies");

    foreach (var obj in objects)
    {
      int layerToCheck = obj.gameObject.layer;

      if (layerToCheck == playerLayer || layerToCheck == enemiesLayer)
      {
        float distance = Vector2.Distance(RigidbodyComponent.position, obj.attachedRigidbody.position);

        if (distance < 1.0f) distance = 1.0f;

        int distanceSquared = (int)Mathf.Pow(distance, 2.0f);

        if (obj.gameObject.layer == enemiesLayer)
        {
          var enemy = obj.gameObject.GetComponentInParent<EnemyBase>();

          int damageDealt = (int)((float)(GlobalConstants.BulletSplashDamage / distanceSquared) * enemy.Defence);

          if (damageDealt != 0)
          {
            enemy.ReceiveDamage(damageDealt);
          }

          //Debug.Log(obj.attachedRigidbody.position + " took " + damageDealt + " damage");
        }
        else if (obj.gameObject.layer == playerLayer)
        {
          var player = obj.gameObject.GetComponentInParent<TankPlayer>();

          int damageDealt = (int)((float)(GlobalConstants.BulletSplashDamage / distanceSquared) * GlobalConstants.TankDefence);

          if (damageDealt != 0)
          {
            player.ReceiveDamage(damageDealt);
          }

          //Debug.Log("Player took " + damageDealt + " damage");
        }
      }
    }

    Destroy(gameObject);
  }
}
