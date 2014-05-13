using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

  public static float DELAY_MARGIN = 1.0f; // seconds
  public static float FADE_OFFSET_Z = 0.1f;
  public static float IMPORTANT_DELAY = 10.0f; // seconds
  public static Vector3 IMPORTANT_MESSAGE_MARGINS = new Vector3(0.2f, 0.2f, 0.0f); // meters
  public static float NORMAL_DELAY = 5.0f; // seconds
  public static float MAXIMUM_FADE_ALPHA = 192.0f / 255.0f;

  public float height;
  public bool important = false;

  protected float creationTime;

  void Start() {
    creationTime = Time.fixedTime;
  }

  void Update() {
    var delay = important ? IMPORTANT_DELAY : NORMAL_DELAY;
    if (creationTime + delay < Time.fixedTime) {
      var amount = 1.0f - Mathf.Clamp01(Time.fixedTime - creationTime - delay);
      var color0 = GetComponent<TextMesh>().color;
      var color1 = transform.GetChild(0).renderer.material.color;
      color0.a = amount;
      color1.a = MAXIMUM_FADE_ALPHA * amount;
      GetComponent<TextMesh>().color = color0;
      transform.GetChild(0).renderer.material.color = color1;
    }
    if (creationTime + delay + DELAY_MARGIN < Time.fixedTime) {
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
    GetComponent<BoxCollider2D>().size = renderer.bounds.size + (
        important ? IMPORTANT_MESSAGE_MARGINS : Vector3.zero);
    GetComponent<BoxCollider2D>().center = new Vector2(renderer.bounds.extents.x, 0.0f);
    transform.GetChild(0).transform.localPosition = new Vector3(
        renderer.bounds.extents.x, 0.0f, FADE_OFFSET_Z);
    transform.GetChild(0).transform.localScale = new Vector3(
        renderer.bounds.size.x, renderer.bounds.size.y, 1.0f);
  }
}
