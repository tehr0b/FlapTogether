using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITakesContinuousForce {
  Rigidbody2D Rigidbody2D { get; }
}

public class ConintuousForceZone : MonoBehaviour {
  private readonly HashSet<ITakesContinuousForce> Targets = new HashSet<ITakesContinuousForce>();

  [SerializeField]
  private Vector2 _continuousForce = Vector2.zero;

  void LateUpdate() {
    foreach (var target in Targets) {
      target.Rigidbody2D.AddForce(_continuousForce * Time.deltaTime, ForceMode2D.Force);
        }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    var forceTaker = other.GetComponent<ITakesContinuousForce>();
    if (forceTaker != null) {
      Targets.Add(forceTaker);
        }
  }

  private void OnTriggerExit2D(Collider2D other) {
    var forceTaker = other.GetComponent<ITakesContinuousForce>();
    if (forceTaker != null) {
      Targets.Remove(forceTaker);
        }
  }
}