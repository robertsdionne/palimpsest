using UnityEngine;
using System.Collections;
using System.Linq;

public class Player : MonoBehaviour {

  private const string HORIZONTAL = "Horizontal";
  private const string RUN = "Run";
  private const string SCENERY = "Scenery";
  private const string SEE = "See";
  private const string VERTICAL = "Vertical";

  public GameObject arrow;
  public float cameraPositionAlpha = 0.032f;
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

  Vector2 Abs(Vector2 input) {
    return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
  }

  void Describe(GameObject item) {
    if (item.GetComponent<Collider2D>().isTrigger) {
      item.GetComponent<Area>().Describe();
    } else {
      item.GetComponent<Scenery>().Describe();
    }
  }

  float DistanceTo(GameObject target, Vector2 position) {
    Vector2 center = target.transform.position;
    if (target.GetComponent<BoxCollider2D>().enabled) {
      Vector2 halfExtent = target.transform.localScale / 2.0f;
      return (Vector2.Max(Abs(center - position) - halfExtent, new Vector2())).magnitude;
    } else {
      float radius = target.transform.localScale.x;
      return (center - position).magnitude - radius;
    }
  }

  Vector2 GetInput() {
    return new Vector2(Input.GetAxis(HORIZONTAL), Input.GetAxis(VERTICAL));
  }

  float IsRunning() {
    return System.Convert.ToSingle(Input.GetButton(RUN));
  }

  void MaybeSee() {
    if (Input.GetButtonDown(SEE)) {
      var items = GameObject.FindGameObjectsWithTag(SCENERY);
      var areas = (GameObject.FindObjectsOfType(typeof(Area)) as Area[]).Where(
          area => area.IsOccupied()).Select(area => area.gameObject).ToList();
      var nearestItems = items.Where(item => !areas.Contains(item)).OrderBy(item =>
          DistanceTo(item, gameObject.transform.position)).ToList();
      Debug.Log("");
      WebsocketServer.BroadcastText("");
      TextConsole.PushText("");
      for (var i = 0; i < areas.Count; ++i) {
        areas[i].GetComponent<Area>().Inside();
      }
      for (var i = 0; i < 3; ++i) {
        Describe(nearestItems[i]);
      }
      Debug.Log("");
      WebsocketServer.BroadcastText("");
      TextConsole.PushText("");
    }
  }

  void UpdateArrowRotationAndScale() {
    var scale = 0.125f * Mathf.Clamp01(rigidbody2D.velocity.magnitude);
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
