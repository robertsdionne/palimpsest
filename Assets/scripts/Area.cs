using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

  private string[] describe = {
    "See the area."
  };

  private string[] enter = {
    "Enter the area."
  };

  private string[] exit = {
    "Exit the area."
  };

  private string[] inside = {
    "Inside the area."
  };

  void OnTriggerEnter2D() {
    Debug.Log(enter[Random.Range(0, enter.Length)]);
  }

  void OnTriggerExit2D() {
    Debug.Log(exit[Random.Range(0, exit.Length)]);
  }
}
