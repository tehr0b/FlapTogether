using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
  
  [SerializeField]
  private GameObject _hitObjectPrefab;

  [SerializeField]
  private GameObject _destroyObjectPrefab;

  [SerializeField]
  private GameObject _warningChild;

  private void Awake() {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  private void Update() {
    if (_warningChild && _currHits >= (_maxHits-1)) {
      _warningChild.SetActive(_rigidbody2D.velocity.magnitude > _maxForce);
    }
  }

  private void OnCollisionEnter2D(Collision2D other) {
    Debug.Log($"Fragile interaction. Force: {other.relativeVelocity.magnitude}");

    if (other.collider.GetComponent<KillZone>()) {
      Break(other);
    }

    // Break if we exceeded max force OR the other exceeded the max force
    if (other.relativeVelocity.magnitude > _maxForce) {
      Hit(other);
    }

    // Check if we still exist and we reached a goal
    if (this && other.collider.GetComponent<Goal>()) {
      OnGoalReached?.Invoke();
    }
  }

  private bool _protec;

  [SerializeField]
  private float _protecSeconds = 0.5f;

  private async void Hit(Collision2D collision) {
    if (_protec) return;
    
    _currHits++;
    if (_currHits >= _maxHits) {
      Break(collision);
      return;
    }

    AudioSourceExtension.PlaySoundFromGroup(_hitSounds);

    if (_hitObjectPrefab) {
      Instantiate(_hitObjectPrefab, collision?.contacts[0].point ?? transform.position, Quaternion.identity);
    }

    _protec = true;
    await UniTask.Delay(TimeSpan.FromSeconds(_protecSeconds));
    _protec = false;
  }

  private void Break(Collision2D collision) {
    AudioSourceExtension.PlaySoundFromGroup(_breakSounds); 
    if (_destroyObjectPrefab) {
      Instantiate(_destroyObjectPrefab, collision?.contacts[0].point ?? transform.position, Quaternion.identity);
    }
    
    OnBreak?.Invoke();
    Destroy(gameObject);
  }

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.GetComponent<KillZone>()) {
      Break(null);
    }
  }
}