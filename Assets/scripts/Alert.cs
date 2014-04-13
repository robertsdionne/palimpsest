using UnityEngine;
using System.Collections;

public class Ambience : MonoBehaviour {

  public string[] alerts;
  public float period;

  void OnTriggerStay2D(Collider2D other) {
    if (Random.value < 1.0f / 60.0f / period) {
      TextConsole.PushText(Choose(alerts));
    }
  }

  protected string Choose(string[] choices) {
    return choices.Length > 0 ? choices[Random.Range(0, choices.Length)] : null;
  }
}
