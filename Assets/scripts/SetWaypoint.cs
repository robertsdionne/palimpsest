using UnityEngine;
using System.Collections;

public class SetWaypoint : MonoBehaviour {

  public Npc[] npcs = { null };
  public Waypoint waypoint;

	void OnEnable() {
    foreach (var npc in npcs) {
      if (null != npc) {
        npc.waypoint = waypoint;
      }
    }
	}
}
