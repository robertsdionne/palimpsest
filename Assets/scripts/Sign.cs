﻿using UnityEngine;
using System.Collections;

public class Sign : Entity {

  public Entity target;

  public override void Describe() {
    seen = true;
    TextConsole.PushSignIndicator(this, target,
        Choose(describe) + "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
  }

  public override void OnTouch(string text) {
    seen = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      if (shake) {
        ScreenShake.Shake();
      }
      TextConsole.PushSignIndicator(this, target,
          null != text ? text : Choose(touch) +
              "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    }
  }
}
