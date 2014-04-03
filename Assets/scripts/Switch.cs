using UnityEngine;
using System.Collections;

public class Switch : Area {

  public Obstacle door;
  public Area doorway;
  public float toggleDelay = 1.0f;

  private bool state = false;
  private float lastToggleTime = float.NegativeInfinity;

  void OnTriggerEnter2D() {
    occupied = true;
    seen = true;
    if (Time.fixedTime - lastToggleTime > toggleDelay) {
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
  }

  void OnTriggerExit2D() {
    occupied = false;
    seen = true;
    if (exit.Length > 0) {
      TextConsole.PushText(Choose(exit));
    }
  }
}
