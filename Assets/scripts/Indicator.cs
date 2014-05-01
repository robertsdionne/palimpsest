using UnityEngine;
using System.Collections;
using System.Linq;

public class Indicator : MonoBehaviour {

  public GameObject arrow;
  public GameObject description;
  public GameObject player;
  public Entity target;

  [HideInInspector]
  public string text;
	
	public void Render() {
    var delta = target.DirectionFrom(player.transform.position);
    var distance = target.DistanceTo(player.transform.position);
    var displayDistance = distance > 0.0f ? distance : 0.0f;
    if (distance > 0.25f) {
      description.GetComponent<TextMesh>().text = string.Format(
          "{0:F1} m {1}", displayDistance, text);
    } else {
      description.GetComponent<TextMesh>().text = string.Format("{0}", text);
    }
    var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    if (distance > 0.0f) {
      arrow.transform.rotation = rotation;
    }
    var scale = 0.125f * System.Convert.ToSingle(
        distance > 0.25f && target.indicators.Contains(gameObject));
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
	}
}
