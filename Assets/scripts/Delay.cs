using UnityEngine;
using System.Collections;

public class Delay : MonoBehaviour {

  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public float time;

  private float startTime;

  void Start() {
    OnEnable();
  }

  void OnEnable() {
    startTime = Time.time;
  }

  void Update() {
    if (Time.time - startTime > time) {
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
      this.enabled = false;
    }
  }
}
