using UnityEngine;
using System.Collections;

public class Title : MonoBehaviour {

  private const string EXAMINE = "Examine";

  public bool button = true;
  public string nextScene;
  public bool exit;

	void Update() {
    if (exit) {
      Application.Quit();
      return;
    }
    if (button) {
      if (Input.GetButtonDown(EXAMINE)) {
        if (null != audio) {
          audio.Play();
        }
        Application.LoadLevel(nextScene);
      }
    } else {
      Application.LoadLevel(nextScene);
    }
	}
}
