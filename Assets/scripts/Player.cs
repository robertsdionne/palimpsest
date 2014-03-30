using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {

  private const string HORIZONTAL = "Horizontal";
  private const string RUN = "Run";
  private const string SCENERY = "Scenery";
  private const string SEE = "See";
  private const string VERTICAL = "Vertical";

  public float cameraPositionAlpha = 0.032f;
  public GameObject mainCamera;
  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumRunningSpeed = 5.81f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
	
	void FixedUpdate () {
    var input = GetInput();
    UpdateCameraPosition();
    UpdatePosition(input);
    UpdateRotation(input);
	}

  void Update() {
    MaybeSee();
  }

  Vector2 GetInput() {
    return new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
  }

  float IsRunning() {
    return System.Convert.ToSingle(Input.GetButton(RUN));
  }

  void MaybeSee() {
    if (Input.GetButtonDown(SEE)) {
      var scenery = GameObject.FindGameObjectsWithTag(SCENERY).OrderBy(
          item => Vector2.Distance(item.transform.position, gameObject.transform.position)).ToList();
      for (var i = 0; i < 3; ++i) {
        Debug.Log(scenery[i]);
      }
      Debug.Log("");
    }
  }

  void UpdateCameraPosition() {
    Vector2 position = Vector2.Lerp(
        mainCamera.transform.position, transform.position, cameraPositionAlpha);
    mainCamera.transform.position = new Vector3(
        position.x, position.y, mainCamera.transform.position.z);
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
