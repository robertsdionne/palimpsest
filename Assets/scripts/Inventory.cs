using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

  protected List<Item> items = new List<Item>();

  private static Inventory instance;

  void Start() {
    instance = this;
  }

  public static void Add(Item item) {
    if (null != instance && !instance.items.Contains(item)) {
      instance.items.Add(item);
      item.gameObject.SetActive(false);
    }
  }

  public static bool Contains(Item item) {
    return null != instance && instance.items.Contains(item);
  }

  public static List<Item> Items() {
    if (null != instance) {
      return instance.items;
    } else {
      return new List<Item>();
    }
  }

  public static void Remove(Item item) {
    if (null != instance) {
      instance.items.Remove(item);
    }
  }
}
