using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

  public enum Mode {Union, Intersection};

  public Mode mode = Mode.Union;
  public string[] see = {
    "Seeing the entity."
  };
  [HideInInspector]
  public GameObject[] indicators;
  public string[] enter = {
    "Entering the area."
  };
  public string[] exit = {
    "Exiting the area."
  };
  public string[] inside = {
    "Inside the area."
  };
  public bool shake = true;
  public string[] touch = {
    "Touching the obstacle."
  };
  public float touchDelay = 1.0f;
  public bool visible = true;

  protected float lastTouchTime = 0.0f;
  protected bool seen = false;
  protected bool wasOccupied = false;

  public virtual void See() {
    seen = true;
    TextConsole.PushIndicator(this, Choose(see));
  }

  public Vector2 DirectionFrom(Vector2 playerPosition) {
    var dx = 1e-5f * Vector2.right;
    var dy = 1e-5f * Vector2.up;
    var direction = -new Vector2(
        DistanceTo(playerPosition + dx) - DistanceTo(playerPosition - dx),
        DistanceTo(playerPosition + dy) - DistanceTo(playerPosition - dy));
    return direction.magnitude > 0.0f ? direction.normalized : new Vector2();
  }

  public virtual float DistanceTo(Vector2 playerPosition) {
    var colliders = GetComponentsInChildren<Collider2D>();
    var extremum = Mode.Union == mode ? float.PositiveInfinity : float.NegativeInfinity;
    foreach (var collider in colliders) {
      var distance = collider.DistanceTo(playerPosition);
      if (Mode.Union == mode) {
        if (distance < extremum) {
          extremum = distance;
        }
      } else {
        if (distance > extremum) {
          extremum = distance;
        }
      }
    }
    return extremum;
  }

  public virtual void Inside() {
    seen = true;
    TextConsole.PushText(Choose(inside));
  }

  public virtual bool IsOccupied() {
    var occupied = Mode.Intersection == mode;
    var collidables = GetComponentsInChildren<Collidable>();
    foreach (var collidable in collidables) {
      if (Mode.Union == mode) {
        occupied |= collidable.IsOccupied();
      } else {
        occupied &= collidable.IsOccupied();
      }
    }
    return occupied;
  }

  public bool IsSeen() {
    return seen;
  }

  public virtual void OnEnter(string text) {
    if (IsOccupied() && !wasOccupied) {
      seen = true;
      wasOccupied = true;
      TextConsole.PushText(null != text ? text : Choose(enter));
    }
  }

  public virtual void OnExit(string text) {
    if (!IsOccupied() && wasOccupied) {
      seen = true;
      wasOccupied = false;
      TextConsole.PushText(null != text ? text : Choose(exit));
    }
  }

  public virtual void OnTouch(string text) {
    seen = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      if (shake) {
        ScreenShake.Shake();
      }
      TextConsole.PushText(null != text ? text : Choose(touch));
    }
  }

  protected string Choose(string[] choices) {
    return choices.Length > 0 ? choices[Random.Range(0, choices.Length)] : null;
  }
}
