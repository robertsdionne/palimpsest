using UnityEngine;
using System.Collections;

public class SetWaypoint : MonoBehaviour {

  public Npc[] npcs = { null };
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
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
      foreach (var disable in disables) {
        if (null != disable) {
          disable.SetActive(false);
        }
      }
      foreach (var next in nexts) {
        if (null != next) {
          next.SetActive(true);
        }
      }
      gameObject.SetActive(false);
    }
  }
}
