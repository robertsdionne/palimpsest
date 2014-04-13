using UnityEngine;
using System.Collections;

public static class DistanceFields {

  public static float DistanceToLine(GameObject first, GameObject second, Vector2 position) {
    Vector2 a = first.transform.position;
    Vector2 b = second.transform.position;
    var pa = position - a;
    var ba = b - a;
    var h = Mathf.Clamp01(Vector2.Dot(pa, ba) / Vector2.Dot(ba, ba));
    return (pa - h * ba).magnitude;
  }

  public static float DistanceTo(this Collider2D target, Vector2 position) {
    if (target is BoxCollider2D) {
      return ((BoxCollider2D) target).DistanceTo(position);
    } else {
      return ((CircleCollider2D) target).DistanceTo(position);
    }
  }

  public static float DistanceTo(this BoxCollider2D target, Vector2 position) {
    Vector2 center = target.transform.position;
    var p = center - position;
    var q = Quaternion.Inverse(target.transform.rotation) * p;
    Vector2 halfExtent = target.transform.lossyScale / 2.0f;
    return (Vector2.Max(Abs(q) - halfExtent, new Vector2())).magnitude;
  }

  public static float DistanceTo(this CircleCollider2D target, Vector2 position) {
    Vector2 center = target.transform.position;
    var p = center - position;
    float radius = Mathf.Max(target.transform.lossyScale.x, target.transform.lossyScale.y);
    return p.magnitude - radius;
  }

  private static Vector2 Abs(Vector2 input) {
    return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
  }
}
