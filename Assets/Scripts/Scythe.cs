using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Scythe : MonoBehaviour
{
    private Transform[] _tips;

    private TileMapManager _tileMap;

    private BoxCollider2D _collider;

    // Start is called before the first frame update
    void Start()
    {
        _tileMap = FindObjectOfType<TileMapManager>();
        _collider = GetComponent<BoxCollider2D>();
        _tips = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var tip in _tips)
        {
            Vector3 centerPosition = tip.position;

            Vector3Int scytheTilePosition = new Vector3Int(Mathf.RoundToInt(centerPosition.x), Mathf.RoundToInt(centerPosition.y));

            _tileMap.Harvest(scytheTilePosition);
        }
    }
}
