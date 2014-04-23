using UnityEngine;
using System.Collections;

public class Alert : MonoBehaviour {

  public string[] alerts;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public bool screenShake = false;
  public float time;

  private float startTime;

  void Start() {
    startTime = Time.time;
  }

  void Update() {
    if (Time.time - startTime > time) {
      if (screenShake) {
        ScreenShake.Shake();
      }
      foreach (var alert in alerts) {
        TextConsole.PushText(alert);
      }
      foreach (var next in nexts) {
        if (null != next) {
          next.SetActive(true);
        }
      }
      foreach (var disable in disables) {
        if (null != disable) {
          disable.SetActive(true);
        }
      }
      Destroy(gameObject);
    }
  }
}
