using UnityEngine;
using System.Collections;

public class Switch : Entity {

  public GameObject door;
  public GameObject doorway;
  public float toggleDelay = 1.0f;

  private bool state = false;
  private float lastToggleTime = float.NegativeInfinity;

  public override void OnEnter(string text) {
    seen = true;
    if (Time.fixedTime - lastToggleTime > toggleDelay) {
      lastToggleTime = Time.fixedTime;
      state = !state;
      if (null != doorway) {
        doorway.SetActive(state);
      }
      if (null != door) {
        door.SetActive(!state);
      }
      if (state && enter.Length > 0) {
        TextConsole.PushText(Choose(enter));
      }
      if (!state && exit.Length > 0) {
        TextConsole.PushText(Choose(exit));
      }
    }
  }

  public override void OnExit(string text) {
    seen = true;
  }
}
