using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
  
	void FixedUpdate() {
    transform.rotation = Quaternion.Euler(0.0f, 0.0f, Time.fixedTime);
	}
}
