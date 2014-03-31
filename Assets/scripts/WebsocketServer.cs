using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

public class WebsocketServer : MonoBehaviour {

  class Service : WebSocketService {

    protected override void OnMessage(MessageEventArgs e) {
      Send(e.Data);
    }
  }

  class Resource {
    public byte[] content;
    public string contentType;

    public Resource(byte[] content, string contentType) {
      this.content = content;
      this.contentType = contentType;
    }
  }

  private const string APPLICATION_JAVASCRIPT = "application/javascript";
  private const string APPLICATION_TRUE_TYPE_FONT = "application/x-font-ttf";
  private const string IMAGE_PNG = "image/png";
  private const string TEXT_HTML = "text/html";

  public TextAsset clientJs;
  public TextAsset eva1Ttf;
  public TextAsset indexHtml;
  public TextAsset ps4Cross;
  public TextAsset ps4L1;
  public TextAsset ps4LeftStick;
  public TextAsset ubuntuMonoTtf;

  private static Dictionary<string, Resource> resourceMap;
  private static HttpServer server;

  public static void BroadcastText(string text) {
    if (null != server) {
      server.WebSocketServices.Broadcast(Facebook.MiniJSON.Json.Serialize(
          new Dictionary<string, string> {
            {"type", "text"},
            {"text", text}
          }));
    }
  }

  public static void BroadcastIndicator(GameObject target, string text) {
    if (null != server) {
      server.WebSocketServices.Broadcast(Facebook.MiniJSON.Json.Serialize(
          new Dictionary<string, object> {
            {"type", "composite"},
            {"messages", new List<object> {
                new Dictionary<string, object> {
                  {"type", "entity"},
                  {"id", target.GetInstanceID()}
                },
                new Dictionary<string, string> {
                  {"type", "text"},
                  {"text", text}
                }
              }}
          }));
    }
  }

  public static void BroadcastTelemetry(Dictionary<string, float> position,
      Dictionary<string, float> direction, Dictionary<int, object> directions) {
    server.WebSocketServices.Broadcast(Facebook.MiniJSON.Json.Serialize(
        new Dictionary<string, object>() {
          {"type", "telemetry"},
          {"position", position},
          {"direction", direction},
          {"directions", directions}
        }
      ));
  }

  void Start() {
    resourceMap = new Dictionary<string, Resource>() {
      {"/client.js", new Resource(clientJs.bytes, APPLICATION_JAVASCRIPT)},
      {"/EVA1.ttf", new Resource(eva1Ttf.bytes, APPLICATION_TRUE_TYPE_FONT)},
      {"/", new Resource(indexHtml.bytes, TEXT_HTML)},
      {"/PS4_Cross.png", new Resource(ps4Cross.bytes, IMAGE_PNG)},
      {"/PS4_L1.png", new Resource(ps4L1.bytes, IMAGE_PNG)},
      {"/PS4_Left_Stick.png", new Resource(ps4LeftStick.bytes, IMAGE_PNG)},
      {"/Ubuntu-M.ttf", new Resource(ubuntuMonoTtf.bytes, APPLICATION_TRUE_TYPE_FONT)}
    };
    server = new HttpServer(8888);
    server.AddWebSocketService<Service>("/", () => new Service() {
      Protocol = "interactive-fiction-protocol"
    });
    server.OnGet += (sender, e) => {
      if (!resourceMap.ContainsKey(e.Request.RawUrl)) {
        e.Response.StatusCode = (int) HttpStatusCode.NotFound;
      } else {
        var resource = resourceMap[e.Request.RawUrl];
        e.Response.ContentType = resource.contentType;
        e.Response.WriteContent(resource.content);
      }
    };
    server.Start();
    Application.OpenURL("http://localhost:8888");
  }

  void Update() {
    var player = GameObject.FindWithTag("Player");
    var items = GameObject.FindGameObjectsWithTag("Scenery");
    var directions = new Dictionary<int, object>();
    foreach (var item in items) {
      directions[item.GetInstanceID()] = WriteVector2(
          DirectionFrom(item, player.transform.position));
    }
    BroadcastTelemetry(WriteVector2(player.transform.position),
        WriteVector2(player.rigidbody2D.velocity.normalized), directions);
  }

  Dictionary<string, float> WriteVector2(Vector2 vector) {
    return new Dictionary<string, float>() {
        {"x", vector.x},
        {"y", vector.y}
      };
  }

  Vector2 DirectionFrom(GameObject target, Vector2 position) {
    var dx = 1e-5f * Vector2.right;
    var dy = 1e-5f * Vector2.up;
    var direction = -new Vector2(
        DistanceTo(target, position + dx) - DistanceTo(target, position - dx),
        DistanceTo(target, position + dy) - DistanceTo(target, position - dy));
    return direction.magnitude > 0.0f ? direction.normalized : new Vector2();
  }

  float DistanceTo(GameObject target, Vector2 position) {
    Vector2 center = target.transform.position;
    if (target.GetComponent<BoxCollider2D>().enabled) {
      Vector2 halfExtent = target.transform.localScale / 2.0f;
      return (Vector2.Max(Abs(center - position) - halfExtent, new Vector2())).magnitude;
    } else {
      float radius = target.transform.localScale.x;
      return (center - position).magnitude - radius;
    }
  }

  Vector2 Abs(Vector2 input) {
    return new Vector2(Mathf.Abs(input.x), Mathf.Abs(input.y));
  }

  void OnApplicationQuit() {
    if (null != server) {
      server.Stop();
    }
  }
}
