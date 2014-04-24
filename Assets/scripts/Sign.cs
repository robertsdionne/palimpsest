using UnityEngine;
using System.Collections;

public class Sign : Entity {

  public Entity target;

  public override void See() {
    seen = true;
    TextConsole.PushSignIndicator(this, target,
        Utilities.Choose(see) + "   \"" +
        Utilities.Choose(target.GetComponent<Entity>().see) + "\"");
  }

  public override void OnTouch(string text) {
    seen = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      if (shake) {
        ScreenShake.Shake();
      }
      TextConsole.PushSignIndicator(this, target,
          null != text ? text : Utilities.Choose(touch) +
              "   \"" + Utilities.Choose(target.GetComponent<Entity>().see) + "\"");
    }
  }
}
