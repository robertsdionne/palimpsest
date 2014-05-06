using UnityEngine;
using System.Collections;

public class SetObjective : MonoBehaviour {

  public GameObject objective;
  public string text;

  public void OnEnable() {
    objective.GetComponent<TextMesh>().text = text;
    objective.SetActive(true);
  }
}
