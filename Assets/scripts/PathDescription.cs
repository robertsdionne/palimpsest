using UnityEngine;
using System.Collections;

public class PathDescription : MonoBehaviour {

  public GameObject arrow;
  public GameObject description;
  public GameObject player;
  public Path target;

  private bool orientation = true;

  void Start() {
    arrow.transform.rotation = DetermineArrowRotation();
  }
	
	void FixedUpdate () {
    var rotation = DetermineArrowRotation();
    arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, 0.1f);
    var distance = target.GetComponent<Entity>().DistanceTo(player.transform.position);
    var scale = 0.125f * System.Convert.ToSingle(distance <= target.width / 2.0f);
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
	}

  Quaternion DetermineArrowRotation() {
    var perpendicular = target.PerpendicularTo(player.transform.position);
    var dot = Vector2.Dot(player.transform.right, perpendicular);
    if (orientation && dot < -0.25f) {
      orientation = false;
    }
    if (!orientation && dot > 0.25f) {
      orientation = true;
    }
    var direction = (orientation ? 1.0f : -1.0f) * perpendicular;
    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }
}
