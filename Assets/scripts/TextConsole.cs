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
  public Line playerArrowLine;

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
      var line = description.GetComponent<Line>();
      description.transform.parent = textConsole.gameObject.transform;
      description.transform.localPosition = textConsole.NextPosition(line);
      description.GetComponent<TextMesh>().text = text;
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushPathText(GameObject target, string text) {
    WebsocketServer.BroadcastText(text);
    if (null != textConsole) {
      textConsole.MaybeClearLines();
      var description = Object.Instantiate(textConsole.descriptionPrefab) as GameObject;
      var line = description.GetComponent<Line>();
      description.transform.parent = textConsole.gameObject.transform;
      description.transform.localPosition = textConsole.NextPosition(line);
      description.GetComponent<TextMesh>().text = text;
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushIndicator(GameObject target, string text) {
    WebsocketServer.BroadcastIndicator(target, text);
    if (null != textConsole) {
      textConsole.MaybeClearLines();
      var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
      var line = indicator.GetComponent<Line>();
      var indicatorComponent = indicator.GetComponent<Indicator>();
      indicator.transform.parent = textConsole.gameObject.transform;
      indicator.transform.localPosition = textConsole.NextPosition(line);
      indicatorComponent.arrow.transform.rotation = textConsole.player.transform.rotation;
      indicatorComponent.description.GetComponent<TextMesh>().text = text;
      indicatorComponent.player = textConsole.player;
      indicatorComponent.target = target;
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushPathIndicator(GameObject target, string text) {
    WebsocketServer.BroadcastIndicator(target, text);
    if (null != textConsole) {
      textConsole.MaybeClearLines();
      var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
      var line = indicator.GetComponent<Line>();
      var indicatorComponent = indicator.GetComponent<Indicator>();
      indicator.transform.parent = textConsole.gameObject.transform;
      indicator.transform.localPosition = textConsole.NextPosition(line);
      indicatorComponent.arrow.transform.rotation = textConsole.player.transform.rotation;
      indicatorComponent.description.GetComponent<TextMesh>().text = text;
      indicatorComponent.player = textConsole.player;
      indicatorComponent.target = target;
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
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
