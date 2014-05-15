using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

  public const float BIG_SHAKE_ALPHA = 0.02f;
  public const float MEDIUM_SHAKE_ALPHA = 0.05f;
  public const float SHAKE_ALPHA = 0.2f;
  public const float SHAKE_THRESHOLD = 0.001f;

  private static ScreenShake screenShake;

  private float shake = 0.0f;
  private float alpha = SHAKE_ALPHA;

  public static void Shake() {
    if (null != screenShake && screenShake.shake < SHAKE_THRESHOLD) {
      screenShake.alpha = SHAKE_ALPHA;
      screenShake.shake = 0.05f;
    }
  }

  public static void ShakeMedium() {
    if (null != screenShake && screenShake.shake < SHAKE_THRESHOLD) {
      screenShake.alpha = MEDIUM_SHAKE_ALPHA;
      screenShake.shake = 2.0f;
    }
  }

  public static void ShakeBig() {
    if (null != screenShake && screenShake.shake < SHAKE_THRESHOLD) {
      screenShake.alpha = BIG_SHAKE_ALPHA;
      screenShake.shake = 5.0f;
    }
  }

  void Start() {
    screenShake = this;
  }

	void Update() {
    if (shake > SHAKE_THRESHOLD) {
      gameObject.transform.localPosition = shake * Random.insideUnitCircle;
      shake = Mathf.Lerp(shake, 0.0f, alpha);
    }
	}
}
