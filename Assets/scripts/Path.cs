using UnityEngine;
using System.Collections;

public class Path : Entity {

  public Color color = Color.black;
  public GameObject pathway;

  public float width = 1.0f;

  public override void See() {
    seen = true;
    TextConsole.PushPathIndicator(this, Choose(see));
  }

  public override void Inside() {
    seen = true;
    TextConsole.PushPathText(this, Choose(inside));
  }

  public Vector2 PerpendicularTo(Vector2 playerPosition) {
    var dx = 1e-5f * Vector2.right;
    var dy = 1e-5f * Vector2.up;
    var direction = -new Vector2(
        DistanceToPath(playerPosition + dx) - DistanceToPath(playerPosition - dx),
        DistanceToPath(playerPosition + dy) - DistanceToPath(playerPosition - dy));
    var dir = direction.magnitude > 0.0f ? direction.normalized : new Vector2();
    var perpendicular = new Vector2(-dir.y, dir.x);
    return perpendicular;
  }

  public override float DistanceTo(Vector2 playerPosition) {
    return DistanceToPath(playerPosition);
  }

  public float DistanceToPath(Vector2 playerPosition) {
    var minimum = float.PositiveInfinity;
    Transform previous = null;
    foreach (Transform child in transform) {
      if (null != previous) {
        var distance = DistanceFields.DistanceToLine(
            previous.gameObject, child.gameObject, playerPosition);
        if (distance < minimum) {
          minimum = distance;
        }
      }
      previous = child;
    }
    return minimum;
  }

  void Start() {
    Transform previous = null;
    foreach (Transform child in transform) {
      if (null != previous) {
        var segment = GameObject.Instantiate(pathway) as GameObject;
        segment.transform.parent = child;
        var delta = child.position - previous.position;
        var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        segment.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        segment.transform.localScale = new Vector2(delta.magnitude, width) / child.localScale.x;
        segment.transform.localPosition = -delta / 2.0f / child.localScale.x;
      }
      previous = child;
    }
  }

  public override void OnEnter(string text) {
    if (!IsOccupied()) {
      seen = true;
      TextConsole.PushPathText(this, Choose(enter));
    }
  }

  public override void OnExit(string text) {
    if (!IsOccupied()) {
      seen = true;
      TextConsole.PushPathText(this, Choose(exit));
    }
  }

  void OnDrawGizmos() {
    var halfWidth = width / 2.0f;
    Transform previous = null;
    Gizmos.color = color;
    foreach (Transform child in transform) {
      child.localScale = new Vector2(halfWidth, halfWidth);
      if (null != previous) {
        var delta = child.position - previous.position;
        var direction = delta.normalized;
        var perpendicular = halfWidth * new Vector3(-direction.y, direction.x, 0.0f);
        Gizmos.DrawLine(previous.position - perpendicular, child.position - perpendicular);
        Gizmos.DrawLine(previous.position + perpendicular, child.position + perpendicular);
      }
      previous = child;
    }
  }
}
