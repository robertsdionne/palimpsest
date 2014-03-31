using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {

  protected string Choose(string[] choices) {
    return choices[Random.Range(0, choices.Length)];
  }
}
