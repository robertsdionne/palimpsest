using UnityEngine;
using System.Collections;

public class DistanceFields {

  public static Vector2 DirectionFrom(GameObject target, Vector2 position) {
    var dx = 1e-5f * Vector2.right;
    var dy = 1e-5f * Vector2.up;
    var direction = -new Vector2(
        DistanceTo(target, position + dx) - DistanceTo(target, position - dx),
        DistanceTo(target, position + dy) - DistanceTo(target, position - dy));
    return direction.magnitude > 0.0f ? direction.normalized : new Vector2();
  }

  public static float DistanceTo(GameObject target, Vector2 position) {
    Vector2 center = target.transform.position;
    var p = center - position;
    if (target.GetComponent<BoxCollider2D>().enabled) {
      var q = Quaternion.Inverse(target.transform.rotation) * p;
      Vector2 halfExtent = target.transform.localScale / 2.0f;
      return (Vector2.Max(Abs(q) - halfExtent, new Vector2())).magnitude;
    } else {
      float radius = target.transform.localScale.x;
      return p.magnitude - radius; // p rather than q for rotational symmetry
    }
  }

  private static Vector2 Abs(Vector2 input) {
    return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
  }
}
