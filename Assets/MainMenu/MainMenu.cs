using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
  public enum State {
    Main,
    Input,
    Credits
  }

  [SerializeField]
  private GameObject[] _mainStateAssets = null;

  [SerializeField]
  private GameObject[] _inputStateAssets = null;

  [SerializeField]
  private GameObject[] _creditsStateAssets = null;

  [SerializeField]
  private SceneReference _firstLevel;

  [SerializeField]
  private KeyCode[] _disallowedKeycodes;

  void Start() {
    MusicManager.Instance.RequestTrack(MusicManager.Track.Menu);
  }

  [UsedImplicitly]
  public async void NewGamePressed() {
    var keyboardEntry = FillOutKeyboardInputs();
    await keyboardEntry;

    if (keyboardEntry.Result) {
      SceneManager.LoadScene(_firstLevel);
    } else {
      SetState(State.Main);
    }
  }

  private async Task<bool> FillOutKeyboardInputs() {
    SetState(State.Input);

    foreach (var birdInputText in _birdInputTexts) {
      birdInputText.text = "";
    }

    var p1 = await WaitForNextKeyboardInput(0);
    var p2 = await WaitForNextKeyboardInput(1, p1);
    var p3 = await WaitForNextKeyboardInput(2, p1, p2);

    _playerInputTextLabel.text = "Press Return to continue or Escape to return to the Main Menu.";
    
    var waitEnter = await WaitForSpecificKeyboardInput(KeyCode.Return, KeyCode.Escape);
    
    // If the escape was complete, go back to main menu. Otherwise, continue on.
    if (!waitEnter) {
      return false;
    }

    BirbControlSingleton.SetKeycodes(new[] {p1, p2, p3});
    
    return true;
  }

  [SerializeField]
  private Text _playerInputTextLabel = null;

  [SerializeField]
  private Text[] _birdInputTexts = null;

  private async UniTask<KeyCode> WaitForNextKeyboardInput(int index, params KeyCode[] extraDisallows) {
    _playerInputTextLabel.text = $"Press the input key for player {index + 1}";

    while (true) {
      if (Input.anyKey) {
        for (var i = 0; i < (int) KeyCode.Joystick8Button19; i++) {
          if (_disallowedKeycodes.Contains((KeyCode) i) || extraDisallows.Contains((KeyCode) i)) {
            continue;
          }

          if (Input.GetKey((KeyCode) i)) {
            _birdInputTexts[index].text = $"{(KeyCode) i}";
            await UniTask.Delay(TimeSpan.FromMilliseconds(50));
            return (KeyCode) i;
          }
        }
      }

      await UniTask.Delay(TimeSpan.FromMilliseconds(50));
    }
  }

  private async UniTask<bool> WaitForSpecificKeyboardInput(KeyCode keyCode, KeyCode cancelKey) {
    while (true) {
      if (Input.GetKey(keyCode)) {
        return true;
      }

      if (Input.GetKey(cancelKey)) {
        return false;
      }

      await UniTask.Yield();
    }
  }

  [UsedImplicitly]
  public void SetState(State state) {
    SetEnableAssetsInState(_mainStateAssets, state == State.Main);
    SetEnableAssetsInState(_inputStateAssets, state == State.Input);
    SetEnableAssetsInState(_creditsStateAssets, state == State.Credits);
  }

  private void SetEnableAssetsInState(GameObject[] objects, bool enable) {
    foreach (var o in objects) {
      o.SetActive(enable);
    }
  }


  [UsedImplicitly]
  public void QuitPressed() {
    Application.Quit();
  }

  [UsedImplicitly]
  public void CreditsPressed() {
    SetState(State.Credits);
  }

  [UsedImplicitly]
  public void ReturnToMainMenu() {
    SetState(State.Main);
  }
}