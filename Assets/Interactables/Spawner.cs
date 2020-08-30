using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
  [SerializeField]
  private GameObject _spawnee = null;

  [SerializeField]
  private Vector2 _minStartingForce;

  [SerializeField]
  private Vector2 _maxStartingForce;

  [SerializeField]
  private float _secondsBetweenSpawns;

  private float _secondsSinceLastSpawn;

  void Update() {
    _secondsSinceLastSpawn += Time.deltaTime;
    while (_secondsSinceLastSpawn > _secondsBetweenSpawns) {
      Spawn();
      _secondsSinceLastSpawn -= _secondsBetweenSpawns;
    }
  }

  void Spawn() {
    var spawned = Instantiate(_spawnee, transform.position, Quaternion.identity);
    Debug.Log("Spawn attempt");
    var rigidBody = spawned.GetComponent<Rigidbody2D>();
    if (rigidBody) {
      rigidBody.AddForce(
        new Vector2(Random.Range(_minStartingForce.x, _maxStartingForce.x),
          Random.Range(_minStartingForce.y, _maxStartingForce.y)), ForceMode2D.Impulse);
    }
  }
}