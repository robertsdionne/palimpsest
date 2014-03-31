using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

  public float height;

  public Vector2 Bottom() {
    return gameObject.transform.localPosition - height / 2.0f * Vector3.up;
  }
}
