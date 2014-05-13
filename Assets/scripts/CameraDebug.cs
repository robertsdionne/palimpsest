using UnityEngine;
using System.Collections;

public class CameraDebug : MonoBehaviour {

  public static int NO_MASK = -1;
  public static int PLAYER_AND_TEXT_MASK = 256 | 512;

  public bool debug = false;

  void Update () {
    if (debug) {
      camera.cullingMask = NO_MASK;
    } else {
      camera.cullingMask = PLAYER_AND_TEXT_MASK;
    }
  }
}
