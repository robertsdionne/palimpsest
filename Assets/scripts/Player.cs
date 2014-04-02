using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

  private const string ENTITY = "Entity";
  private const string HORIZONTAL = "Horizontal";
  private const string RUN = "Run";
  private const string SEE = "See";
  private const string VERTICAL = "Vertical";

  public GameObject arrow;
  public float cameraPositionAlpha = 0.032f;
  public GameObject eye;
  public GameObject mainCamera;
  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumRunningSpeed = 5.81f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
  public int numberToSee = 3;
  public float seeDelay = 4.0f;

  private List<GameObject> occupiedAreas = new List<GameObject>();
  private List<GameObject> nearestEntities = new List<GameObject>();
  private float lastSeeTime = float.NegativeInfinity;
	
	void FixedUpdate () {
    UpdateAreasAndEntities();
    var input = GetInput();
    UpdateArrowRotationAndScale(input);
    UpdateEyeScale();
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

  List<GameObject> GetOccupiedAreas() {
    return (GameObject.FindObjectsOfType(typeof(Area)) as Area[]).Where(
          area => area.IsOccupied()).Select(area => area.gameObject).ToList();
  }

  List<GameObject> GetNearestEntities(List<GameObject> occupiedAreas) {
    var entities = GameObject.FindGameObjectsWithTag(ENTITY);
    return entities.Where(entity => !occupiedAreas.Contains(entity)).OrderBy(entity =>
        DistanceFields.DistanceTo(
            entity, gameObject.transform.position)).Take(numberToSee).ToList();
  }

  bool IsMoreToSee() {
    return nearestEntities.Any(entity => !entity.GetComponent<Entity>().IsSeen());
  }

  float IsRunning() {
    return System.Convert.ToSingle(Input.GetButton(RUN));
  }

  void MaybeSee() {
    if (Input.GetButtonDown(SEE) && (Time.fixedTime - lastSeeTime > seeDelay || IsMoreToSee())) {
      lastSeeTime = Time.fixedTime;
      TextConsole.PushText("");
      for (var i = 0; i < occupiedAreas.Count; ++i) {
        occupiedAreas[i].GetComponent<Area>().Inside();
      }
      for (var i = 0; i < nearestEntities.Count; ++i) {
        nearestEntities[i].GetComponent<Entity>().Describe();
      }
      TextConsole.PushText("");
    }
  }

  void UpdateAreasAndEntities() {
    var newlyOccupiedAreas = GetOccupiedAreas();
    var newlyNearestEntities = GetNearestEntities(occupiedAreas);
    foreach (var entity in newlyNearestEntities) {
      if (!occupiedAreas.Contains(entity) && !nearestEntities.Contains(entity)) {
        lastSeeTime = float.NegativeInfinity;
      }
    }
    occupiedAreas = newlyOccupiedAreas;
    nearestEntities = newlyNearestEntities;
  }

  void UpdateArrowRotationAndScale(Vector2 input) {
    var moving = Mathf.Clamp01(rigidbody2D.velocity.magnitude);
    var scale = 0.125f * Mathf.Clamp01(5.0f * input.magnitude);
    var frequency = 4.0f + 4.0f * IsRunning();
    arrow.transform.localPosition = (0.01f + 0.01f * IsRunning()) * moving * (
        Mathf.Sin(frequency * Time.fixedTime) * Vector2.right +
        Mathf.Sin(2.0f * frequency * Time.fixedTime) * Vector2.up);
    arrow.transform.rotation = gameObject.transform.rotation;
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
  }

  void UpdateEyeScale() {
    var scale = 0.125f * System.Convert.ToSingle(IsMoreToSee());
    eye.transform.localScale = Vector2.Lerp(
        eye.transform.localScale, new Vector2(0.125f, scale), 0.1f);
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
