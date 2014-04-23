using UnityEngine;
using System.Collections;

public class Alert : MonoBehaviour {

  public string[] alerts;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
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
