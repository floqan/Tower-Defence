using UnityEngine;
using UnityEngine.AI;

public class Enemy 
{
    public string Name;
    public int MaxHitpoint;
    public int CurrentHitpoints;
    public int Damage;
    public int Strength;
    public GameObject Mesh;

    public float Velocity;
    public Vector3 NextGoal;
    public Vector3 LastGoal;

    public Enemy()
    {

    }

    public Enemy(Enemy enemy)
    {
        this.Name = enemy.Name;
        MaxHitpoint = enemy.MaxHitpoint;
        CurrentHitpoints = enemy.CurrentHitpoints; ;
        Strength = enemy.Strength; ;
        Mesh = enemy.Mesh;
        Velocity = enemy.Velocity;
        Damage = enemy.Damage;
    }
}
