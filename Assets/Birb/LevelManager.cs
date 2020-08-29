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

  private bool committedEnding = false;

  void Start() {
    Assert.IsNotNull(fragile, "Fragile must be set in the Level Manager!");
    Assert.IsNotNull(eventText, "Event Text must be set in the Level Manager!");
    fragile.OnBreak += OnFragileBreak;
    fragile.OnGoalReached += OnGoalReached;
    
    CountdownLevelStart();
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

  private void OnFragileBreak() {
    if (committedEnding) {
      return;
    }
    
    eventText.text = "You broke it!";

    committedEnding = true;
    
    WaitAndRestartLevel();
  }

  private async void WaitAndRestartLevel() {
    await Task.Delay(TimeSpan.FromSeconds(3));
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  private void OnGoalReached() {
    if (committedEnding) {
      return;
    }
    
    eventText.text = "You delivered it!";
    
    committedEnding = true;
    
    // SceneManager.LoadScene(nextLevel);
    WaitAndRestartLevel();
  }

  private void Quit() {
    if (committedEnding) {
      return;
    }
    SceneManager.LoadScene(menuScreen);
  }
}