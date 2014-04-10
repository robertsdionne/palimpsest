using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

  private const string SEE = "See";

  public string nextScene;
	
	void Update() {
    if (Input.GetButtonDown(SEE)) {
      Application.LoadLevel(nextScene);
    }
	}
}
