using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextConsole : MonoBehaviour {

  public GameObject descriptionPrefab;
  public GameObject indicatorPrefab;
  public int maximumLines;
  public GameObject player;
  public GameObject playerArrow;

  private List<GameObject> lines = new List<GameObject>();

  private static TextConsole textConsole;

  void Start() {
    textConsole = this;
  }

  public static void PushText(string text) {
    textConsole.MaybeClearItems();
    var description = Object.Instantiate(textConsole.descriptionPrefab) as GameObject;
    description.transform.parent = textConsole.gameObject.transform;
    description.transform.localPosition = textConsole.NextPosition();
    description.GetComponent<TextMesh>().text = text;
    textConsole.lines.Add(description);
    textConsole.playerArrow.transform.localPosition = textConsole.NextPosition();
  }

  public static void PushIndicator(GameObject target, string text) {
    textConsole.MaybeClearItems();
    var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
    indicator.transform.parent = textConsole.gameObject.transform;
    indicator.transform.localPosition = textConsole.NextPosition();
    indicator.GetComponent<Indicator>().arrow.transform.rotation = textConsole.player.transform.rotation;
    indicator.GetComponent<Indicator>().description.GetComponent<TextMesh>().text = text;
    indicator.GetComponent<Indicator>().player = textConsole.player;
    indicator.GetComponent<Indicator>().target = target;
    textConsole.lines.Add(indicator);
    textConsole.playerArrow.transform.localPosition = textConsole.NextPosition();
  }

  void MaybeClearItems() {
    if (lines.Count > maximumLines) {
      while (lines.Count > maximumLines) {
        Object.Destroy(lines[0]);
        lines.RemoveAt(0);
      }
      var oldLines = lines;
      lines = new List<GameObject>();
      foreach (var line in oldLines) {
        line.transform.localPosition = NextPosition();
        lines.Add(line);
      }
    }
  }

  Vector2 NextPosition() {
    return new Vector2(0, -lines.Count / 3.0f);
  }
}
