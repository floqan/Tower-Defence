using System.Collections.Generic;
using UnityEngine;

public class Building
{
    public Sprite Icon;
    public string Name;
    public string Beschreibung;
    public int GoldKosten;
    public List<KeyValuePair<int,int>> AddKosten = new List<KeyValuePair<int, int>>(); //First number is the item, second the needed amount
    public GameObject Mesh;

    public Building()
    {
       
    }

    public Building(Building building)
    {
        Icon = building.Icon;
        Name = building.Name;
        Beschreibung = building.Beschreibung;
        GoldKosten = building.GoldKosten;
        AddKosten = building.AddKosten;
        Mesh = building.Mesh;
    }
}
