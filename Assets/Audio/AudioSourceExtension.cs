using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSourceExtension {
  public static void PlaySoundFromGroup(AudioClip[] clips) {
    if (clips.Length > 0) {
      AudioSource.PlayClipAtPoint(clips[Random.Range(0, clips.Length)], Camera.main.transform.position);
    }
  }
}