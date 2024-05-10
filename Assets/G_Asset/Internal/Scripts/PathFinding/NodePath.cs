using UnityEngine;

public class NodePath
{
    public Vector2 pos;
    public int index;
    public Vector2Int indexPos;
    public int gCost = 0;
    public int hCost = 0;
    public int fCost = 0;
    public bool isWalkable = true;
    public NodePath connection = null;
    public void CaculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void ResetNode()
    {
        connection = null;
        gCost = int.MaxValue;
        CaculateFCost();
    }
}
