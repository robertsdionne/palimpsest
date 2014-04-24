using UnityEngine;
using System.Collections;

public class DisableIndicator : MonoBehaviour {

  public Player player;

	void Start () {
    player.fieldArrow.SetActive(false);
    player.fieldArrowTarget = null;
    gameObject.SetActive(false);
	}
}
