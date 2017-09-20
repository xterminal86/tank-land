using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour 
{
  public Image Bar;

  float _barDelta = 1.0f;
  Vector2 _barSize = Vector2.one;
  Vector2 _barPosition = Vector2.zero;
  Vector2 _originalBarPosition = Vector2.zero;
  void Awake()
  {
    _originalBarPosition = Bar.rectTransform.localPosition;
  }

  public void Setup(int maxHitpoints, EnemyBase enemyType)
  {
    _barDelta = 1.0f / (float)maxHitpoints;

    _barSize.y = Bar.rectTransform.sizeDelta.y;

    if (enemyType is EnemyWeak) _barPosition.y = _originalBarPosition.y;
    if (enemyType is EnemyMedium) _barPosition.y = _originalBarPosition.y + 0.25f;
    if (enemyType is EnemyHeavy) _barPosition.y = _originalBarPosition.y + 0.5f;      

    Bar.rectTransform.localPosition = _barPosition;
    Bar.rectTransform.sizeDelta = _barSize;
  }

  public void Damage(int damageReceived)
  {
    _barSize.x -= _barDelta * (float)damageReceived;
    Bar.rectTransform.sizeDelta = _barSize;
  }
}
