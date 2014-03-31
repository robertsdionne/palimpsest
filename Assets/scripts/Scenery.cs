﻿using UnityEngine;
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
      var text = describe[Random.Range(0, describe.Length)];
      WebsocketServer.BroadcastText(text);
      TextConsole.PushIndicator(gameObject, text);
      Debug.Log(text);
    }
  }

  void OnCollisionEnter2D(Collision2D collision) {
    if (touch.Length > 0) {
      var text = touch[Random.Range(0, touch.Length)];
      WebsocketServer.BroadcastText(text);
      TextConsole.PushIndicator(gameObject, text);
      Debug.Log(text);
    }
  }
}
