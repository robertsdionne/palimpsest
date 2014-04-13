﻿using UnityEngine;
using System.Collections;

public class Sign : Entity {

  public GameObject target;

  public override void Describe() {
    seen = true;
    if (describe.Length > 0) {
      TextConsole.PushSignIndicator(gameObject, target,
          Choose(describe) + "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    }
  }

  public override void OnTouch(string text) {
    seen = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      ScreenShake.Shake();
      TextConsole.PushSignIndicator(gameObject, target,
          null != text ? text : Choose(touch) +
              "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    }
  }
}
