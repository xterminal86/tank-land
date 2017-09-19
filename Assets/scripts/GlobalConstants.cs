using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
  public const int MapSize = 64;
  public const int MaxWeaponIndex = 1;

  public const float BulletLameSpeed = 20.0f;
  public const float BulletSplashSpeed = 10.0f;
  public const float TankMoveSpeed = 6.0f;
  public const float TankRotationSpeed = 2.0f;

  public enum BulletType
  {
    LAME = 0,
    SPLASH = 1,
    MAX
  }

  public static Dictionary<BulletType, float> BulletSpeedByType = new Dictionary<BulletType, float>() 
  {
    { BulletType.LAME, BulletLameSpeed },
    { BulletType.SPLASH, BulletSplashSpeed }
  };
}
