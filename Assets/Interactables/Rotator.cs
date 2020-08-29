using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Rotator : MonoBehaviour {
    private Rigidbody2D _rigidbody2D;

    [SerializeField]
    private float _angularVelocity = 100.0f;
    
    void Awake() {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void LateUpdate() {
        _rigidbody2D.angularVelocity = _angularVelocity;
    }
}
