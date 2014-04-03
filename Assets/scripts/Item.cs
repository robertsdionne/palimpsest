using UnityEngine;
using System.Collections;

public class Item : Obstacle {

  public string[] describeInventory = {
    "An item."
  };

  public void DescribeInventory() {
    if (describe.Length > 0) {
      TextConsole.PushText(Choose(describeInventory));
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    seen = true;
    if (touch.Length > 0 && Time.fixedTime - lastTouchTime > touchDelay) {
      ScreenShake.Shake();
      TextConsole.PushText(Choose(touch));
      lastTouchTime = Time.fixedTime;
      Inventory.Add(this);
    }
  }
}
