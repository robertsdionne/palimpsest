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
  public Line playerArrowLine;

  private List<Line> lines = new List<Line>();

  private static TextConsole textConsole;

  void Start() {
    textConsole = this;
  }

  public static void PushText(string text) {
    if (null != textConsole && null != text) {
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

  public static void PushPathText(Path target, string text) {
    if (null != textConsole && null != text) {
      textConsole.MaybeClearLines();
      var description = Object.Instantiate(textConsole.pathDescriptionPrefab) as GameObject;
      var line = description.GetComponent<Line>();
      description.transform.parent = textConsole.gameObject.transform;
      description.transform.localPosition = textConsole.NextPosition(line);
      description.GetComponent<PathDescription>().arrow.transform.rotation =
          target.transform.rotation;
      description.GetComponent<PathDescription>().description.GetComponent<TextMesh>().text = text;
      description.GetComponent<PathDescription>().arrow.transform.localPosition =
          description.GetComponent<PathDescription>().description.transform.localPosition + 
              (2.0f * description.GetComponent<PathDescription>().description.renderer.bounds.extents.x + 0.25f) * Vector3.right;
      description.GetComponent<PathDescription>().player = textConsole.player;
      description.GetComponent<PathDescription>().target = target;
      target.describers = new GameObject[] {description};
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushIndicator(Entity target, string text) {
    if (null != textConsole && null != text) {
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
      target.describers = new GameObject[] {indicator};
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushPathIndicator(Path target, string text) {
    if (null != textConsole && null != text) {
      textConsole.MaybeClearLines();
      var description = Object.Instantiate(textConsole.pathDescriptionPrefab) as GameObject;
      description.GetComponent<PathDescription>().arrow.transform.rotation =
          target.transform.rotation;
      description.GetComponent<PathDescription>().description.GetComponent<TextMesh>().text = text;
      description.GetComponent<PathDescription>().arrow.transform.localPosition =
          description.GetComponent<PathDescription>().description.transform.localPosition + 
              (2.0f * description.GetComponent<PathDescription>().description.renderer.bounds.extents.x + 0.25f) * Vector3.right;
      description.GetComponent<PathDescription>().player = textConsole.player;
      description.GetComponent<PathDescription>().target = target;
      var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
      var line = indicator.GetComponent<Line>();
      var indicatorComponent = indicator.GetComponent<Indicator>();
      indicator.transform.parent = textConsole.gameObject.transform;
      indicator.transform.localPosition = textConsole.NextPosition(line);
      indicatorComponent.arrow.transform.rotation = textConsole.player.transform.rotation;
      description.transform.parent = indicator.transform;
      description.transform.localPosition = indicatorComponent.description.transform.localPosition;
      GameObject.Destroy(indicatorComponent.description);
      indicatorComponent.player = textConsole.player;
      indicatorComponent.target = target;
      target.describers = new GameObject[] {description, indicator};
      textConsole.lines.Add(line);
      textConsole.playerArrowLine.gameObject.transform.localPosition =
          textConsole.NextPosition(textConsole.playerArrowLine);
    }
  }

  public static void PushSignIndicator(Entity target, Entity signTarget, string text) {
    if (null != textConsole && null != text) {
      textConsole.MaybeClearLines();
      var description = Object.Instantiate(textConsole.signDescriptionPrefab) as GameObject;
      description.GetComponent<Indicator>().arrow.transform.rotation =
          textConsole.player.transform.rotation;
      description.GetComponent<Indicator>().description.GetComponent<TextMesh>().text = text;
      description.GetComponent<Indicator>().arrow.transform.localPosition =
          description.GetComponent<Indicator>().description.transform.localPosition + 
              (2.0f * description.GetComponent<Indicator>().description.renderer.bounds.extents.x + 0.25f) * Vector3.right;
      description.GetComponent<Indicator>().player = textConsole.player;
      description.GetComponent<Indicator>().target = signTarget;
      var indicator = Object.Instantiate(textConsole.indicatorPrefab) as GameObject;
      var line = indicator.GetComponent<Line>();
      var indicatorComponent = indicator.GetComponent<Indicator>();
      indicator.transform.parent = textConsole.gameObject.transform;
      indicator.transform.localPosition = textConsole.NextPosition(line);
      indicatorComponent.arrow.transform.rotation = textConsole.player.transform.rotation;
      description.transform.parent = indicator.transform;
      description.transform.localPosition = indicatorComponent.description.transform.localPosition;
      GameObject.Destroy(indicatorComponent.description);
      indicatorComponent.player = textConsole.player;
      indicatorComponent.target = target;
      target.describers = new GameObject[] {indicator};
      signTarget.describers = new GameObject[] {description};
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
