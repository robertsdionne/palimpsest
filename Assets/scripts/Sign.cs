using UnityEngine;
using System.Collections;

public class Sign : Obstacle {

  public GameObject target;

  public override void Describe() {
    seen = true;
    if (describe.Length > 0) {
      TextConsole.PushSignIndicator(gameObject, target,
          Choose(describe) + "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    seen = true;
    if (touch.Length > 0 && Time.fixedTime - lastTouchTime > touchDelay) {
      ScreenShake.Shake();
      TextConsole.PushSignIndicator(gameObject, target,
          Choose(touch) + "   \"" + Choose(target.GetComponent<Entity>().describe) + "\"");
      lastTouchTime = Time.fixedTime;
    }
  }
}
