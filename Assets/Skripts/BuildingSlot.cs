using UnityEngine;
using UnityEngine.UI;

public class BuildingSlot : MonoBehaviour
{

    public Image icon;
    Building building;
    int buildingNumber;

    public void AddBuilding(Building building, int buildingNumber)
    {
        gameObject.SetActive(true);
        this.building = building;
        this.buildingNumber = buildingNumber;
        transform.GetChild(0).GetComponent<Image>().sprite = building.Icon;
        transform.Find("Name").GetComponent<Text>().text = building.Name;
        transform.Find("CoinCost/Cost").GetComponent<Text>().text = building.GoldKosten.ToString();
        for(int i = 0; i < building.AddKosten.Count; i++)
        {
            Transform go = transform.Find("AdditionalCost" + i);
            go.Find(("AddCost" + i)).GetComponent<Text>().text = building.AddKosten[i].Value.ToString();
            go.Find("AddImage" + i).GetComponent<Image>().sprite = Statics.itemIcons[building.AddKosten[i].Key];
            go.gameObject.SetActive(true);
        }

    }
    public void ClearSlot()
    {
        gameObject.SetActive(false);
        building = null;
        buildingNumber = -1;

        icon.sprite = null;
        icon.enabled = false;

    }

    public void PlaceBuilding()
    {
        FindObjectOfType<GameManager>().GetComponent<GameManager>().InstantiateBuilding(building, buildingNumber);
    }
}
