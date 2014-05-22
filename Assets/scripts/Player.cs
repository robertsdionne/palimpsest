using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

  private const string HORIZONTAL = "Horizontal";
  private const string RUN = "Run";
  private const string SEE = "See";
  private const float VELOCITY_MOTION_SCALE = 2.0f;
  private const string VERTICAL = "Vertical";
  private const float WALK_SOUND_ALPHA = 0.1f;
  private const float WALK_SOUND_MAXIMUM_VOLUME = 0.2f;

  public AudioSource idleSound;
  public float idleSoundMaximumVolume = 0.1f;
  public AudioSource walkSound;
  public GameObject targetPrefab;
  public float cameraPositionAlpha = 0.032f;
  public GameObject mainCamera;
  public float maximumAngularVelocity = 10.0f;
  public float maximumForce = 50.0f;
  public float maximumTorque = 10.0f;
  public float maximumWalkingSpeed = 1.38f;
  public int numberToSee = 3;
  public float viewDistance = 2.0f;

  private List<Entity> occupiedAreas = new List<Entity>();
  private List<Entity> nearestEntities = new List<Entity>();
  private Dictionary<Entity, GameObject> targets = new Dictionary<Entity, GameObject>();

  void Start() {
    Screen.showCursor = false;
  }

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
    UpdateIdleAndWalkSoundVolumes();
	}

  void Update() {
    MaybeSee();
  }

  private float IsMoving() {
    return Mathf.Clamp01(VELOCITY_MOTION_SCALE * rigidbody2D.velocity.magnitude);
  }

  private Vector2 GetInput() {
    var input = new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
    if (input.magnitude > 0.1f) {
      RestartTimeout.Interact();
    }
    return input;
  }

  private Vector2 GetInput2() {
    var input = new Vector2(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));
    if (input.magnitude > 0.1f) {
      RestartTimeout.Interact();
    }
    return input;
  }

  private List<Entity> GetOccupiedAreas() {
    return (GameObject.FindObjectsOfType(typeof(Entity)) as Entity[]).Where(
          entity => entity.IsOccupied() && entity.visible).ToList();
  }

  private List<Entity> GetNearestEntities(List<Entity> occupiedAreas) {
    var entities = GameObject.FindObjectsOfType(typeof(Entity)) as Entity[];
    return entities.Where(entity => !occupiedAreas.Contains(entity) && entity.visible).OrderBy(
        entity => entity.DistanceTo(gameObject.transform.position)).Take(numberToSee).ToList();
  }

  private void MaybeSee() {
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
    return position + entity.DistanceTo(position) * entity.DirectionFrom(position);
  }

  private Quaternion RotationOf(Entity entity) {
    var direction = entity.DirectionFrom(transform.position);
    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    return Quaternion.AngleAxis(angle, Vector3.forward);
  }

  private void UpdateAreasAndEntities() {
    occupiedAreas = GetOccupiedAreas();
    nearestEntities = GetNearestEntities(occupiedAreas);
  }

  private void UpdateCameraPosition() {
    if (null != mainCamera) {
      Vector3 input2 = viewDistance * GetInput2();
      Vector2 position = Vector2.Lerp(
          mainCamera.transform.position, transform.position + input2, cameraPositionAlpha);
      mainCamera.transform.position = new Vector3(
          position.x, position.y, mainCamera.transform.position.z);
    }
  }

  private void UpdatePosition(Vector2 input) {
    rigidbody2D.AddForce(maximumForce * rigidbody2D.mass * input);
    rigidbody2D.velocity = Vector3.ClampMagnitude(rigidbody2D.velocity, maximumWalkingSpeed);
  }

  private void UpdateRotation(Vector2 input) {
    rigidbody2D.angularVelocity = Mathf.Clamp(
        rigidbody2D.angularVelocity, -maximumAngularVelocity, maximumAngularVelocity);
    if (input.magnitude > 0.0f) {
      var torque = Vector3.Cross(rigidbody2D.transform.right, input.normalized).z;
      rigidbody2D.AddTorque(maximumTorque * rigidbody2D.mass * torque);
    }
  }

  private void UpdateIdleAndWalkSoundVolumes() {
    idleSound.volume = Mathf.Lerp(
        idleSound.volume, idleSoundMaximumVolume * (1.0f - IsMoving()), WALK_SOUND_ALPHA);
    walkSound.volume = Mathf.Lerp(
        walkSound.volume, WALK_SOUND_MAXIMUM_VOLUME * IsMoving(), WALK_SOUND_ALPHA);
  }
}
