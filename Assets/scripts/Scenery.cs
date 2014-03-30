using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

  void OnCollisionEnter2D() {
    Debug.Log("Test");
  }
}
