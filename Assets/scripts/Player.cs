using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {

  private const string ENTITY = "Entity";
  private const string HORIZONTAL = "Horizontal";
  // private const string RUN = "Run";
  private const string SEE = "See";
  private const string VERTICAL = "Vertical";

  public GameObject arrow;
  public float cameraPositionAlpha = 0.032f;
  public GameObject instructions;
  public GameObject mainCamera;
  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumRunningSpeed = 5.81f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
	
	void FixedUpdate () {
    var input = GetInput();
    UpdateArrowRotationAndScale();
    UpdateCameraPosition();
    UpdatePosition(input);
    UpdateRotation(input);
	}

  void Update() {
    MaybeSee();
  }

  void Describe(GameObject entity) {
    entity.GetComponent<Entity>().Describe();
  }

  Vector2 GetInput() {
    return new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
  }

  float IsRunning() {
    // return System.Convert.ToSingle(Input.GetButton(RUN));
    return 0.0f;
  }

  void MaybeSee() {
    if (Input.GetButtonDown(SEE)) {
      var entities = GameObject.FindGameObjectsWithTag(ENTITY);
      var areas = (GameObject.FindObjectsOfType(typeof(Area)) as Area[]).Where(
          area => area.IsOccupied()).Select(area => area.gameObject).ToList();
      var nearestEntities = entities.Where(entity => !areas.Contains(entity)).OrderBy(entity =>
          DistanceFields.DistanceTo(entity, gameObject.transform.position)).ToList();
      TextConsole.PushText("");
      for (var i = 0; i < areas.Count; ++i) {
        areas[i].GetComponent<Area>().Inside();
      }
      for (var i = 0; i < entities.Length && i < 3; ++i) {
        Describe(nearestEntities[i]);
      }
      TextConsole.PushText("");
    }
  }

  void UpdateArrowRotationAndScale() {
    var scale = 0.125f * Mathf.Clamp01(rigidbody2D.velocity.magnitude);
    var frequency = 4.0f + 4.0f * IsRunning();
    arrow.transform.localPosition = 0.01f * (
        Mathf.Sin(frequency * Time.fixedTime) * Vector2.right +
        Mathf.Sin(2.0f * frequency * Time.fixedTime) * Vector2.up);
    arrow.transform.rotation = gameObject.transform.rotation;
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
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
      var torque = Vector3.Cross(rigidbody2D.transform.right, input.normalized).z;
      rigidbody2D.AddTorque(maximumTorque * rigidbody2D.mass * torque);
    }
  }
}
