using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteEmitter : MonoBehaviour {
  [SerializeField]
  private SpriteRenderer _prefab;

  [SerializeField]
  private float _radius;

  [SerializeField]
  private float _cooldownSeconds;

  [SerializeField]
  private int _particlesToGenerate;
  
  private async void Start() {
    for (var i = 0; i < _particlesToGenerate; i++) {
      Spawn();
      await UniTask.Delay(TimeSpan.FromSeconds(_cooldownSeconds));
    }
    Destroy(gameObject);
  }

  private void Spawn() {
    Instantiate(_prefab, transform.position + (Vector3)Random.insideUnitCircle * _radius, Quaternion.identity);
  }
  
  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _radius);
  }
}
