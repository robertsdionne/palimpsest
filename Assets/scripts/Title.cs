using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

  private const string EXAMINE = "Examine";

  public bool button = true;
  public string nextScene;
	
	void Update() {
    if (button) {
      if (Input.GetButtonDown(EXAMINE)) {
        Application.LoadLevel(nextScene);
      }
    } else {
      Application.LoadLevel(nextScene);
    }
	}
}
