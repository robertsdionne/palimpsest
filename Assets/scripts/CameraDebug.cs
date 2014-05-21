using UnityEngine;
using System.Collections;

public class CameraDebug : MonoBehaviour {

  public const int NO_MASK = -1;
  public const int PLAYER_AND_TEXT_MASK = 256 | 512;

  public bool debug = false;
  public int targetFrameRate = 60;

  void Start() {
    Application.targetFrameRate = targetFrameRate;
  }

  void Update () {
    if (debug) {
      camera.cullingMask = NO_MASK;
    } else {
      camera.cullingMask = PLAYER_AND_TEXT_MASK;
    }
  }
}
