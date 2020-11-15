

using System;
using UnityEngine;

public static class Statics
{

    public enum inventoryItems {Karotte}
    public static Sprite[] itemIcons;

    public static int getInventorySize()
    {
        return System.Enum.GetValues(typeof(inventoryItems)).Length;
    }

    public static string getItemName(int number)
    {
        return System.Enum.GetName(typeof(inventoryItems), number);
    }

    public static int getItemNumber(string name)
    {
        return (int)System.Enum.Parse(typeof(inventoryItems), name);
    }

    //Tower
    public static Tower towerOne = new Tower()
    {
        GoldKosten = 12
    };

    //Plants
    public static Field field = new Field()
    {
        Name = "Feld",
        Icon = Resources.Load<Sprite>("Icons/Feld_Icon"),
        Beschreibung = "Ein Feld, welches zum Anbauen von Pflanzen benötigt wird.",
        Mesh = Resources.Load<GameObject>("Plants/Feld"),
        GoldKosten = 10
    };

    public static Plant plantOne = new Plant()
    {
        Name = "Karotte",
        Icon = Resources.Load<Sprite>("Icons/Karotte_Icon"),
        Beschreibung = "Eine Karotte. Kann als Munition hergenommen werden.",
        GoldKosten = 8,
        growTime = 9,
        crop = 12,
        plantNumber = getItemNumber("Karotte"),
        Meshes = new GameObject[] {Resources.Load<GameObject>("Plants/Karotte_0"), Resources.Load<GameObject>("Plants/Karotte_1") , Resources.Load<GameObject>("Plants/Karotte_2") }
    };

    public static Enemy enemyOne = new Enemy()
    {
        Name = "enemyOne",
        Mesh = Resources.Load<GameObject>("Enemies/EnemyOne"),
        Velocity = 5f,
        MaxHitpoint = 15,
        Damage = 5
    };
}
