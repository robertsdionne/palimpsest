using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

  public GameObject arrow;
  public GameObject description;
  public GameObject player;
  public GameObject target;
	
	void FixedUpdate () {
    var delta = DistanceFields.DirectionFrom(target, player.transform.position);
    var distance = DistanceFields.DistanceTo(target, player.transform.position);
    var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    if (distance > 0.0f) {
      arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, 0.1f);
    }
    var scale = 0.125f * System.Convert.ToSingle(distance > 0.0f);
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
	}
}
