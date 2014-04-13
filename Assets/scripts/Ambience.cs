using UnityEngine;
using System.Collections;

public class Alert : MonoBehaviour {

  public string[] alerts;
  public float time;

  private float startTime;

  void Start() {
    startTime = Time.time;
  }

  void Update() {
    if (Time.time - startTime > time) {
      foreach (var alert in alerts) {
        TextConsole.PushText(alert);
      }
      Destroy(gameObject);
    }
  }
}
