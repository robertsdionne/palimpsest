using UnityEngine;
using System.Collections;

public static class Utilities {	

  public static bool IsPlayer(GameObject other) {
    return "Player" == other.tag && other.activeInHierarchy;
  }

  public static string Choose(string[] choices) {
    return choices.Length > 0 ? choices[Random.Range(0, choices.Length)] : null;
  }
}
