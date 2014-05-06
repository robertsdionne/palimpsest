using UnityEngine;
using System.Collections;

public class SnapTo : MonoBehaviour {

  public GameObject snapTo;

	void OnEnable() {
    transform.position = snapTo.transform.position;
  }
}
