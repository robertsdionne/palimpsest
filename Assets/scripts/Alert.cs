using UnityEngine;
using System.Collections;

public class Alert : MonoBehaviour {

  public string[] alerts;
  public GameObject at;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public bool screenShake = false;
  public float time;
  public bool long_duration = true;
  public bool important = true;
  public bool dialogue = false;

  private float startTime;

  void Start() {
    OnEnable();
  }

  void OnEnable() {
    startTime = Time.time;
  }

  void Update() {
    if (Time.time - startTime > time) {
      if (screenShake) {
        ScreenShake.Shake();
      }
      if (alerts.Length > 0) {
        TextConsole.PushText(string.Join("\n", alerts),
            null == at ? gameObject.transform.position : at.transform.position, Vector2.up, important, dialogue, long_duration);
      }
      foreach (var disable in disables) {
        if (null != disable) {
          disable.SetActive(false);
        }
      }
      foreach (var next in nexts) {
        if (null != next) {
          next.SetActive(true);
        }
      }
      gameObject.SetActive(false);
    }
  }
}
