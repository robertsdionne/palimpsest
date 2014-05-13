using UnityEngine;
using System.Collections;

public class Counter : MonoBehaviour {

  public string[] alerts;
  public string status;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public int triggerCount = 1;
  public bool destroy = false;

  private int count = 0;

  void OnEnable() {
    count += 1;
    if (null != status) {
      TextConsole.PushText(status.Contains("{0}") ?
          string.Format(status, count) : status, transform.position, Vector2.up);
    }
    if (count >= triggerCount) {
      TextConsole.PushText(string.Join("\n", alerts), transform.position, Vector2.up);
      foreach (var disable in disables) {
        if (null != disable) {
          disable.SetActive(true);
        }
      }
      foreach (var next in nexts) {
        if (null != next) {
          next.SetActive(true);
        }
      }
      count = 0;
      if (destroy) {
        Destroy(gameObject);
      }
    }
    gameObject.SetActive(false);
  }
}
