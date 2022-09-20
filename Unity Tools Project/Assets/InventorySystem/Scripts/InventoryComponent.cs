using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryComponent : MonoBehaviour
{
    public string inventoryName;
    public int inventorySize;
    public List<Item> inventoryItems;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item itemToAdd)
    {
        //check if there is room in the inventory before adding the item
        if(inventoryItems.Count < inventorySize)
        {
            inventoryItems.Add(itemToAdd);
        }
    }

    public void RemoveItem(Item itemToRemove)
    {
        //check if the item is in the inventory before trying to remove it
        if(inventoryItems.Contains(itemToRemove))
        {
            inventoryItems.Remove(itemToRemove);
        }
    }

}
