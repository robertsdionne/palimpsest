using UnityEngine;
using System.Collections;

public class Switch : Area {

  public Obstacle door;
  public Area doorway;

  private bool state = false;

  void OnTriggerEnter2D() {
    occupied = true;
    seen = true;
    state = !state;
    if (null != doorway) {
      doorway.gameObject.SetActive(state);
    }
    if (null != door) {
      door.gameObject.SetActive(!state);
    }
    if (enter.Length > 0) {
      TextConsole.PushText(Choose(enter));
    }
  }

  void OnTriggerExit2D() {
    occupied = false;
    seen = true;
    if (exit.Length > 0) {
      TextConsole.PushText(Choose(exit));
    }
  }
}
