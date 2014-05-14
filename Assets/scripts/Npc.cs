using UnityEngine;
using System.Collections;

public class Npc : MonoBehaviour {

  public float maximumForce = 50.0f;
  public float maximumWalkingSpeed = 0.3f;
  public float tolerance = 1.0f;
  public Waypoint waypoint;

	void Update() {
    if (null != waypoint) {
      Vector2 position = transform.position;
      Vector2 targetPosition = waypoint.transform.position;
      var delta = targetPosition - position;
      UpdatePosition(delta.normalized);
      if (delta.magnitude < tolerance) {
        waypoint = waypoint.next;
      }
    }
	}

  private void UpdatePosition(Vector2 input) {
    rigidbody2D.AddForce(maximumForce * rigidbody2D.mass * input);
    rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maximumWalkingSpeed);
  }
}
