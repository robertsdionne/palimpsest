using UnityEngine;
using System.Collections;

public class Item : Entity {

  public string[] inventoryListing = {
    "An item."
  };

  public void ListInventory() {
    TextConsole.PushText(Utilities.Choose(inventoryListing), transform.position, Vector2.up);
  }

  public override void OnTouch(string text, Vector2 position, Vector2 normal) {
    base.OnTouch(text, position, normal);
    Inventory.Add(this);
  }
}
