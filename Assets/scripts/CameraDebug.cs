using UnityEngine;
using System.Collections;

public class CameraDebug : MonoBehaviour {

  public bool debug = false;

  void Update () {
    if (debug) {
      camera.cullingMask = -1;
    } else {
      camera.cullingMask = 256;
    }
  }
}
