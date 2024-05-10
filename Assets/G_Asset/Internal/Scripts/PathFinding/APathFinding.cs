using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APathFinding
{
    public static int STRAIGHT = 10;
    public static int DIALOG = 14;
    private List<NodePath> processed = new();
    private List<NodePath> closed = new();

    private List<NodePath> nodesList = new();
    public APathFinding()
    {
        nodesList = MapGenerator.instance.GetNodeList();
    }

    public List<NodePath> FindPath(Vector2 from, Vector2 to)
    {
        NodePath startNode = GetNode(from);
        NodePath endNode = GetNode(to);

        if (startNode == null || endNode == null)
        {
            return null;
        }

        ResetNodes();

        startNode.hCost = GetDistance(from, to);
        startNode.connection = null;
        startNode.CaculateFCost();

        processed = new() { startNode };
        closed = new();
        while (processed.Count > 0)
        {
            NodePath currentNode = GetLowestFCost(processed);
            if (currentNode == endNode)
            {
                return GetPaths(endNode);
            }

            processed.Remove(currentNode);
            closed.Add(currentNode);

            foreach (NodePath neighbour in GetNeighbour(currentNode.pos))
            {
                if (closed.Contains(neighbour))
                {
                    continue;
                }

                if (!neighbour.isWalkable)
                {
                    closed.Add(neighbour);
                    continue;
                }

                int tentactiveGcost = currentNode.gCost + GetDistance(currentNode.pos, neighbour.pos);
                if (tentactiveGcost < neighbour.gCost)
                {
                    neighbour.gCost = tentactiveGcost;
                    neighbour.connection = currentNode;
                    neighbour.hCost = GetDistance(neighbour.pos, endNode.pos);
                    neighbour.CaculateFCost();

                    if (!processed.Contains(neighbour))
                    {
                        processed.Add(neighbour);
                    }
                }
            }
        }

        return null;
    }
    public List<NodePath> GetPaths(NodePath end)
    {
        List<NodePath> list = new() { end };
        NodePath currentNode = end;
        while (currentNode.connection != null)
        {
            list.Add(currentNode.connection);
            currentNode = currentNode.connection;
        }
        return list;
    }
    public List<NodePath> GetNeighbour(Vector2 to)
    {
        List<NodePath> list = new();

        Vector2 left = new(to.x - 0.16f, to.y);
        Vector2 topleft = new(to.x - 0.16f, to.y - 0.16f);
        Vector2 bottomLeft = new(to.x - 0.16f, to.y + 0.16f);

        Vector2 right = new(to.x + 0.16f, to.y);
        Vector2 topRight = new(to.x + 0.16f, to.y - 0.16f);
        Vector2 bottomRight = new(to.x + 0.16f, to.y + 0.16f);

        Vector2 top = new(to.x, to.y + 0.16f);
        Vector2 bottom = new(to.x, to.y - 0.16f);

        GetNodePath(left, list);
        GetNodePath(topleft, list);
        GetNodePath(bottomLeft, list);

        GetNodePath(right, list);
        GetNodePath(topRight, list);
        GetNodePath(bottomRight, list);

        GetNodePath(top, list);
        GetNodePath(bottom, list);

        return list;
    }
    public void GetNodePath(Vector2 pos, List<NodePath> list)
    {
        NodePath node = GetNode(pos);
        if (node != null)
        {
            list.Add(node);
        }
    }
    public NodePath GetLowestFCost(List<NodePath> list)
    {
        NodePath node = list[0];
        for (int i = 1; i < list.Count; i++)
        {
            if (node.fCost < list[i].fCost)
            {
                node = list[i];
            }
        }
        return node;
    }
    public int GetDistance(Vector2 from, Vector2 to)
    {
        NodePath startNode = GetNode(from);
        NodePath endNode = GetNode(to);

        int x = Mathf.Abs(startNode.indexPos.x - endNode.indexPos.x);
        int y = Mathf.Abs(startNode.indexPos.y - endNode.indexPos.y);
        int remain = Mathf.Abs(x - y);
        return DIALOG * Mathf.Min(x, y) + STRAIGHT * remain;
    }
    public NodePath GetNode(int index)
    {
        return nodesList[index];
    }
    public NodePath GetNode(Vector2 pos)
    {
        int offsetX = (int)(pos.x / 0.16f);
        int offsetY = (int)(pos.y / 0.16f);

        int index = offsetX * MapGenerator.instance.totalWidth + offsetY;

        return GetNode(index);
    }
    public void ResetNodes()
    {
        for (int i = 0; i < nodesList.Count; i++)
        {
            nodesList[i].ResetNode();
        }
    }
}