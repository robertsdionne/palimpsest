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

  void OnCollisionEnter2D(Collision2D collision) {
    if (touch.Length > 0) {
      var arrow = "";
      var normal = collision.contacts[0].normal;
      if (Mathf.Abs(normal.x) > Mathf.Abs(normal.y)) {
        if (normal.x < 0) {
          arrow = "← ";
        } else {
          arrow = "→ ";
        }
      } else {
        if (normal.y < 0) {
          arrow = "↓ ";
        } else {
          arrow = "↑ ";
        }
      }
      Debug.Log(arrow + touch[Random.Range(0, touch.Length)]);
    }
  }
}
