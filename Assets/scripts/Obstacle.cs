using UnityEngine;
using System.Collections;

public class Obstacle : Entity {

  public string[] touch = {
    "Touch the object."
  };

  public float touchDelay;

  private float lastTouchTime = 0.0f;

  void OnCollisionEnter2D(Collision2D collision) {
    seen = true;
    if (touch.Length > 0 && Time.fixedTime - lastTouchTime > touchDelay) {
      TextConsole.PushIndicator(gameObject, Choose(touch));
      lastTouchTime = Time.fixedTime;
    }
  }
}
