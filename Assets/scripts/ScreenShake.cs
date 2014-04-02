using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

  private static ScreenShake screenShake;
  
  private float shake = 0.0f;

  public static void Shake() {
    if (null != screenShake) {
      screenShake.shake = 0.05f;
    }
  }

  void Start() {
    screenShake = this;
  }
	
	void Update() {
    gameObject.transform.localPosition = shake * Random.insideUnitCircle;
    shake = Mathf.Lerp(shake, 0.0f, 0.2f);
	}
}
