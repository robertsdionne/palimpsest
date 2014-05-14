using UnityEngine;
using System.Collections;

public class MeteorStrike : MonoBehaviour {

  public bool big = false;

	void OnEnable() {
    if (big) {
      ScreenShake.ShakeBig();
    } else {
      ScreenShake.ShakeMedium();
    }
    gameObject.SetActive(false);
	}
}
