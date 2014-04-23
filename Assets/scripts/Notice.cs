using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

  public string[] notices;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public bool screenShake = false;

  void OnTriggerEnter2D(Collider2D other) {
    if (screenShake) {
      ScreenShake.Shake();
    }
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
