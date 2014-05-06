using UnityEngine;
using System.Collections;

public class Switch : Entity {

  public GameObject[] disables;
  public GameObject[] enables;
  public float toggleDelay = 1.0f;

  private bool state = false;
  private float lastToggleTime = float.NegativeInfinity;

  public override void OnEnter(string text) {
    seen = true;
    if (Time.fixedTime - lastToggleTime > toggleDelay) {
      lastToggleTime = Time.fixedTime;
      state = !state;
      foreach (var disable in disables) {
        if (null != disable) {
          disable.SetActive(!state);
        }
      }
      foreach (var enable in enables) {
        if (null != enable) {
          enable.SetActive(state);
        }
      }
      if (state) {
        TextConsole.PushText(Utilities.Choose(enter));
      }
      if (!state) {
        TextConsole.PushText(Utilities.Choose(exit));
      }
    }
  }

  public override void OnExit(string text) {
    seen = true;
  }
}
