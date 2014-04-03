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
      lastToggleTime = Time.fixedTime;
      state = !state;
      if (null != doorway) {
        doorway.gameObject.SetActive(state);
      }
      if (null != door) {
        door.gameObject.SetActive(!state);
      }
      if (state && enter.Length > 0) {
        TextConsole.PushText(Choose(enter));
      }
      if (!state && exit.Length > 0) {
        TextConsole.PushText(Choose(exit));
      }
    }
  }

  void OnTriggerExit2D() {
    occupied = false;
    seen = true;
  }
}
