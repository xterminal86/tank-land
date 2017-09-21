using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpread : BulletBase 
{
  public List<BulletLame> Bullets;

  List<Vector2> _directions = new List<Vector2>();
  public override void Propel(Vector2 origin, Vector2 direction, float angle, float bulletSpeed = 1)
  { 
    bool isBulletsCountEven = (Bullets.Count % 2 == 0);
    int bulletsEven = isBulletsCountEven ? Bullets.Count : Bullets.Count - 1;
    int indexToSkip = Bullets.Count / 2;
    float angleDelta = GlobalConstants.BulletSpreadArcAngle / bulletsEven;
    float startingAngle = angle - angleDelta * (bulletsEven / 2);

    for (int i = 0; i <= Bullets.Count; i++)
    { 
      if (isBulletsCountEven && i == indexToSkip)
      {
        continue;
      }

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
