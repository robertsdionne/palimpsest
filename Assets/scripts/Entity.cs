using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

  public string[] describe = {
    "See the entity."
  };

  protected bool seen = false;

  public virtual void Describe() {
    seen = true;
    if (describe.Length > 0) {
      TextConsole.PushIndicator(gameObject, Choose(describe));
    }
  }

  public bool IsSeen() {
    return seen;
  }

  protected string Choose(string[] choices) {
    return choices[Random.Range(0, choices.Length)];
  }
}
