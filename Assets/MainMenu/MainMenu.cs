using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

    var p1 = WaitForNextKeyboardInput(0);
    await p1;
    var p2 = WaitForNextKeyboardInput(1, p1.Result);
    await p2;
    var p3 = WaitForNextKeyboardInput(2, p1.Result, p2.Result);
    await p3;

    _playerInputTextLabel.text = "Press Return to continue or Escape to return to the Main Menu.";
    var cancellationTokenSource = new CancellationTokenSource();
    var waitEnter = WaitForSpecificKeyboardInput(KeyCode.Return, cancellationTokenSource.Token);
    var waitEscape = WaitForSpecificKeyboardInput(KeyCode.Escape, cancellationTokenSource.Token);

    await Task.WhenAny(waitEnter, waitEscape);

    // By here, one should be completed. Cancel the other.
    cancellationTokenSource.Cancel();

    // If the escape was complete, go back to main menu. Otherwise, continue on.
    if (waitEscape.IsCompleted) {
      return false;
    }

    BirbControlSingleton.SetKeycodes(new[] {p1.Result, p2.Result, p3.Result});
    return true;
  }

  [SerializeField]
  private Text _playerInputTextLabel = null;

  [SerializeField]
  private Text[] _birdInputTexts = null;

  private async Task<KeyCode> WaitForNextKeyboardInput(int index, params KeyCode[] extraDisallows) {
    _playerInputTextLabel.text = $"Press the input key for player {index + 1}";

    while (true) {
      if (Input.anyKey) {
        for (var i = 0; i < (int) KeyCode.Joystick8Button19; i++) {
          if (_disallowedKeycodes.Contains((KeyCode) i) || extraDisallows.Contains((KeyCode) i)) {
            continue;
          }

          if (Input.GetKey((KeyCode) i)) {
            _birdInputTexts[index].text = $"{(KeyCode) i}";
            await Task.Delay(TimeSpan.FromMilliseconds(50));
            return (KeyCode) i;
          }
        }
      }

      await Task.Delay(TimeSpan.FromMilliseconds(50));
    }
  }

  private async Task WaitForSpecificKeyboardInput(KeyCode keyCode, CancellationToken cancellationToken) {
    while (!cancellationToken.IsCancellationRequested) {
      if (Input.GetKey(keyCode)) {
        return;
      }

      await Task.Delay(TimeSpan.FromMilliseconds(50));
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