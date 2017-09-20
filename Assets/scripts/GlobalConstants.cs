using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
  public const int MapSize = 64;
  public const int MaxWeaponIndex = 1;
  public const int BulletLameCooldown = 100;
  public const int BulletSpreadCooldown = 500;
  public const int BulletSplashCooldown = 1000;

  public const float BulletLameSpeed = 20.0f;
  public const float BulletSplashSpeed = 10.0f;
  public const float BulletLameDamage = 0.1f;
  public const float BulletSplashDamage = 2.0f;
  public const float BulletSpreadArcAngle = 60.0f;

  public const float TankMoveSpeed = 6.0f;
  public const float TankRotationSpeed = 2.0f;
  public const float TankHitpoints = 20.0f;
  public const float TankDefence = 0.5f;
  public const float TankRamDamage = 0.2f;
  public const float EnemyWeakSpeed = 1.0f;
  public const float EnemyWeakHitpoints = 1.0f;
  public const float EnemyWeakDefence = 0.8f;
  public const float EnemyWeakDamage = 0.2f;

  public enum BulletType
  {
    LAME = 0,
    SPREAD,
    SPLASH,
    MAX
  }

  public static Dictionary<BulletType, float> BulletSpeedByType = new Dictionary<BulletType, float>() 
  {
    { BulletType.LAME, BulletLameSpeed },
    { BulletType.SPREAD, BulletLameSpeed },
    { BulletType.SPLASH, BulletSplashSpeed }
  };

  public static Dictionary<BulletType, int> BulletCooldownByType = new Dictionary<BulletType, int>()
  {
    { BulletType.LAME, BulletLameCooldown },
    { BulletType.SPREAD, BulletSpreadCooldown },
    { BulletType.SPLASH, BulletSplashCooldown }
  };
}
