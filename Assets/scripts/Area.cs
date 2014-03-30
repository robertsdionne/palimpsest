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

  void OnTriggerEnter2D() {
    if (enter.Length > 0) {
      Debug.Log(enter[Random.Range(0, enter.Length)]);
    }
  }

  void OnTriggerExit2D() {
    if (exit.Length > 0) {
      Debug.Log(exit[Random.Range(0, exit.Length)]);
    }
  }
}
