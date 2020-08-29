using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public static class BirbControlSingleton {
  private static List<KeyCode> _keycodes = new List<KeyCode> {KeyCode.LeftArrow, KeyCode.UpArrow, KeyCode.RightArrow};
  public static IEnumerable<KeyCode> Keycodes => _keycodes;

  public static void SetKeycodes(IEnumerable<KeyCode> newKeyCodes) {
    Assert.AreEqual(3, newKeyCodes.Count());

    _keycodes.Clear();
    _keycodes.AddRange(newKeyCodes);
  }

  public static KeyCode GetKeycodeByPlayerIndex(int i) {
    return _keycodes[i];
  }
}