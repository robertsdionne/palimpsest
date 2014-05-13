using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

  private const string HORIZONTAL = "Horizontal";
  private const string RUN = "Run";
  private const string SEE = "See";
  private const string VERTICAL = "Vertical";

  public float arrowScale = 0.25f;
  public float eyeScale = 0.25f;
  public AudioSource idleSound;
  public AudioSource walkSound;
  public GameObject targetPrefab;
  public GameObject arrow;
  public GameObject blockedArrow;
  public GameObject fieldArrow;
  public Entity fieldArrowTarget;
  public GameObject nearArrow;
  public GameObject playerArrow;
  public float cameraPositionAlpha = 0.032f;
  public GameObject eye;
  public GameObject mainCamera;
  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumRunningSpeed = 5.81f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
  public int numberToSee = 3;

  private List<Entity> occupiedAreas = new List<Entity>();
  private List<Entity> nearestEntities = new List<Entity>();
  private Dictionary<Entity, GameObject> targets = new Dictionary<Entity, GameObject>();

  void OnDisable() {
    var removal = new List<Entity>();
    removal.AddRange(targets.Keys);
    foreach (var entity in removal) {
      Destroy(targets[entity]);
      targets.Remove(entity);
      entity.touched = false;
    }
    targets.Clear();
  }

	void FixedUpdate () {
    UpdateAreasAndEntities();
    var input = GetInput();
    UpdateCameraPosition();
    UpdatePosition(input);
    UpdateRotation(input);
    idleSound.volume = Mathf.Lerp(idleSound.volume, 0.1f * (1.0f - IsMoving()), 0.1f);
    walkSound.volume = Mathf.Lerp(walkSound.volume, 0.2f * IsMoving(), 0.1f);
	}

  void Update() {
    MaybeSee();
  }

  float IsMoving() {
    return Mathf.Clamp01(2.0f * rigidbody2D.velocity.magnitude);
  }

  Vector2 GetInput() {
    return new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
  }

  List<Entity> GetOccupiedAreas() {
    return (GameObject.FindObjectsOfType(typeof(Entity)) as Entity[]).Where(
          entity => entity.IsOccupied() && entity.visible).ToList();
  }

  List<Entity> GetNearestEntities(List<Entity> occupiedAreas) {
    var entities = GameObject.FindObjectsOfType(typeof(Entity)) as Entity[];
    return entities.Where(entity => !occupiedAreas.Contains(entity) && entity.visible).OrderBy(
        entity => entity.DistanceTo(gameObject.transform.position)).Take(numberToSee).ToList();
  }

  bool IsMoreToSee() {
    return false;//nearestEntities.Any(entity => !entity.IsSeen());
  }

  float IsRunning() {
    return 0.0f;
  }

  void MaybeSee() {
    var j = 0;
    foreach (var entity in nearestEntities) {
      var position = PositionOf(entity);
      var rotation = RotationOf(entity);
      if (!targets.ContainsKey(entity)) {
        targets[entity] = Instantiate(targetPrefab, position, Quaternion.identity) as GameObject;
        targets[entity].transform.GetChild(0).gameObject.SetActive(false);
        targets[entity].transform.GetChild(0).GetComponent<TextMesh>().text = entity.see[0];
        targets[entity].transform.GetChild(1).rotation = rotation;
        var color00 = targets[entity].transform.GetChild(0).renderer.material.color;
        var color10 = targets[entity].transform.GetChild(1).renderer.material.color;
        color00.a = color10.a = 0.0f;
        targets[entity].transform.GetChild(0).renderer.material.color = color00;
        targets[entity].transform.GetChild(1).renderer.material.color = color10;
      }
      targets[entity].transform.position = Vector2.Lerp(
          targets[entity].transform.position, position, 0.25f);
      targets[entity].transform.GetChild(0).localPosition = Vector2.Lerp(
          targets[entity].transform.GetChild(0).localPosition,
          new Vector2(-0.2f, 1.0f / 4.0f * (1 - j++)), 0.1f);
      targets[entity].transform.GetChild(0).gameObject.SetActive(entity.touched || entity.known);
      targets[entity].transform.GetChild(1).rotation = Quaternion.Slerp(
          targets[entity].transform.GetChild(1).rotation, rotation, 0.25f);
      var color0 = targets[entity].transform.GetChild(0).renderer.material.color;
      var color1 = targets[entity].transform.GetChild(1).renderer.material.color;
      color0.a = color1.a =  Mathf.Lerp(color0.a, 1.0f, 0.1f);
      targets[entity].transform.GetChild(0).renderer.material.color = color0;
      targets[entity].transform.GetChild(1).renderer.material.color = color1;
    }
    var missingEntities = targets.Keys.Where(key => !nearestEntities.Contains(key)).Cast<Entity>();
    var removal = new List<Entity>();
    foreach (var entity in missingEntities) {
      var color0 = targets[entity].transform.GetChild(0).renderer.material.color;
      var color1 = targets[entity].transform.GetChild(1).renderer.material.color;
      color0.a = color1.a = Mathf.Lerp(color0.a, 0.0f, 0.1f);
      targets[entity].transform.GetChild(0).renderer.material.color = color0;
      targets[entity].transform.GetChild(1).renderer.material.color = color1;
      if (color0.a <= 0.1f) {
        removal.Add(entity);
      }
    }
    foreach (var entity in removal) {
      Destroy(targets[entity]);
      targets.Remove(entity);
      entity.touched = false;
    }
  }

  private Vector2 PositionOf(Entity entity) {
    Vector2 position = transform.position;
    return position + entity.DistanceTo(transform.position) * entity.DirectionFrom(transform.position);
  }

  private Quaternion RotationOf(Entity entity) {
    var direction = entity.DirectionFrom(transform.position);
    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }

  void UpdateAreasAndEntities() {
    occupiedAreas = GetOccupiedAreas();
    nearestEntities = GetNearestEntities(occupiedAreas);
  }

  void UpdateArrowRotationAndScale(Vector2 input) {
    playerArrow.SetActive(true);
    blockedArrow.SetActive(false);
    var moving = Mathf.Clamp01(rigidbody2D.velocity.magnitude);
    var scale = arrowScale * Mathf.Clamp01(5.0f * input.magnitude);
    var frequency = 4.0f + 4.0f * IsRunning();
    arrow.transform.localPosition = (0.02f + 0.01f * IsRunning()) * moving * (
        Mathf.Sin(frequency * Time.fixedTime) * Vector2.right +
        Mathf.Sin(2.0f * frequency * Time.fixedTime) * Vector2.up);
    arrow.transform.rotation = gameObject.transform.rotation;
    arrow.transform.localScale = Vector2.Lerp(
        arrow.transform.localScale, new Vector2(scale, scale), 0.1f);
  }

  void UpdateCameraPosition() {
    Vector2 position = Vector2.Lerp(
        mainCamera.transform.position, transform.position + 0.0f * Vector3.right, cameraPositionAlpha);
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
