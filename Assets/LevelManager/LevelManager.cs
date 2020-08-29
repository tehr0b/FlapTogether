using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manager for the current level.
/// Needs to be configured for each level.
/// </summary>
public class LevelManager : MonoBehaviour {
  [SerializeField]
  private Fragile fragile = null;

  [SerializeField]
  private SceneReference nextLevel = null;

  [SerializeField]
  private SceneReference menuScreen = null;

  [SerializeField]
  private Text eventText = null;

  [SerializeField]
  private AudioClip[] _winSounds = null;

  [SerializeField]
  private AudioClip[] _loseSounds = null;

  private bool committedEnding = false;

  void Start() {
    Assert.IsNotNull(fragile, "Fragile must be set in the Level Manager!");
    Assert.IsNotNull(eventText, "Event Text must be set in the Level Manager!");
    fragile.OnBreak += OnFragileBreak;
    fragile.OnGoalReached += OnGoalReached;

    CountdownLevelStart();

    MusicManager.Instance.RequestTrack(MusicManager.Track.Gameplay);
  }

  private async void CountdownLevelStart() {
    Time.timeScale = 0.0f;
    eventText.text = "3!";
    await Task.Delay(TimeSpan.FromSeconds(1));
    eventText.text = "2!";
    await Task.Delay(TimeSpan.FromSeconds(1));
    eventText.text = "1!";
    await Task.Delay(TimeSpan.FromSeconds(1));
    eventText.text = "Go!";
    await Task.Delay(TimeSpan.FromSeconds(1));
    Time.timeScale = 1.0f;
    await Task.Delay(TimeSpan.FromSeconds(1));
    eventText.text = "";
  }

  void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Quit();
    }
  }

  private async void OnFragileBreak() {
    if (committedEnding) {
      return;
    }

    eventText.text = "You broke it!";
    committedEnding = true;

    await Task.Delay(TimeSpan.FromSeconds(3));
    
    AudioSourceExtension.PlaySoundFromGroup(_loseSounds);
    
    await Task.Delay(TimeSpan.FromSeconds(3));

    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  private async void OnGoalReached() {
    if (committedEnding) {
      return;
    }

    eventText.text = "You delivered it!";
    committedEnding = true;

    AudioSourceExtension.PlaySoundFromGroup(_winSounds);

    await Task.Delay(TimeSpan.FromSeconds(3));

    SceneManager.LoadScene(nextLevel ?? menuScreen);
  }

  private void Quit() {
    if (committedEnding) {
      return;
    }

    SceneManager.LoadScene(menuScreen);
  }
}