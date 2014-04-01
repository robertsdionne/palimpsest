using UnityEngine;
using System.Collections;

public class Obstacle : Entity {

  public string[] describe = {
    "See the area."
  };

  public string[] touch = {
    "Touch the object."
  };

  public float touchDelay;

  private float lastTouchTime = 0.0f;

  public void Describe() {
    if (describe.Length > 0) {
      TextConsole.PushIndicator(gameObject, Choose(describe));
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (touch.Length > 0 && Time.fixedTime - lastTouchTime > touchDelay) {
      TextConsole.PushIndicator(gameObject, Choose(touch));
      lastTouchTime = Time.fixedTime;
    }
  }
}
