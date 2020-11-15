using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BottomUI : MonoBehaviour
{

    public Transform towerParent;
    public Transform plantsParent;
    public GameManager gm;
    public Button plantsButton;
    public Button towerButton;

    TowerList tower;
    BuildingSlot[] towerSlots;
    PlantList plants;
    BuildingSlot[] plantSlots;

    private int getCurrentGold()
    {
        return gm.getCurrentGold();
    }

    private float getCurrentHealthForUI()
    {
        return ((float)gm.getCurrentHealth()) / 100f;
    }

    private int getCurrentWave()
    {
        return gm.getCurrentWave();
    }

    // Start is called before the first frame update
    void Start()
    {

        tower = TowerList.instance;

        towerSlots = towerParent.GetComponentsInChildren<BuildingSlot>();
        for (int i = 0; i < towerSlots.Length; i++)
        {
            if (i < TowerList.instance.getLength())
            {
                towerSlots[i].AddBuilding(TowerList.instance.towes[i], i);
            }
            else
            {
                towerSlots[i].ClearSlot();
            }
        }

        plants = PlantList.instance;

        plantSlots = plantsParent.GetComponentsInChildren<BuildingSlot>();
        for (int i = 0; i < plantSlots.Length; i++)
        {
            if (i < plants.getLength())
            {
                plantSlots[i].AddBuilding(plants.plants[i], i);
            }
            else
            {
                plantSlots[i].ClearSlot();
            }
        }

        plantsButton.GetComponent<Button>().onClick.AddListener(showPlants);
        towerButton.GetComponent<Button>().onClick.AddListener(showTower);
        
        showPlants();
        UpdateStatsUI();
    }

    void showTower()
    {
        plantsParent.gameObject.SetActive(false);
        towerParent.gameObject.SetActive(true);
    }

    void showPlants()
    {
        towerParent.gameObject.SetActive(false);
        plantsParent.gameObject.SetActive(true);

    }

    public string stringToEdit = "Hello World";

    void OnGUI()
    {
        // Make a text field that modifies stringToEdit.
        stringToEdit = GUI.TextField(new Rect(10, 10, 200, 20), stringToEdit, 25);
    }

    public void UpdateStatsUI()
    {
        GameObject tmp = GameObject.FindGameObjectWithTag("Stats");
        if (tmp != null)
        {
            tmp.transform.GetChild(1).GetComponent<Text>().text = getCurrentGold().ToString();
            tmp.transform.GetChild(3).GetChild(1).GetComponent<Image>().fillAmount = getCurrentHealthForUI();
            tmp.transform.GetChild(5).GetComponent<Text>().text = getCurrentWave().ToString();
        }
    }
}
