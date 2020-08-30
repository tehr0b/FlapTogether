using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PathMover : MonoBehaviour {
  [SerializeField]
  private float speedForce;

  [SerializeField]
  private Vector2[] patrolPoints;

  private Rigidbody2D _rigidbody2D;

  private int currPatrolPointIndex;

  private float closeEnough = 0.2f;

  void Awake() {
    _rigidbody2D = GetComponent<Rigidbody2D>();
  }

  void LateUpdate() {
    var position = transform.position;
    var dist = patrolPoints[currPatrolPointIndex] - new Vector2(position.x, position.y);

    _rigidbody2D.velocity = dist.normalized * speedForce;
    // _rigidbody2D.AddForce(dist.normalized * (speedForce * Time.deltaTime), ForceMode2D.Force);
    
    if (dist.magnitude < closeEnough) {
      currPatrolPointIndex++;
      if (currPatrolPointIndex >= patrolPoints.Length) {
        currPatrolPointIndex = 0;
      }
    }
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.magenta;

    Vector2 lastPoint = transform.position;
    foreach (var patrolPoint in patrolPoints) {
      Gizmos.DrawLine(lastPoint, patrolPoint);
      Gizmos.DrawWireSphere(patrolPoint, 1.0f);
      lastPoint = patrolPoint;
    }

    Gizmos.DrawLine(lastPoint, patrolPoints[0]);
  }
}