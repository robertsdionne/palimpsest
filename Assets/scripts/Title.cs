using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

  private const string EXAMINE = "Examine";

  public string nextScene;
	
	void Update() {
    if (Input.GetButtonDown(EXAMINE)) {
      Application.LoadLevel(nextScene);
    }
	}
}
