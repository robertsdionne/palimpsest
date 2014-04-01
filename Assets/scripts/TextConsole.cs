using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TextConsole : MonoBehaviour {

  public Line title;
  public Line controls;
  public GameObject descriptionPrefab;
  public GameObject indicatorPrefab;
  public int maximumLines;
  public GameObject player;
  public GameObject playerArrow;

  private List<Line> lines = new List<Line>();

  private static TextConsole textConsole;

  void Start() {
    textConsole = this;
    lines.Add(title);
    lines.Add(controls);
  }

  public static void PushText(string text) {
    WebsocketServer.BroadcastText(text);
    if (null != textConsole) {
      textConsole.MaybeClearLines();
      var description = Object.Instantiate(textConsole.descriptionPrefab) as GameObject;
      description.transform.parent = textConsole.gameObject.transform;
      description.transform.localPosition = textConsole.NextPosition(
          description.GetComponent<Line>());
      description.GetComponent<TextMesh>().text = text;
      textConsole.lines.Add(description.GetComponent<Line>());
      textConsole.playerArrow.transform.localPosition = textConsole.NextPosition(
          textConsole.playerArrow.GetComponent<Line>());
    }
  }

  public static void PushIndicator(GameObject target, string text) {
    WebsocketServer.BroadcastIndicator(target, text);
    if (null != textConsole) {
      textConsole.MaybeClearLines();
      var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
      indicator.transform.parent = textConsole.gameObject.transform;
      indicator.transform.localPosition = textConsole.NextPosition(
          indicator.GetComponent<Line>());
      indicator.GetComponent<Indicator>().arrow.transform.rotation =
          textConsole.player.transform.rotation;
      indicator.GetComponent<Indicator>().description.GetComponent<TextMesh>().text = text;
      indicator.GetComponent<Indicator>().player = textConsole.player;
      indicator.GetComponent<Indicator>().target = target;
      textConsole.lines.Add(indicator.GetComponent<Line>());
      textConsole.playerArrow.transform.localPosition = textConsole.NextPosition(
          textConsole.playerArrow.GetComponent<Line>());
    }
  }

  void MaybeClearLines() {
    if (lines.Count > maximumLines) {
      while (lines.Count > maximumLines) {
        Object.Destroy(lines[0].gameObject);
        lines.RemoveAt(0);
      }
      RespaceLines();
    }
  }

  void RespaceLines() {
    var oldLines = lines;
    lines = new List<Line>();
    foreach (var line in oldLines) {
      line.transform.localPosition = NextPosition(line);
      lines.Add(line);
    }
  }

  Vector2 NextPosition(Line line) {
    Vector2 previousPosition = new Vector2();
    if (lines.Count > 0) {
      var previousLine = lines.Last();
      previousPosition = previousLine.Bottom();
    }
    return previousPosition + new Vector2(0.0f, -line.height / 2.0f);
  }
}
