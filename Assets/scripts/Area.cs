using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour {

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
      var text = "←" + describe[Random.Range(0, describe.Length)];
      WebsocketServer.BroadcastText(text);
      Debug.Log(text);
    }
  }

  public void Inside() {
    if (inside.Length > 0) {
      var text = inside[Random.Range(0, inside.Length)];
      WebsocketServer.BroadcastText(text);
      Debug.Log(text);
    }
  }

  public bool IsOccupied() {
    return occupied;
  }

  void OnTriggerEnter2D() {
    occupied = true;
    if (enter.Length > 0) {
      var text = enter[Random.Range(0, enter.Length)];
      WebsocketServer.BroadcastText(text);
      Debug.Log(text);
    }
  }

  void OnTriggerExit2D() {
    occupied = false;
    if (exit.Length > 0) {
      var text = exit[Random.Range(0, exit.Length)];
      WebsocketServer.BroadcastText(text);
      Debug.Log(text);
    }
  }
}
