using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

  public const float SHAKE_ALPHA = 0.2f;
  public const float SHAKE_THRESHOLD = 0.001f;

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
    if (shake > SHAKE_THRESHOLD) {
      gameObject.transform.localPosition = shake * Random.insideUnitCircle;
      shake = Mathf.Lerp(shake, 0.0f, SHAKE_ALPHA);
    }
	}
}
