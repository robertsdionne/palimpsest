using UnityEngine;
using System.Collections;

public class Ambience : MonoBehaviour {

  public string[] alerts;
  public float period;

  void OnTriggerStay2D(Collider2D other) {
    if (!Utilities.IsPlayer(other.gameObject)) {
      return;
    }
    if (Random.value < 1.0f / 60.0f / period) {
      TextConsole.PushText(Utilities.Choose(alerts));
    }
  }
}
