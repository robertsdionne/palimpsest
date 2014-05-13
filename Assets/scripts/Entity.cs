using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

  public const float D = 1e-5f;

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
  public float shakeDelay = 1.0f;
  public float touchDelay = 5.0f;
  public bool visible = true;

  protected float lastShakeTime = 0.0f;
  protected float lastTouchTime = 0.0f;
  protected bool seen = false;

  public bool known = false;
  public bool touched = false;
  protected bool wasOccupied = false;

  public virtual void See() {
    seen = true;
  }

  public Vector2 DirectionFrom(Vector2 playerPosition) {
    var dx = D * Vector2.right;
    var dy = D * Vector2.up;
    var direction = -new Vector2(
        DistanceTo(playerPosition + dx) - DistanceTo(playerPosition - dx),
        DistanceTo(playerPosition + dy) - DistanceTo(playerPosition - dy));
    return direction.magnitude > 0.0f ? direction.normalized : new Vector2();
  }

  public virtual float DistanceTo(Vector2 playerPosition) {
    var colliders = GetComponentsInChildren<Collidable>();
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
    TextConsole.PushText(
        string.Format("({0})", Utilities.Choose(inside)), transform.position, Vector2.up);
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

  public virtual void OnEnter(string text, Vector2 position) {
    if (IsOccupied() && !wasOccupied) {
      seen = true;
      touched = true;
      wasOccupied = true;
      text = null == text ? Utilities.Choose(enter) : text;
      text = null == text ? null : string.Format("({0})", text);
      TextConsole.PushText(text, position, Vector2.up);
    }
  }

  public virtual void OnExit(string text, Vector2 position) {
    if (!IsOccupied() && wasOccupied) {
      seen = true;
      touched = true;
      wasOccupied = false;
      text = null == text ? Utilities.Choose(exit) : text;
      text = null == text ? null : string.Format("({0})", text);
      TextConsole.PushText(text, position, Vector2.up);
    }
  }

  public virtual void OnTouch(string text, Vector2 position, Vector2 normal) {
    seen = true;
    touched = true;
    if (Time.fixedTime - lastTouchTime > touchDelay) {
      lastTouchTime = Time.fixedTime;
      TextConsole.PushText(null != text ? text : Utilities.Choose(touch), position, normal);
    }
    if (shake && Time.fixedTime - lastShakeTime > shakeDelay) {
      lastShakeTime = Time.fixedTime;
      ScreenShake.Shake();
    }
  }
}
