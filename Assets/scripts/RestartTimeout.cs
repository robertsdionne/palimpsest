using UnityEngine;
using System.Collections;

public class RestartTimeout : MonoBehaviour {

  public float timeout = 90.0f;

  private static RestartTimeout instance = null;

  private float lastInteractTime = 0.0f;

  void Start() {
    instance = this;
    Interact();
  }

  public static void Interact() {
    if (null != instance) {
      instance.lastInteractTime = Time.time;
    }
  }

	void Update() {
    if (Input.GetButtonDown("Restart") || Time.time - lastInteractTime > timeout) {
      Application.LoadLevel("title");
    }
	}
}
