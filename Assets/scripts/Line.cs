using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

  public float height;
  public bool important = false;

  protected float creationTime;

  void Start() {
    creationTime = Time.fixedTime;
  }

  void Update() {
    var delay = important ? 10.0f : 5.0f;
    if (creationTime + delay < Time.fixedTime) {
      var amount = 1.0f - Mathf.Clamp01(Time.fixedTime - creationTime - delay);
      var color0 = GetComponent<TextMesh>().color;
      var color1 = transform.GetChild(0).renderer.material.color;
      color0.a = amount;
      color1.a = 192.0f / 255.0f * amount;
      GetComponent<TextMesh>().color = color0;
      transform.GetChild(0).renderer.material.color = color1;
    }
    if (creationTime + delay + 1.0f < Time.fixedTime) {
      Destroy(gameObject);
    }
  }

  public void OnCollisionEnter2D(Collision2D collision) {
    var line = collision.gameObject.GetComponent<Line>();
    if (null != line && !(line.important && !important) && line.creationTime < creationTime) {
      Destroy(collision.gameObject);
    }
  }

  public void SetText(string text) {
    GetComponent<TextMesh>().text = text;
    GetComponent<BoxCollider2D>().size = renderer.bounds.size;
    GetComponent<BoxCollider2D>().center = new Vector2(renderer.bounds.extents.x, 0.0f);
    transform.GetChild(0).transform.localPosition = new Vector3(renderer.bounds.extents.x, 0.0f, 0.1f);
    transform.GetChild(0).transform.localScale = new Vector3(renderer.bounds.size.x, renderer.bounds.size.y, 1.0f);
  }

  public Vector2 Bottom() {
    return gameObject.transform.localPosition - height / 2.0f * Vector3.up;
  }

  public Vector2 Top() {
    return gameObject.transform.localPosition + height / 2.0f * Vector3.up;
  }
}
