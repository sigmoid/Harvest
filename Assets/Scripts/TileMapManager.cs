using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GroundTileType { GRASS, DIRT };

public class HarvestTile
{
    public GroundTileType TileType = GroundTileType.GRASS;
    public int PlantStage = 0;
    public float PlantedTime;
}

public class TileMapManager : MonoBehaviour
{
    public Tilemap PlantsMap;
    public Tilemap GroundMap;

    public Tile DirtTile;
    public Tile SeedTile;

    public Dictionary<(int, int), HarvestTile> _tileTypes;

    public List<Tile> PlantStages = new List<Tile>();
    public float PlantLifetime;

    public GameObject Seed;
    public float SeedPlacementRange = 0.4f;

    // Start is called before the first frame update
    void Start()
    {
        _tileTypes = new Dictionary<(int, int), HarvestTile>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cell in _tileTypes)
        {
            var tile = cell.Value;
            if (tile.PlantStage > 0)
            {
                float t = Mathf.Clamp01((Time.time - tile.PlantedTime) / PlantLifetime);
                int currentStage = Mathf.RoundToInt((t * (float)(PlantStages.Count-1)));
                PlantsMap.SetTile(new Vector3Int(cell.Key.Item1, cell.Key.Item2), PlantStages[currentStage]);
                tile.PlantStage = currentStage + 1;
            }
        }
    }

    public void Harvest(Vector3 position)
    {
        var cellPos = PlantsMap.WorldToCell(position);
        PlantsMap.SetTile(cellPos, null);

        var cellTuple = (cellPos.x, cellPos.y);

		if (_tileTypes.ContainsKey(cellTuple) && _tileTypes[cellTuple].TileType == GroundTileType.DIRT)
		{
            if (_tileTypes[cellTuple].PlantStage == PlantStages.Count)
            {
                Vector3 offset = Random.insideUnitCircle * SeedPlacementRange;
				GameObject.Instantiate(Seed, new Vector3(cellTuple.Item1 + 0.5f, cellTuple.Item2 + 0.5f, 0) + offset, Quaternion.identity);
            }
			_tileTypes[cellTuple].PlantStage = 0;
			_tileTypes[cellTuple].PlantedTime = Time.time;
		}
	}

    public void Hoe(Vector3 position)
    {
        var cellPos = GroundMap.WorldToCell(position);
        _tileTypes[(cellPos.x, cellPos.y)] = new HarvestTile() { TileType = GroundTileType.DIRT };
        GroundMap.SetTile(cellPos, DirtTile);
    }

    public void Plant(Vector3 position)
    {
		var cellPos = PlantsMap.WorldToCell(position);
        var cellTuple = (cellPos.x, cellPos.y);

		if (_tileTypes.ContainsKey(cellTuple) 
            && _tileTypes[cellTuple].TileType == GroundTileType.DIRT 
            && _tileTypes[cellTuple].PlantStage == 0)
        {
            _tileTypes[cellTuple].PlantStage = 1;
            _tileTypes[cellTuple].PlantedTime = Time.time;
        }
	}
}
