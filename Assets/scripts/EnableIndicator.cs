using UnityEngine;
using System.Collections;

public class EnableIndicator : MonoBehaviour {

  public Player player;
  public Entity target;

	void Start () {
    player.fieldArrow.SetActive(true);
    player.fieldArrowTarget = target;
    gameObject.SetActive(false);
	}
}
