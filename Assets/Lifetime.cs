using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Lifetime : MonoBehaviour {
  [SerializeField]
  private float _lifetimeSeconds;

  async void Start() {
    await UniTask.Delay(TimeSpan.FromSeconds(_lifetimeSeconds));
    Destroy(gameObject);
  }
}