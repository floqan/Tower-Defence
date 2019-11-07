using System.Collections.Generic;
using UnityEngine;

public class Level1 : MonoBehaviour
{
    public int size;
    public GameObject BuildingOne;
    public GameObject BuildingTwo;
    public List<GameObject> enemies; 
    private void Start()
    {
        
        // TODO init Inventory Sprites
        // TODO init Inventory Enum
         
        
        Tower tmp = new Tower();
        //for(int i = 0; i < size; i++)
        //{
        tmp.Name = "Tower 1";
        tmp.AddKosten.Add(new KeyValuePair<int, int>(0, 5));
        tmp.rotationTime = 10.0f;
        tmp.attackRadius = 20.0f;
        tmp.attackSpeed = 1f;
        tmp.bullet = Resources.Load<GameObject>("Bullets/BulletOne");
        tmp.Mesh = BuildingOne;// GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tmp.GoldKosten = 3;
            Sprite sprite = Resources.Load<Sprite>("Images/Healthbar");
//            tmp.ButtonIcon  = Resources.Load<Sprite>("Assets/Images/Healthbar.png") as Sprite;
            if(sprite == null) //tmp.ButtonIcon == null)
            {
                Debug.LogWarning("Sprite konnte nicht geladen werden.");
            }
            tmp.Icon = sprite;
        TowerList.instance.AddBuilding(tmp);
        //}
        Tower tmp2 = new Tower();
        tmp2.attackRadius = 15.0f;
        tmp2.rotationTime = 5.0f;
        tmp2.Name = "Tower 2";
        tmp2.Mesh = BuildingTwo;//Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere));
        tmp2.GoldKosten = 12;
        tmp2.Icon = sprite;
        TowerList.instance.AddBuilding(tmp2);
        GameManager.instance.setMaxGold(500);
        GameManager.instance.IncreaseCurrentGold(60);

        PlantList.instance.AddBuilding(Statics.field);
        PlantList.instance.AddBuilding(Statics.plantOne);

        Statics.itemIcons = new Sprite[] { Resources.Load<Sprite>("Icons/Karotte_Icon") };

        GameManager.instance.enemies.Enqueue(new Enemy(Statics.enemyOne)); 
        GameManager.instance.enemies.Enqueue(new Enemy(Statics.enemyOne)); 
        GameManager.instance.enemies.Enqueue(new Enemy(Statics.enemyOne));
        //Load Enemies
        /*Enemy enemy = new Enemy();
        enemy.Name = "Enemy One";
        //enemy.Mesh = enemies[0];
        enemy.Velocity = new Vector3(1, 0, 0);
        GameManager.instance.passiveEnemies.Add(new Enemy(Statics.enemyOne));
        GameManager.instance.passiveEnemies.Add(new Enemy(Statics.enemyOne));
        GameManager.instance.passiveEnemies.Add(new Enemy(Statics.enemyOne));
        /*GameManager.instance.passiveEnemies.Add(enemy);
        GameManager.instance.passiveEnemies.Add(enemy);
        GameManager.instance.passiveEnemies.Add(enemy);
        */

    }
}
