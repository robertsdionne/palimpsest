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

  public void Describe() {
    if (describe.Length > 0) {
      Debug.Log("←" + describe[Random.Range(0, describe.Length)]);
    }
  }

  public void Inside() {
    if (inside.Length > 0) {
      Debug.Log(inside[Random.Range(0, inside.Length)]);
    }
  }

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
