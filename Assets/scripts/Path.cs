using UnityEngine;
using System.Collections;

public class Path : Area {

  public override void Inside() {
    seen = true;
    if (inside.Length > 0) {
      TextConsole.PushPathText(gameObject, Choose(inside));
    }
  }

  public override void Describe() {
    seen = true;
    if (describe.Length > 0) {
      TextConsole.PushPathIndicator(gameObject, Choose(describe));
    }
  }

  void OnTriggerEnter2D() {
    occupied = true;
    seen = true;
    if (enter.Length > 0) {
      TextConsole.PushPathText(gameObject, Choose(enter));
    }
  }

  void OnTriggerExit2D() {
    occupied = false;
    seen = true;
    if (exit.Length > 0) {
      TextConsole.PushPathText(gameObject, Choose(exit));
    }
  }
}
