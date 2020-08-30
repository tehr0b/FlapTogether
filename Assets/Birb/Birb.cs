using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class Birb : MonoBehaviour, ITakesContinuousForce {
  private Rigidbody2D _rigidbody2D;
  public Rigidbody2D Rigidbody2D => _rigidbody2D;
  private Animator _animator;
  private SpriteRenderer _spriteRenderer;

  [SerializeField]
  private AudioClip[] _flapSounds = null;

  [SerializeField]
  private AudioClip[] _bonkSounds = null;

  [SerializeField]
  private ScriptableValue flapCooldownSeconds = null;

  [SerializeField]
  private ScriptableValue flappingAnimationSeconds = null;

  [SerializeField]
  private ScriptableValue flapStrength = null;

  [SerializeField]
  private ScriptableValue stunSeconds = null;

  [SerializeField]
  private Vector2 flapDirection = Vector2.up;

  [SerializeField]
  private int playerIndex = 0;

  private static readonly int Flapping = Animator.StringToHash("Flapping");
  private bool FlapPressed => Input.GetKeyDown(BirbControlSingleton.GetKeycodeByPlayerIndex(playerIndex));

  private bool _flapOnCooldown;
  private bool _stunned;

  private readonly HashSet<CloudZone> _currentClouds = new HashSet<CloudZone>();

  private bool CanFlap => !_flapOnCooldown && !_stunned && _currentClouds.Count == 0;

  private void Awake() {
    _rigidbody2D = GetComponent<Rigidbody2D>();
    _animator = GetComponent<Animator>();
    _spriteRenderer = GetComponent<SpriteRenderer>();

    flapDirection = flapDirection.normalized;
  }

  private void Start() {
    if (stunObject) {
      stunObject.SetActive(false);
    }
  }

  void Update() {
    // We turn down timescale in the pre-game countdown. This is a bad hack. ¯\_(ツ)_/¯
    if (Time.timeScale < 1.0f) {
      return;
    }

    // Poll flap input
    if (CanFlap && FlapPressed) {
      Flap();
    }

    if (_rigidbody2D.velocity.x > 0.1f) {
      _spriteRenderer.flipX = true;
    } else if (_rigidbody2D.velocity.x < -0.1f) {
      _spriteRenderer.flipX = false;
    }
  }

  private CancellationTokenSource _flapCancellationTokenSource;

  private void Flap() {
    AudioSourceExtension.PlaySoundFromGroup(_flapSounds);

    _rigidbody2D.AddForce(flapDirection * flapStrength.Value, ForceMode2D.Impulse);
    WaitToEnableFlap(flapCooldownSeconds.Value);

    _flapCancellationTokenSource?.Cancel();
    _flapCancellationTokenSource = new CancellationTokenSource();
#pragma warning disable 4014
    PlayFlappingAnimation(flappingAnimationSeconds.Value, _flapCancellationTokenSource.Token);
#pragma warning restore 4014
  }

  private async void WaitToEnableFlap(float seconds) {
    _flapOnCooldown = true;
    await Task.Delay(TimeSpan.FromSeconds(seconds));
    _flapOnCooldown = false;
  }

  private async Task PlayFlappingAnimation(float seconds, CancellationToken token) {
    _animator.SetBool(Flapping, true);
    await Task.Delay(TimeSpan.FromSeconds(seconds), token);
    _animator.SetBool(Flapping, false);
  }

  private void OnCollisionEnter2D(Collision2D other) {
    AddCloud(other.collider.GetComponent<CloudZone>());

    if (other.collider.GetComponent<IStunGeometry>() != null) {
      Stun(stunSeconds.Value);
    }

    if (other.collider.GetComponent<KillZone>()) {
      Destroy(gameObject);
    }
  }

  private void OnTriggerEnter2D(Collider2D other) {
    AddCloud(other.GetComponent<CloudZone>());

    if (other.GetComponent<KillZone>()) {
      Destroy(gameObject);
    }
  }

  private void AddCloud(CloudZone cloud) {
    if (cloud) {
      _currentClouds.Add(cloud);
    }
  }

  private void OnCollisionExit2D(Collision2D other) {
    TryRemoveCloud(other.collider.GetComponent<CloudZone>());
  }

  private void OnTriggerExit2D(Collider2D other) {
    TryRemoveCloud(other.GetComponent<CloudZone>());
  }

  private void TryRemoveCloud(CloudZone cloud) {
    if (cloud) {
      _currentClouds.Remove(cloud);
    }
  }

  [SerializeField]
  private GameObject stunObject;

  private static readonly int Stunned = Animator.StringToHash("Stun");

  private async void Stun(float seconds) {
    AudioSourceExtension.PlaySoundFromGroup(_bonkSounds);

    _stunned = true;
    _animator.SetBool(Stunned, true);
    if (stunObject) {
      stunObject.SetActive(true);
    }

    await Task.Delay(TimeSpan.FromSeconds(seconds));
    
    _stunned = false;
    _animator.SetBool(Stunned, false);
    if (stunObject) {
      stunObject.SetActive(false);
    }
  }
}