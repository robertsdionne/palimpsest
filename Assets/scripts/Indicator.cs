using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

  public GameObject arrow;
  public GameObject description;
  public GameObject player;
  public GameObject target;
	
	void FixedUpdate () {
    var delta = target.transform.position - player.transform.position;
    var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
    var rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    arrow.transform.rotation = Quaternion.Slerp(arrow.transform.rotation, rotation, 0.1f);
	}
}
