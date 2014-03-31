using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

  public Font fontEva;
  public Font fontUbuntu;
  public Texture ps4Cross;
  public Texture ps4L1;
  public Texture ps4LeftStick;
  public bool showGui = true;
	
	// Update is called once per frame
	void OnGUI() {
    if (showGui) {
      var style = new GUIStyle();
      style.font = fontEva;
      GUI.Label(new Rect(100, 100, 100, 20), "Palimpsest", style);
      style.font = fontUbuntu;
      GUI.Label(new Rect(100, 200, 100, 20), "Palimpsest", style);
      GUI.Label(new Rect(100, 300, ps4LeftStick.width, ps4LeftStick.height), ps4LeftStick);
      GUI.Label(new Rect(100, 400, ps4Cross.width, ps4Cross.height), ps4Cross);
      GUI.Label(new Rect(100, 500, ps4L1.width, ps4L1.height), ps4L1);
    }
	}
}
