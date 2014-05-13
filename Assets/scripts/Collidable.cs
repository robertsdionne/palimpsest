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
    if (!Utilities.IsPlayer(collision.gameObject)) {
      return;
    }
    transform.root.GetComponent<Entity>().OnTouch(Utilities.Choose(touch), collision.contacts[0].point, collision.contacts[0].normal);
  }

  void OnTriggerEnter2D(Collider2D other) {
    if (!Utilities.IsPlayer(other.gameObject)) {
      return;
    }
    occupied = true;
    var choice = Utilities.Choose(enter);
    if (null != choice) {
      TextConsole.PushText(choice, other.transform.position, Vector2.up);
    } else {
      transform.root.GetComponent<Entity>().OnEnter(choice, other.transform.position);
    }
  }

  void OnTriggerExit2D(Collider2D other) {
    if (!Utilities.IsPlayer(other.gameObject)) {
      return;
    }
    occupied = false;
    var choice = Utilities.Choose(exit);
    if (null != choice) {
      TextConsole.PushText(choice, other.transform.position, Vector2.up);
    } else {
      transform.root.GetComponent<Entity>().OnExit(choice, other.transform.position);
    }
  }
}
