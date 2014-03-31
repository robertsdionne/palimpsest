using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : Entity {

  public string[] describe = {
    "See the area."
  };

  public string[] enter = {
    "Enter the area."
  };

  public string[] exit = {
    "Exit the area."
  };

  public string[] inside = {
    "Inside the area."
  };

  private bool occupied = false;

  public void Describe() {
    if (describe.Length > 0) {
      TextConsole.PushIndicator(gameObject, Choose(describe));
    }
  }

  public void Inside() {
    if (inside.Length > 0) {
      TextConsole.PushText(Choose(inside));
    }
  }

  public bool IsOccupied() {
    return occupied;
  }

  void OnTriggerEnter2D() {
    occupied = true;
    if (enter.Length > 0) {
      TextConsole.PushText(Choose(enter));
    }
  }

  void OnTriggerExit2D() {
    occupied = false;
    if (exit.Length > 0) {
      TextConsole.PushText(Choose(exit));
    }
  }
}
