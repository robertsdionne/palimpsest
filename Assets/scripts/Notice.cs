using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

  public string[] notices;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };

  void OnTriggerEnter2D(Collider2D other) {
    foreach (var notice in notices) {
      TextConsole.PushText(notice);
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
    gameObject.SetActive(false);
  }
}
