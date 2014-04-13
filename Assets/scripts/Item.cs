using UnityEngine;
using System.Collections;

public class Item : Entity {

  public string[] describeInventory = {
    "An item."
  };

  public void DescribeInventory() {
    if (describe.Length > 0) {
      TextConsole.PushText(Choose(describeInventory));
    }
  }

  public override void OnTouch(string text) {
    base.OnTouch(text);
    Inventory.Add(this);
  }
}
