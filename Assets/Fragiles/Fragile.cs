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
  private float _maxForce = 10.0f;

  [SerializeField]
  private int _maxHits = 1;

  private int _currHits = 0;

  [SerializeField]
  private AudioClip[] _hitSounds = null;

  [SerializeField]
  private AudioClip[] _breakSounds = null;

  private void Awake() {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other) {
    Debug.Log($"Fragile interaction. Force: {_rigidbody2D.velocity.magnitude}");

    // Break if we exceeded max force
    if (other.collider.GetComponent<KillZone>()) {
      Break();
    }

    if (_rigidbody2D.velocity.magnitude > _maxForce) {
      Hit();
    }

    // Check if we still exist and we reached a goal
    if (this && other.collider.GetComponent<Goal>()) {
      OnGoalReached?.Invoke();
    }
  }

  private void Hit() {
    _currHits++;
    if (_currHits >= _maxHits) {
      Break();
      return;
    }

    AudioSourceExtension.PlaySoundFromGroup(_hitSounds);
  }

  private void Break() {
    AudioSourceExtension.PlaySoundFromGroup(_breakSounds);
    OnBreak?.Invoke();
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponent<KillZone>()) {
      Break();
    }
  }
}