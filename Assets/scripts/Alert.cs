using UnityEngine;
using System.Collections;

public class Alert : MonoBehaviour {

  public string[] alerts;
  public GameObject[] nexts;
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
        next.SetActive(true);
      }
      Destroy(gameObject);
    }
  }
}
