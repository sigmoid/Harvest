using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    public Tilemap PlantsMap;
    public Tilemap GroundMap;

    public Tile DirtTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Harvest(Vector3 position)
    {
        var cellPos = PlantsMap.WorldToCell(position);
        PlantsMap.SetTile(cellPos, null);
    }

    public void Hoe(Vector3 position)
    {
        var cellPos = GroundMap.WorldToCell(position);
        GroundMap.SetTile(cellPos, DirtTile);
    }
}
