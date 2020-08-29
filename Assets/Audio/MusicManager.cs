using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
  public enum Track {
    None,
    Menu,
    Gameplay
  }

  private static MusicManager _instance;
  public static MusicManager Instance => _instance;

  private Track _currentTrack = Track.None;

  [SerializeField]
  private AudioClip _menuTrack = null;

  [SerializeField]
  private AudioClip _gameplayTrack = null;

  private AudioSource _audioSource = null;

  void Awake() {
    if (_instance) {
      Destroy(gameObject);
    } else {
      _instance = this;
      DontDestroyOnLoad(gameObject);
    }

    _audioSource = GetComponent<AudioSource>();
  }

  public void RequestTrack(Track track) {
    if (track != _currentTrack) {
      _currentTrack = track;
      
      _audioSource.Stop();
      switch (track) {
        case Track.Menu:
          _audioSource.clip = _menuTrack;
          break;
        case Track.Gameplay:
          _audioSource.clip = _gameplayTrack;
          break;
        default:
          return;
      }

      _audioSource.Play();
    }
  }
}