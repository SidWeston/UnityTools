using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "InventorySystem/New Item", order = 1)]
public class Item : ScriptableObject
{
    public enum ItemType
    { 
        CONSUMABLE,
        WEAPON,
        TOOL,
        ARMOUR,
        OTHER
    }

    public ItemType itemType;
    public string itemName;
    public string itemDescription;
    public Image itemImage;
    public bool stackable;
    public int maxStackCount;
    public int itemIndex;

}
