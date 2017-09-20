using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeak : EnemyBase
{ 
  protected override void Init()
  {
    base.Init();
  
    _defence = GlobalConstants.EnemyWeakDefence;
    _hitpoints = GlobalConstants.EnemyWeakHitpoints;
  }
  
  void OnCollisionEnter2D(Collision2D collision)
  { 
    if (collision.gameObject.layer != LayerMask.NameToLayer("Obstacles") 
      && collision.gameObject.layer != LayerMask.NameToLayer("Enemies"))
    {
      string l = LayerMask.LayerToName(collision.gameObject.layer);

      if (l == "Player")
      {
        float damageDealt = GlobalConstants.EnemyWeakDamage * GlobalConstants.TankDefence;

        _player.ReceiveDamage(damageDealt);
      }
      else
      {
        _damageIndicator = 0.0f;
      }
    }
  }

  float _damageIndicator = 0.0f;

  Color _lerpedColor = Color.white;
  void Update()
  {
    _damageIndicator += Time.smoothDeltaTime;

    _damageIndicator = Mathf.Clamp(_damageIndicator, 0.0f, 1.0f);

    _lerpedColor = Color.Lerp(Color.red, _originalColor, _damageIndicator);

    SpriteRendererComponent.color = _lerpedColor;
  }
}
