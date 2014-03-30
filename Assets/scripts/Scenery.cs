using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {

  private string[] touch = {
    "Touch the object."
  };

  void OnCollisionEnter2D() {
    Debug.Log(touch[Random.Range(0, touch.Length)]);
  }
}
