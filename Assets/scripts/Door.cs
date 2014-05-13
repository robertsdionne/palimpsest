using UnityEngine;
using System.Collections;

public class Door : Entity {

  public Collidable door;
  public Collidable doorway;

  public void Close() {
    door.gameObject.SetActive(true);
    doorway.gameObject.SetActive(false);
  }

  public bool IsClosed() {
    return door.gameObject.activeInHierarchy;
  }

  public void Open() {
    door.gameObject.SetActive(false);
    doorway.gameObject.SetActive(true);
  }

  public override void OnExit(string text, Vector2 position) {
    if (!IsClosed()) {
      Close();
    }
    base.OnExit(text, position);
  }

  public override void OnTouch(string text, Vector2 position, Vector2 normal) {
    if (IsClosed()) {
      Open();
    }
    base.OnTouch(text, position, normal);
  }
}
