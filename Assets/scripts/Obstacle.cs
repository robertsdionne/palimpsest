using UnityEngine;
using System.Collections;

public class Obstacle : Entity {

  public string[] describe = {
    "See the area."
  };

  public string[] touch = {
    "Touch the object."
  };

  public void Describe() {
    if (describe.Length > 0) {
      var text = describe[Random.Range(0, describe.Length)];
      WebsocketServer.BroadcastIndicator(gameObject, text);
      TextConsole.PushIndicator(gameObject, text);
      Debug.Log(text);
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (touch.Length > 0) {
      var text = touch[Random.Range(0, touch.Length)];
      WebsocketServer.BroadcastIndicator(gameObject, text);
      TextConsole.PushIndicator(gameObject, text);
      Debug.Log(text);
    }
  }
}
