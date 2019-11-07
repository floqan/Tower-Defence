using UnityEngine;

public class GridPlatz
{
    public bool besetzt;
    public Building Gebäude;
    public bool target;
    public int enemyCounter;

    public GridPlatz()
    {
        besetzt = false;
        target = false;
        Gebäude = null;
    }

    public bool isBauenErlaubt()
    {
        return enemyCounter < 1 && !besetzt;
    }

    public bool isField()
    {
        return Gebäude is Field;
    }
}
