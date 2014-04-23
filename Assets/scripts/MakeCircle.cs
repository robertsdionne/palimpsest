using UnityEngine;
using System.Collections;

public class MakeCircle : MonoBehaviour {

  public int number = 100;
  public float overlap = 0.5f;
  public float radius = 25.0f;
  public bool shift = false;
  public float thickness = 1.0f;

  public GameObject obstacle;

	// Use this for initialization
	void Start () {
    var step = 2.0f * Mathf.PI / number;
    for (int i = 0; i < number; ++i) {
      var angle = (i + (shift ? 0.5f : 0.0f)) * step;
      var x = (radius + thickness / 2.0f - overlap) * Mathf.Cos(angle);
      var y = (radius + thickness / 2.0f - overlap) * Mathf.Sin(angle);
      var wall = Instantiate(obstacle) as GameObject;
      wall.transform.localScale = new Vector3(
          thickness, (radius + thickness - overlap) * step, 1.0f);
      wall.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle * Mathf.Rad2Deg);
      wall.transform.parent = transform;
      wall.transform.position = transform.position + new Vector3(x, y, 0.0f);
    }
	}
}
