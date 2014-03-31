using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Loader : MonoBehaviour {

  public GameObject areaPrefab;
  public Mesh boxMesh;
  public Mesh circleMesh;
  public GameObject objectPrefab;
  public TextAsset sceneJson;

	// Use this for initialization
	void Start () {
    ReadScene(Facebook.MiniJSON.Json.Deserialize(sceneJson.text) as Dictionary<string, object>);
	}

  void ReadAabb(GameObject prefab, Dictionary<string, object> json) {
    var aabbJson = json["aabb"] as Dictionary<string, object>;
    var minimum = ReadVector2(aabbJson["minimum"]);
    var maximum = ReadVector2(aabbJson["maximum"]);
    var position = (minimum + maximum) / 2.0f;
    var size = maximum - minimum;
    var aabb = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
    aabb.name = json["name"] as string;
    aabb.transform.localScale = new Vector3(size.x, size.y, 1);
    aabb.GetComponent<CircleCollider2D>().enabled = false;
    aabb.GetComponent<MeshFilter>().sharedMesh = boxMesh;
    if (areaPrefab == prefab) {
      ReadAreaMessages(aabb, json["messages"] as Dictionary<string, object>);
    } else {
      ReadObjectMessages(aabb, json["messages"] as Dictionary<string, object>);
    }
  }

  void ReadCircle(GameObject prefab, Dictionary<string, object> json) {
    var position = ReadVector2(json["position"]);
    var radius = (float) (double) json["radius"];
    var circle = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
    circle.name = json["name"] as string;
    circle.transform.localScale = new Vector3(radius, radius, 1);
    circle.GetComponent<BoxCollider2D>().enabled = false;
    circle.GetComponent<MeshFilter>().sharedMesh = circleMesh;
    if (areaPrefab == prefab) {
      ReadAreaMessages(circle, json["messages"] as Dictionary<string, object>);
    } else {
      ReadObjectMessages(circle, json["messages"] as Dictionary<string, object>);
    }
  }

  void ReadItem(GameObject prefab, Dictionary<string, object> item) {
    if (item.ContainsKey("position")) {
      ReadCircle(prefab, item);
    } else {
      ReadAabb(prefab, item);
    }
  }

  void ReadAreaMessages(GameObject obj, Dictionary<string, object> json) {
    if (json.ContainsKey("describe")) {
      obj.GetComponent<Area>().describe = (
          json["describe"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("enter")) {
      obj.GetComponent<Area>().enter = (json["enter"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("exit")) {
      obj.GetComponent<Area>().exit = (json["exit"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("inside")) {
      obj.GetComponent<Area>().inside = (json["inside"] as List<object>).Cast<string>().ToArray();
    }
  }

  void ReadObjectMessages(GameObject obj, Dictionary<string, object> json) {
    if (json.ContainsKey("touch")) {
      obj.GetComponent<Obstacle>().touch = (json["touch"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("describe")) {
      obj.GetComponent<Obstacle>().describe = (
          json["describe"] as List<object>).Cast<string>().ToArray();
    }
  }

  void ReadScene(Dictionary<string, object> scene) {
    var areas = scene["areas"] as List<object>;
    foreach (var area in areas) {
      ReadItem(areaPrefab, area as Dictionary<string, object>);
    }
    var objects = scene["objects"] as List<object>;
    foreach (var obj in objects) {
      ReadItem(objectPrefab, obj as Dictionary<string, object>);
    }
  }

  Vector2 ReadVector2(object json) {
    var vector = json as Dictionary<string, object>;
    return new Vector2((float) (double) vector["x"], (float) (double) vector["y"]);
  }
}
