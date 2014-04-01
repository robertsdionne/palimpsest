using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Loader : MonoBehaviour {

  public GameObject areaPrefab;
  public Mesh boxMesh;
  public Mesh circleMesh;
  public GameObject obstaclePrefab;
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
    aabb.GetComponent<BoxCollider2D>().enabled = true;
    aabb.GetComponent<CircleCollider2D>().enabled = false;
    aabb.GetComponent<MeshFilter>().sharedMesh = boxMesh;
    if (areaPrefab == prefab) {
      ReadAreaMessages(aabb.GetComponent<Area>(), json["messages"] as Dictionary<string, object>);
    } else {
      ReadObstacleMessages(
          aabb.GetComponent<Obstacle>(), json["messages"] as Dictionary<string, object>);
    }
  }

  void ReadCircle(GameObject prefab, Dictionary<string, object> json) {
    var position = ReadVector2(json["position"]);
    var radius = (float) (double) json["radius"];
    var circle = Object.Instantiate(prefab, position, Quaternion.identity) as GameObject;
    circle.name = json["name"] as string;
    circle.transform.localScale = new Vector3(radius, radius, 1);
    circle.GetComponent<BoxCollider2D>().enabled = false;
    circle.GetComponent<CircleCollider2D>().enabled = true;
    circle.GetComponent<MeshFilter>().sharedMesh = circleMesh;
    if (areaPrefab == prefab) {
      ReadAreaMessages(circle.GetComponent<Area>(), json["messages"] as Dictionary<string, object>);
    } else {
      ReadObstacleMessages(
          circle.GetComponent<Obstacle>(), json["messages"] as Dictionary<string, object>);
    }
  }

  void ReadItem(GameObject prefab, Dictionary<string, object> item) {
    if (item.ContainsKey("position")) {
      ReadCircle(prefab, item);
    } else {
      ReadAabb(prefab, item);
    }
  }

  void ReadAreaMessages(Area area, Dictionary<string, object> json) {
    if (json.ContainsKey("describe")) {
      area.describe = (json["describe"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("enter")) {
      area.enter = (json["enter"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("exit")) {
      area.exit = (json["exit"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("inside")) {
      area.inside = (json["inside"] as List<object>).Cast<string>().ToArray();
    }
  }

  void ReadObstacleMessages(Obstacle obstacle, Dictionary<string, object> json) {
    if (json.ContainsKey("touch")) {
      obstacle.touch = (json["touch"] as List<object>).Cast<string>().ToArray();
    }
    if (json.ContainsKey("describe")) {
      obstacle.describe = (json["describe"] as List<object>).Cast<string>().ToArray();
    }
  }

  void ReadScene(Dictionary<string, object> scene) {
    var areas = scene["areas"] as List<object>;
    foreach (var area in areas) {
      ReadItem(areaPrefab, area as Dictionary<string, object>);
    }
    var obstacles = scene["objects"] as List<object>;
    foreach (var obstacle in obstacles) {
      ReadItem(obstaclePrefab, obstacle as Dictionary<string, object>);
    }
  }

  Vector2 ReadVector2(object json) {
    var vector = json as Dictionary<string, object>;
    return new Vector2((float) (double) vector["x"], (float) (double) vector["y"]);
  }
}
