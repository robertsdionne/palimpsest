using UnityEngine;
using System.Collections;

public class SetWaypoint : MonoBehaviour {

  public Npc[] npcs = { null };
  public float time;
  public Waypoint waypoint;
  
  private float startTime;

  void Start() {
    OnEnable();
  }

	void OnEnable() {
    startTime = Time.time;
	}

  void Update() {
    if (Time.time - startTime > time) {
      foreach (var npc in npcs) {
        if (null != npc) {
          npc.waypoint = waypoint;
        }
      }
      gameObject.SetActive(false);
    }
  }
}
