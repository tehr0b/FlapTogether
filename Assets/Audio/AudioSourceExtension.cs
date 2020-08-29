using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class AudioSourceExtension {
  public static void PlaySoundFromGroup(AudioClip[] clips) {
    Debug.Log("Sound");
    if (clips.Length > 0) {
      AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length)], Camera.main.transform.position);
    }
  }
}