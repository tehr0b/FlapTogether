using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  [SerializeField]
  private SceneReference _firstLevel;

  [UsedImplicitly]
  public void NewGamePressed() {
    
    SceneManager.LoadScene(_firstLevel);
  }

  public async Task<KeyCode> WaitForNextKeyboardInput() {
    
    while (true) {
      if (Input.anyKeyDown) {
        for (var i = 0; i < (int)KeyCode.Joystick8Button19; i++) {
          if (Input.GetKeyDown((KeyCode) i)) {
            return (KeyCode) i;
          }
        }
      }

      await Task.Delay(TimeSpan.FromMilliseconds(50));
    }
  }

  [UsedImplicitly]
  public void QuitPressed() {
    Application.Quit();
  }
}