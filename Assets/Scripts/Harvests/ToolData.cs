using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "tool", menuName = "item/New tool")]
public class ToolData : ItemData
{
    public ToolData() {
        type = ItemType.Tool;
        stackable = false;
        price = 0;
    }
}
