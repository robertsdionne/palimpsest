using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TextConsole : MonoBehaviour {

  public GameObject descriptionPrefab;
  public GameObject indicatorPrefab;
  public GameObject pathDescriptionPrefab;
  public GameObject signDescriptionPrefab;
  public int maximumLines;
  public GameObject player;
  public Line viewConsoleLine;
  public Line playerArrowLine;

  private static TextConsole textConsole;

  void Start() {
    textConsole = this;
  }

  public static void PushText(
      string text, Vector2 position, Vector2 normal, bool important = false) {
    if (null != textConsole && null != text) {
      textConsole.audio.pitch = Random.Range(0.5f, 2.0f);
      textConsole.audio.Play();
    }

    var description = Object.Instantiate(textConsole.descriptionPrefab) as GameObject;
    var line = description.GetComponent<Line>();
    line.important = important;
    line.SetText(text);
    description.transform.position = new Vector3(position.x + 0.2f, position.y, -2.0f);
  }
}
