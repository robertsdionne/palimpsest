using UnityEngine;
using System.Collections;

public class Sign : Entity {

  public Entity target;

  public override void Describe() {
    seen = true;
    if (visited) {
      TextConsole.PushSignIndicator(this, target,
          Choose(describe) + "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    } else {
      TextConsole.PushIndicator(this, "Something.");
    }
    
  }

  public override void OnTouch(string text) {
    seen = true;
    visited = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      ScreenShake.Shake();
      TextConsole.PushSignIndicator(this, target,
          null != text ? text : Choose(touch) +
              "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    }
  }
}
