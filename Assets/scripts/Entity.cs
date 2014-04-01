using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

  public string[] describe = {
    "See the entity."
  };

  public virtual void Describe() {
    if (describe.Length > 0) {
      TextConsole.PushIndicator(gameObject, Choose(describe));
    }
  }

  protected string Choose(string[] choices) {
    return choices[Random.Range(0, choices.Length)];
  }
}
