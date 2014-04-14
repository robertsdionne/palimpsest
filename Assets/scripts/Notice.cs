using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

  public string[] notices;
  public GameObject[] nexts;

  void OnTriggerEnter2D(Collider2D other) {
    foreach (var notice in notices) {
      TextConsole.PushText(notice);
    }
    foreach (var next in nexts) {
      next.SetActive(true);
    }
    Destroy(gameObject);
  }
}
