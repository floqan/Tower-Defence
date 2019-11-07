using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    int[] inventory;
    InventorySlot[] slots;
    public Transform inventoryParent;

    int maxSpace = 25;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Inventory was instanciated more then one");
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        inventory = new int[Statics.getInventorySize()];
        for (int i = 0; i < inventory.Length; i++){
            inventory[i] = 0;
        }
        slots = inventoryParent.GetComponentsInChildren<InventorySlot>();
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.Length)
            {
                slots[i].initItem(i);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void addItemsToInventory(int item, int count)
    {
        inventory[item] += count;
        if (inventory[item] > maxSpace)
        {
            inventory[item] = maxSpace;
        }
        slots[item].updateDisplay();

    }

    public bool removeItemsFromInventory(int item, int count)
    {
        if (inventory[item] - count < 0)
        {
            return false;
        }
        else
        {
            inventory[item] -= count;
            slots[item].updateDisplay();
            return true; 
        }
    }

    public int getMaxSpace()
    {
        return maxSpace;
    }

    public void setMaxSpace(int maxSpace) {
        this.maxSpace = maxSpace;
            }

    public int getItemCount(int item)
    {
        return inventory[item];
    }


}
