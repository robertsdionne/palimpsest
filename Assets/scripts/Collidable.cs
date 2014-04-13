using UnityEngine;
using System.Collections;

public class Collidable : MonoBehaviour {

  public string[] enter;
  public string[] exit;
  public string[] touch;

  protected bool occupied = false;

  public bool IsOccupied() {
    return occupied;
  }

  void OnCollisionEnter2D(Collision2D collision) {
    transform.root.GetComponent<Entity>().OnTouch(Choose(touch));
  }

  void OnTriggerEnter2D() {
    var choice = Choose(enter);
    if (null != choice) {
      TextConsole.PushText(choice);
    } else {
      transform.root.GetComponent<Entity>().OnEnter(choice);
    }
    occupied = true;
  }

  void OnTriggerExit2D() {
    occupied = false;
    var choice = Choose(exit);
    if (null != choice) {
      TextConsole.PushText(choice);
    } else {
      transform.root.GetComponent<Entity>().OnExit(choice);
    }
  }

  protected string Choose(string[] choices) {
    return choices.Length > 0 ? choices[Random.Range(0, choices.Length)] : null;
  }
}
