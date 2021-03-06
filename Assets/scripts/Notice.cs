﻿using UnityEngine;
using System.Collections;

public class Notice : MonoBehaviour {

  public AudioSource audioSource;
  public string[] notices;
  public GameObject at;
  public GameObject[] nexts = { null };
  public GameObject[] disables = { null };
  public bool choose = false;
  public bool recycle = false;
  public bool removable = false;
  public bool screenShake = false;
  public bool usable = false;
  public bool long_duration = true;
  public bool important = true;
  public bool dialogue = false;

  void OnTriggerEnter2D(Collider2D other) {
    if (!Utilities.IsPlayer(other.gameObject)) {
      return;
    }
    foreach (Transform child in transform) {
      child.gameObject.SetActive(true);
    }
    if (usable) {
      return;
    }
    Trigger();
  }

  void OnTriggerStay2D(Collider2D other) {
    if (!Utilities.IsPlayer(other.gameObject)) {
      return;
    }
    if (!usable) {
      return;
    }
    if (Input.GetButtonDown("Examine")) {
      RestartTimeout.Interact();
      Trigger();
    }
  }

  private void Trigger() {
    if (screenShake) {
      ScreenShake.Shake();
    }
    if (audioSource) {
      Instantiate(audioSource);
    }
    if (notices.Length > 0) {
      if (choose) {
        TextConsole.PushText(Utilities.Choose(notices),
            null == at ? gameObject.transform.position : at.transform.position,
                Vector2.up, important, dialogue, long_duration);
      } else {
        TextConsole.PushText(string.Join("\n", notices),
            null == at ? gameObject.transform.position : at.transform.position,
                Vector2.up, important, dialogue, long_duration);
      }
    }
    foreach (var disable in disables) {
      if (null != disable) {
        disable.SetActive(false);
      }
    }
    foreach (var next in nexts) {
      if (null != next) {
        next.SetActive(true);
      }
    }
    if (!recycle) {
      if (removable) {
        Destroy(gameObject);
      } else {
        gameObject.SetActive(false);
      }
    }
  }
}
