using UnityEngine;
using System.Collections;

public class Waypoint : Collidable {

  void OnDrawGizmos() {
    Gizmos.color = transform.parent.GetComponent<Path>().color;
    Gizmos.DrawWireSphere(transform.position, transform.parent.GetComponent<Path>().width / 2.0f);
  }
}
