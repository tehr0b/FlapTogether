using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {
    [SerializeField]
    private SceneReference _mainMenuScene;

    private void Start() {
        MusicManager.Instance.RequestTrack(MusicManager.Track.Menu);
    }

    [UsedImplicitly]
    public void OnMainMenuPressed() {
        SceneManager.LoadScene(_mainMenuScene);
    }
}
