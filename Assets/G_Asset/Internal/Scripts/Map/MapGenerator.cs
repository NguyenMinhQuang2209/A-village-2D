using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator instance;

    [SerializeField] private Texture2D mapNoise;
    [SerializeField] private Tilemap groundTile;
    [SerializeField] private TileBase groundBase;
    [SerializeField] private TileBase waterBase;
    [SerializeField] private Color waterColor;
    [SerializeField] private TileBase sandBase;
    [SerializeField] private Color sandColor;
    [SerializeField] private TileBase snowBase;
    [SerializeField] private Color snowColor;

    [SerializeField] private float size = 0.159f;
    public float width = 0;
    public float height = 0;

    public int totalWidth = 0;
    public int totalHeight = 0;

    private List<NodePath> nodesList = new();
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        SpawnGrid();
    }
    public void SpawnGrid()
    {
        totalWidth = mapNoise.width;
        totalHeight = mapNoise.height;

        width = totalWidth * size;
        height = totalHeight * size;

        Color[] pixels = mapNoise.GetPixels();
        for (int i = 0; i < totalWidth; i++)
        {
            for (int j = 0; j < totalHeight; j++)
            {
                int index = i * totalWidth + j;
                Color color = pixels[index];
                TileBase baseTile = groundBase;
                if (color.Equals(waterColor))
                {
                    baseTile = waterBase;
                }
                else if (color.Equals(sandColor))
                {
                    baseTile = sandBase;
                }
                else if (color.Equals(snowColor))
                {
                    baseTile = snowBase;
                }
                Vector3Int pos = new(i, j, 0);
                groundTile.SetTile(pos, baseTile);
            }
        }
    }
    public List<NodePath> GetNodeList()
    {
        List<NodePath> list = new();
        for (int i = 0; i < totalWidth; i++)
        {
            for (int j = 0; j < totalHeight; j++)
            {
                int index = i * totalWidth + j;
                Vector2 storePos = new(i * 0.16f, j * 0.16f);
                NodePath node = new()
                {
                    pos = storePos,
                    index = index,
                    connection = null,
                    gCost = int.MaxValue,
                    indexPos = new(i, j)
                };
                list[index] = node;
            }
        }
        return list;
    }
}
