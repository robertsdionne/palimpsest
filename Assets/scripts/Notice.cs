using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

  public string[] notices;

  void OnTriggerEnter2D(Collider2D other) {
    foreach (var notice in notices) {
      TextConsole.PushText(notice);
    }
    Destroy(gameObject);
  }
}
