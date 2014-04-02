using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : Entity {

  public string[] enter = {
    "Enter the area."
  };

  public string[] exit = {
    "Exit the area."
  };

  public string[] inside = {
    "Inside the area."
  };

  protected bool occupied = false;

  public virtual void Inside() {
    seen = true;
    if (inside.Length > 0) {
      TextConsole.PushText(Choose(inside));
    }
  }

  public bool IsOccupied() {
    return occupied;
  }

  void OnTriggerEnter2D() {
    occupied = true;
    seen = true;
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
