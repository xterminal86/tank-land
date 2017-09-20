using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpread : BulletBase 
{
  public List<BulletLame> Bullets;

  List<Vector2> _directions = new List<Vector2>();
  public override void Propel(Vector2 origin, Vector2 direction, float angle, float bulletSpeed = 1)
  {     
    float angleDelta = GlobalConstants.BulletSpreadArcAngle / Bullets.Count;
    float startingAngle = angle - GlobalConstants.BulletSpreadArcAngle / 2.0f;

    for (int i = 0; i < Bullets.Count; i++)
    {
      float thisAngle = startingAngle + angleDelta * i;
      float cos = Mathf.Cos(thisAngle * Mathf.Deg2Rad);
      float sin = Mathf.Sin(thisAngle * Mathf.Deg2Rad);
      _directions.Add(new Vector2(cos, sin));
    }

    int index = 0;
    foreach (var item in Bullets)
    {
      item.Propel(origin, _directions[index], -1.0f, bulletSpeed);
      index++;
    }
  }

  // Disable FixedUpdate code in base class by implementing empty method here
  void FixedUpdate()
  {
  }
}
