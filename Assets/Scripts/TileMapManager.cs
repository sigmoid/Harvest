using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum GroundTileType { GRASS, DIRT };
public enum PlantTileType { GRASS_TUFT, CROP}

public class HarvestTile
{
    public GroundTileType TileType = GroundTileType.GRASS;
    public PlantTileType? PlantTileType = null;
    public int PlantStage = 0;
    public float PlantedTime;
    public InventoryItem SeedData;
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

    public Tile GrassTile;

    public float NoiseScale = 0.5f;
    public float NoiseThreshold = 0.5f;

    public GameObject CropItem;

    public BreedingManager BreedingManager;

    // Start is called before the first frame update
    void Start()
    {
        _tileTypes = new Dictionary<(int, int), HarvestTile>();
        var bounds = PlantsMap.cellBounds;

        for (int y = bounds.yMin; y < bounds.yMax; y++)
        {
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);

                float u = (float)(x - bounds.xMin) / (float)(bounds.xMax - bounds.xMin);
                float v = (float)(y - bounds.yMin) / (float)(bounds.yMax - bounds.yMin);

                u *= NoiseScale;
                v *= NoiseScale;

                float sample = Mathf.PerlinNoise(u, v);

                if (sample > NoiseThreshold)
                {
                    PlantsMap.SetTile(cellPos, GrassTile);
                    _tileTypes[(x, y)] = new HarvestTile() { PlantTileType = PlantTileType.GRASS_TUFT };
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var cell in _tileTypes)
        {
            var tile = cell.Value;
            if (tile.PlantStage > 0)
            {
                float t = Mathf.Clamp01((Time.time - tile.PlantedTime) / (PlantLifetime / cell.Value.SeedData.GetGrowthSpeed()));
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

		if (_tileTypes.ContainsKey(cellTuple) && (_tileTypes[cellTuple].TileType == GroundTileType.DIRT || _tileTypes[cellTuple].PlantTileType == PlantTileType.GRASS_TUFT))
		{
            if (_tileTypes[cellTuple].PlantTileType == PlantTileType.CROP)
            {
                if (_tileTypes[cellTuple].PlantStage == PlantStages.Count)
                {
                    int numCrop = Mathf.RoundToInt(_tileTypes[cellTuple].SeedData.GetYield());
                    if (numCrop <= 0)
                        numCrop = 1;
					SpawnCrop(numCrop, cellPos.x, cellPos.y);
                }
				SpawnSeeds(1, 4, cellTuple.x, cellTuple.y, _tileTypes[cellTuple].SeedData);
			}
			else if ( _tileTypes[cellTuple].PlantTileType == PlantTileType.GRASS_TUFT)
            {
                SpawnSeeds(0, 2, cellTuple.x, cellTuple.y, GetRandomSeed());
            }
            _tileTypes[cellTuple].PlantedTime = 0;
            _tileTypes[cellTuple].PlantTileType = null;
            _tileTypes[cellTuple].PlantStage = 0;
		}
	}

    public void Hoe(Vector3 position)
    {
		var cellPos = PlantsMap.WorldToCell(position);
		var cellTuple = (cellPos.x, cellPos.y);
		if (_tileTypes.ContainsKey(cellTuple) && _tileTypes[cellTuple].PlantTileType != null)
        {
            return;
        }

        _tileTypes[(cellPos.x, cellPos.y)] = new HarvestTile() { TileType = GroundTileType.DIRT };
        GroundMap.SetTile(cellPos, DirtTile);
    }

    public bool Plant(Vector3 position, InventoryItem seedData)
    {
		var cellPos = PlantsMap.WorldToCell(position);
        var cellTuple = (cellPos.x, cellPos.y);

		if (_tileTypes.ContainsKey(cellTuple) 
            && _tileTypes[cellTuple].TileType == GroundTileType.DIRT 
            && _tileTypes[cellTuple].PlantStage == 0)
        {
            _tileTypes[cellTuple].PlantStage = 1;
            _tileTypes[cellTuple].PlantedTime = Time.time;
            _tileTypes[cellTuple].PlantTileType = PlantTileType.CROP;
            _tileTypes[cellTuple].SeedData = seedData;
            return true;
        }
        return false;
	}

    private void SpawnSeeds(int minDrop, int maxDrop, int x, int y, InventoryItem item)
    {
		
		int numSeeds = Random.Range(minDrop, maxDrop);

        for (int i = 0; i < numSeeds; i++)
        {
			Vector3 offset = Random.insideUnitCircle * SeedPlacementRange;
			var tmp = GameObject.Instantiate(Seed, new Vector3(x + 0.5f, y + 0.5f, 0) + offset, Quaternion.identity);
            item.Quantity = 1;
            tmp.GetComponent<Collectible>().Item = item;
        }
	}

    private void SpawnCrop(int numCrop, int x, int y)
    {
		for (int i = 0; i < numCrop; i++)
		{
			Vector3 offset = Random.insideUnitCircle * SeedPlacementRange;
			GameObject.Instantiate(CropItem, new Vector3(x + 0.5f, y + 0.5f, 0) + offset, Quaternion.identity);
		}
	}

    private InventoryItem GetRandomSeed()
    {
        return BreedingManager.GetRandomSeed();
    }
}

