using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Fragile : MonoBehaviour, ITakesContinuousForce {
  public event Action OnBreak;
  public event Action OnGoalReached;

  private Rigidbody2D _rigidbody2D;
  public Rigidbody2D Rigidbody2D => _rigidbody2D;

  [SerializeField]
  private bool _ignoreBirbCollisions = true;

  [SerializeField]
  private float _maxForce = 10.0f;

  private void Awake() {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other) {
    // Ignore birb collisions
    if (_ignoreBirbCollisions && other.collider.GetComponent<Birb>()) {
      return;
    }
    
    Debug.Log($"Fragile interaction. Force: {_rigidbody2D.velocity.magnitude}");

    // Break if we exceeded max force
    if (_rigidbody2D.velocity.magnitude > _maxForce || other.collider.GetComponent<KillZone>()) {
      OnBreak?.Invoke();
      Destroy(gameObject);
    } else if (other.collider.GetComponent<Goal>()) { // Win if we hit goal w/o exceeding max force
      OnGoalReached?.Invoke();
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponent<KillZone>()) {
      OnBreak?.Invoke();
      Destroy(gameObject);
    }
  }
}