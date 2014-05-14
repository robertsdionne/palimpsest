using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TextConsole : MonoBehaviour {

  public const float PITCH_SHIFT_MINIMUM = 0.8f;
  public const float PITCH_SHIFT_MAXIMUM = 1.2f;
  public const float TEXT_DEFAULT_Z = -2.0f;
  public const float TEXT_SHIFT_X = 0.2f;

  public GameObject descriptionPrefab;
  public GameObject indicatorPrefab;
  public GameObject pathDescriptionPrefab;
  public GameObject signDescriptionPrefab;
  public int maximumLines;
  public GameObject player;
  public Line viewConsoleLine;
  public Line playerArrowLine;

  private static TextConsole textConsole;

  protected Line lastDialogueLine;

  void Start() {
    textConsole = this;
  }

  public static void PushText(
      string text, Vector2 position, Vector2 normal, bool important = false, bool dialogue = false, bool long_duration = false) {
    if (null != textConsole && null != text) {
      textConsole.audio.pitch = Random.Range(PITCH_SHIFT_MINIMUM, PITCH_SHIFT_MAXIMUM);
      textConsole.audio.Play();
      var description = Object.Instantiate(textConsole.descriptionPrefab) as GameObject;
      var line = description.GetComponent<Line>();
      line.important = important;
      line.long_duration = long_duration;
      line.SetText(text);
      line.transform.position = new Vector3(
          position.x + TEXT_SHIFT_X, position.y, TEXT_DEFAULT_Z);
      if (dialogue) {
        if (null != textConsole.lastDialogueLine) {
          textConsole.lastDialogueLine.transform.GetChild(0).gameObject.SetActive(true);
          textConsole.lastDialogueLine.transform.GetChild(1).gameObject.SetActive(false);
        }
        line.transform.GetChild(0).gameObject.SetActive(false);
        line.transform.GetChild(1).gameObject.SetActive(true);
        textConsole.lastDialogueLine = line;
      }
    }
  }
}
