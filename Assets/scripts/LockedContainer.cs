using UnityEngine;
using System.Collections;

public class LockedContainer : Entity {

  public string[] opened = {
    "You opened the locked container."
  };

  public Item key;
  public Entity contents;
  public Collidable closed;
  public Collidable open;

  public override void OnTouch(string text, Vector2 position, Vector2 normal) {
    seen = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      if (shake) {
        ScreenShake.Shake();
      }
      if (closed.gameObject.activeInHierarchy && Inventory.Contains(key)) {
        Inventory.Remove(key);
        TextConsole.PushText(Utilities.Choose(opened), position, normal);
        closed.gameObject.SetActive(false);
        contents.gameObject.SetActive(true);
        open.gameObject.SetActive(true);
      } else {
        TextConsole.PushIndicator(this, Utilities.Choose(touch));
      }
      lastTouchTime = Time.fixedTime;
    }
  }
}
