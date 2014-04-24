using UnityEngine;
using System.Collections;

public class Item : Entity {

  public string[] inventoryListing = {
    "An item."
  };

  public void ListInventory() {
    TextConsole.PushText(Utilities.Choose(inventoryListing));
  }

  public override void OnTouch(string text) {
    base.OnTouch(text);
    Inventory.Add(this);
  }
}
