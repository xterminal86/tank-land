using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour 
{
  public Image Bar;

  float _barDelta = 1.0f;
  Vector2 _barSize = Vector2.one;
  public void Setup(int maxHitpoints)
  {
    _barDelta = 1.0f / (float)maxHitpoints;

    _barSize.y = Bar.rectTransform.sizeDelta.y;

    Bar.rectTransform.sizeDelta = _barSize;
  }

  public void Damage(int damageReceived)
  {
    _barSize.x -= _barDelta * (float)damageReceived;
    Bar.rectTransform.sizeDelta = _barSize;
  }
}
