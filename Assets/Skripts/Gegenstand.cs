using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gegenstand
{
    public Sprite Icon;
    private string name;

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return name;
    }

}
