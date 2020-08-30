using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FollowCamera : MonoBehaviour {
  [SerializeField]
  private Transform FollowTarget;

  [SerializeField]
  private float Margin;

  [SerializeField]
  private Vector2 minBounds = Vector2.zero;

  [SerializeField]
  private Vector2 maxBounds = Vector2.zero;

  public Vector2 PlacementPercentage {
    get {
      var position = transform.position;
      var x = Mathf.InverseLerp(minBounds.x, maxBounds.x, position.x);
      var y = Mathf.InverseLerp(minBounds.y, maxBounds.y, position.y);
      return new Vector2(x, y);
    }
  }

  private void Start() {
    if (!FollowTarget) {
      Debug.LogError("Camera has no Follow Target assigned! Turning off");
      enabled = false;
    }
  }

  // Update is called once per frame
  void Update() {
    if (!FollowTarget) {
      return;
    }

    //Check horizontal distance
    var horizontalDistance = FollowTarget.position.x - transform.position.x;
    if (Mathf.Abs(horizontalDistance) > Margin) {
      var transformPosition = transform.position;
      if (horizontalDistance > 0) {
        transformPosition.x = Mathf.Min(maxBounds.x, FollowTarget.position.x - Margin);
      } else {
        transformPosition.x = Mathf.Max(minBounds.x, FollowTarget.position.x + Margin);
      }

      transform.position = transformPosition;
    }

    //Check vertical distance
    var verticalDistance = FollowTarget.position.y - transform.position.y;
    if (Mathf.Abs(verticalDistance) > Margin) {
      var transformPosition = transform.position;
      if (verticalDistance > 0) {
        transformPosition.y = Mathf.Min(maxBounds.y, FollowTarget.position.y - Margin);
      } else {
        transformPosition.y = Mathf.Max(minBounds.y, FollowTarget.position.y + Margin);
      }

      transform.position = transformPosition;
    }
  }

  private void OnDrawGizmos() {
    Gizmos.color = Color.blue;
    Gizmos.DrawWireCube(transform.position, Vector3.one * Margin * 2.0f);

    Gizmos.color = Color.cyan;
    Gizmos.DrawWireCube((minBounds + maxBounds) / 2.0f, maxBounds - minBounds + Vector2.one * Margin * 2.0f);
  }
}