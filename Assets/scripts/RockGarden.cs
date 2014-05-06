using UnityEngine;
using System.Collections;

public class RockGarden : Path {

  public override Vector2 PerpendicularTo(Vector2 playerPosition) {
    var dx = 1e-5f * Vector2.right;
    var dy = 1e-5f * Vector2.up;
    var direction = -new Vector2(
        DistanceTo(playerPosition + dx) - DistanceTo(playerPosition - dx),
        DistanceTo(playerPosition + dy) - DistanceTo(playerPosition - dy));
    var dir = direction.magnitude > 0.0f ? direction.normalized : new Vector2();
    var perpendicular = new Vector2(-dir.y, dir.x);
    return perpendicular;
  }

  public override float DistanceTo(Vector2 playerPosition) {
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

  void OnDrawGizmos() {}

  void Start() {}
}
