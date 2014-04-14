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

  public override void OnTouch(string text) {
    seen = true;
    visited = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      if (shake) {
        ScreenShake.Shake();
      }
      if (closed.gameObject.activeInHierarchy && Inventory.Contains(key)) {
        Inventory.Remove(key);
        TextConsole.PushText(Choose(opened));
        closed.gameObject.SetActive(false);
        contents.gameObject.SetActive(true);
        open.gameObject.SetActive(true);
      } else {
        TextConsole.PushIndicator(this, Choose(touch));
      }
      lastTouchTime = Time.fixedTime;
    }
  }
}
