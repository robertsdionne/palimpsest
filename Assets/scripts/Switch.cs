using UnityEngine;
using System.Collections;

public class Switch : Entity {

  public GameObject[] disables;
  public GameObject[] enables;
  public float toggleDelay = 1.0f;

  private bool state = false;
  private float lastToggleTime = float.NegativeInfinity;

  public override void OnEnter(string text, Vector2 position) {
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
        TextConsole.PushText(Utilities.Choose(enter), position, Vector2.up);
      }
      if (!state) {
        TextConsole.PushText(Utilities.Choose(exit), position, Vector2.up);
      }
    }
  }

  public override void OnExit(string text, Vector2 position) {
    seen = true;
  }
}
