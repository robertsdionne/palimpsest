using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumRunningSpeed = 5.81f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
	
	// Update is called once per frame
	void FixedUpdate () {
    var input = GetInput();
    UpdatePosition(input);
    UpdateRotation(input);
	}

  Vector2 GetInput() {
    return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
  }

  float IsRunning() {
    return System.Convert.ToSingle(Input.GetButton("Run"));
  }

  void UpdatePosition(Vector2 input) {
    var maximumSpeed = Mathf.Lerp(maximumWalkingSpeed, maximumRunningSpeed, IsRunning());
    rigidbody2D.AddForce(maximumForce * rigidbody2D.mass * input);
    rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maximumSpeed);
  }

  void UpdateRotation(Vector2 input) {
    rigidbody2D.angularVelocity = Mathf.Clamp(
        rigidbody2D.angularVelocity, -maximumAngularVelocity, maximumAngularVelocity);
    if (input.magnitude > 0.0f) {
      var torque = Vector3.Cross(rigidbody2D.transform.up, input.normalized).z;
      rigidbody2D.AddTorque(maximumTorque * rigidbody2D.mass * torque);
    }
  }
}
