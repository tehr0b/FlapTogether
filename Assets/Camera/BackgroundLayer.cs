using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundLayer : MonoBehaviour
{
  [SerializeField]
  private SpriteRenderer _sprite;

  [SerializeField]
  private Vector2 _minBounds;

  [SerializeField]
  private Vector2 _maxBounds;

  public void SetPercentage(Vector2 percentages) {
    _sprite.transform.localPosition = new Vector2(
      Mathf.Lerp(_minBounds.x, _maxBounds.x, percentages.x),
      Mathf.Lerp(_minBounds.y, _maxBounds.y, percentages.y)
    );
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.green;
    
    Gizmos.DrawWireCube((_minBounds + _maxBounds) / 2.0f, _maxBounds - _minBounds + (Vector2)_sprite.bounds.size);
  }
}
