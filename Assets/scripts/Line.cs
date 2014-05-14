using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

  public const float DELAY_MARGIN = 1.0f; // seconds
  public const float FADE_OFFSET_Z = 0.1f;
  public const float LONG_DELAY = 10.0f; // seconds
  public static Vector3 IMPORTANT_MESSAGE_MARGINS = new Vector3(0.2f, 0.2f, 0.0f); // meters
  public const float NORMAL_DELAY = 5.0f; // seconds
  public const float MAXIMUM_FADE_ALPHA = 192.0f / 255.0f;

  public float height;
  public bool long_duration = false;
  public bool important = false;

  protected float creationTime;
  protected GameObject bold;
  protected GameObject regular;
  protected GameObject quad;

  void Start() {
    creationTime = Time.fixedTime;
  }

  void Update() {
    var delay = long_duration ? LONG_DELAY : NORMAL_DELAY;
    if (creationTime + delay < Time.fixedTime) {
      var amount = 1.0f - Mathf.Clamp01(Time.fixedTime - creationTime - delay);
      var color0 = regular.GetComponent<TextMesh>().color;
      var color1 = bold.GetComponent<TextMesh>().color;
      var color2 = quad.renderer.material.color;
      color0.a = color1.a = amount;
      color2.a = MAXIMUM_FADE_ALPHA * amount;
      regular.GetComponent<TextMesh>().color = color0;
      bold.GetComponent<TextMesh>().color = color1;
      quad.renderer.material.color = color2;
    }
    if (creationTime + delay + DELAY_MARGIN < Time.fixedTime) {
      Destroy(gameObject);
    }
  }

  public void OnTriggerEnter2D(Collider2D other) {
    var line = other.gameObject.GetComponent<Line>();
    if (null != line && !(line.important && !important) && line.creationTime < creationTime) {
      Destroy(other.gameObject);
    }
  }

  public void OnTriggerStay2D(Collider2D other) {
    var line = other.gameObject.GetComponent<Line>();
    if (null != line && !important && line.important) {
      transform.position = new Vector3(transform.position.x, transform.position.y - 1.0f / 60.0f, transform.position.z);
    }
  }

  public void OnCollisionEnter2D(Collision2D collision) {
    var line = collision.gameObject.GetComponent<Line>();
    if (null != line && !(line.important && !important) && line.creationTime < creationTime) {
      Destroy(collision.gameObject);
    }
  }

  public void SetText(string text) {
    regular = transform.GetChild(0).gameObject;
    bold = transform.GetChild(1).gameObject;
    quad = transform.GetChild(2).gameObject;
    regular.GetComponent<TextMesh>().text = text;
    bold.GetComponent<TextMesh>().text = text;
    var bounds = regular.activeSelf ? regular.renderer.bounds : bold.renderer.bounds;
    GetComponent<BoxCollider2D>().size = bounds.size + (
        important ? IMPORTANT_MESSAGE_MARGINS : Vector3.zero);
    GetComponent<BoxCollider2D>().center = new Vector2(bounds.extents.x, 0.0f);
    quad.transform.localPosition = new Vector3(bounds.extents.x, 0.0f, FADE_OFFSET_Z);
    quad.transform.localScale = new Vector3(bounds.size.x, bounds.size.y, 1.0f);
  }
}
