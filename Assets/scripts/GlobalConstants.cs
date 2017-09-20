using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
  public const int MapSize = 80;
  public const int MaxWeaponIndex = 1;
  public const int MaxEnemies = 10;
  public const float SpawnTimeout = 3.0f;

  public const int BulletLameCooldown = 100;
  public const int BulletSpreadCooldown = 1000;
  public const int BulletSplashCooldown = 3000;

  public const int TankHitpoints = 400;
  public const float TankDefence = 0.5f;
  public const int TankRamDamage = 5;

  public const int EnemyWeakHitpoints = 20;
  public const float EnemyWeakDefence = 0.8f;
  public const int EnemyWeakDamage = 10;
  public const float EnemyWeakSpeed = 4.0f;
  public const int EnemyWeakScore = 10;
    
  public const int EnemyMediumHitpoints = 40;
  public const float EnemyMediumDefence = 0.5f;
  public const int EnemyMediumDamage = 20;
  public const float EnemyMediumSpeed = 2.0f;
  public const int EnemyMediumScore = 20;

  public const int EnemyHeavyHitpoints = 80;
  public const float EnemyHeavyDefence = 0.25f;
  public const int EnemyHeavyDamage = 60;
  public const float EnemyHeavyFireTimeout = 5.0f;
  public const float EnemyHeavySpeed = 1.0f;
  public const float EnemyHeavyBulletSpeed = 20.0f;
  public const int EnemyHeavyScore = 40;

  public const float BulletLameSpeed = 20.0f;
  public const float BulletSplashSpeed = 10.0f;
  public const int BulletLameDamage = 10;
  public const int BulletSplashDamage = 80;
  public const float BulletSplashRadius = 5.0f;
  public const float BulletSpreadArcAngle = 60.0f;

  public const float TankMoveSpeed = 6.0f;
  public const float TankRotationSpeed = 2.0f;

  public const float EnemyPushForceFactor = 10.0f;

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
