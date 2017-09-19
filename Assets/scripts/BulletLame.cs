using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLame : BulletBase 
{
  void OnTriggerEnter2D(Collider2D collision)
  {
    _stopMoving = true;

    AnimationComponent.SetTrigger("bullet-hit");

    Destroy(gameObject, 1.0f);
  }
}
