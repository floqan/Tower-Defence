using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public int item;
    public Text text;


    public void initItem(int item)
    {
        gameObject.SetActive(true);
        transform.Find("Icon").GetComponent<Image>().sprite = Statics.itemIcons[item];
        this.item = item;
        updateDisplay();
    }

    public void ClearSlot()
    {
        gameObject.SetActive(false);
        text.text = "";
        
    }

    public void updateDisplay()
    {
        text.text = Inventory.instance.getItemCount(item).ToString();
        if(Inventory.instance.getItemCount(item) == Inventory.instance.getMaxSpace())
        {
            text.color = Color.red;
        }
        else
        {
            text.color = Color.black;
        }
    }

}
