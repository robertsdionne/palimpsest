using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

  public GameObject arrow;
  public GameObject description;
  public GameObject player;
  public GameObject target;
	
	void FixedUpdate () {
    var delta = DirectionFrom(target, player.transform.position);
    var distance = DistanceTo(target, player.transform.position);
    var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    if (distance > 0.0f) {
      arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, 0.1f);
    }
    var scale = 0.125f * System.Convert.ToSingle(distance > 0.0f);
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
	}

  Vector2 DirectionFrom(GameObject target, Vector2 position) {
    var dx = new Vector2(1e-5f, 0.0f);
    var dy = new Vector2(0.0f, 1e-5f);
    var direction = -new Vector2(
        DistanceTo(target, position + dx) - DistanceTo(target, position - dx),
        DistanceTo(target, position + dy) - DistanceTo(target, position - dy));
    return direction.magnitude > 0.0f ? direction.normalized : new Vector2();
  }

  float DistanceTo(GameObject target, Vector2 position) {
    Vector2 center = target.transform.position;
    if (target.GetComponent<BoxCollider2D>().enabled) {
      Vector2 halfExtent = target.transform.localScale / 2.0f;
      return (Vector2.Max(Abs(center - position) - halfExtent, new Vector2())).magnitude;
    } else {
      float radius = target.transform.localScale.x;
      return (center - position).magnitude - radius;
    }
  }

  Vector2 Abs(Vector2 input) {
    return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
  }
}
