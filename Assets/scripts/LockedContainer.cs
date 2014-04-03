using UnityEngine;
using System.Collections;

public class LockedContainer : Obstacle {

  public string[] opened = {
    "You opened the locked container."
  };

  public Item key;
  public Obstacle contents;
  public Obstacle replacement;

  void OnCollisionEnter2D(Collision2D collision) {
    seen = true;
    if (touch.Length > 0 && Time.fixedTime - lastTouchTime > touchDelay) {
      ScreenShake.Shake();
      if (gameObject.activeInHierarchy && Inventory.Contains(key)) {
        Inventory.Remove(key);
        TextConsole.PushText(Choose(opened));
        gameObject.SetActive(false);
        contents.gameObject.SetActive(true);
        replacement.gameObject.SetActive(true);
      } else {
        TextConsole.PushIndicator(gameObject, Choose(touch));
      }
      lastTouchTime = Time.fixedTime;
    }
  }
}
