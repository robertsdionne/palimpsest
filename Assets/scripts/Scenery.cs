using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {

  public string[] describe = {
    "See the area."
  };

  public string[] touch = {
    "Touch the object."
  };

  public void Describe() {
    if (describe.Length > 0) {
      Debug.Log("←" + describe[Random.Range(0, describe.Length)]);
    }
  }

  void OnCollisionEnter2D() {
    if (touch.Length > 0) {
      Debug.Log(touch[Random.Range(0, touch.Length)]);
    }
  }
}
