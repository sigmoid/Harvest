using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string Name;
}

public class Inventory : MonoBehaviour
{
    private List<InventoryItem> _items;
    // Start is called before the first frame update
    void Start()
    {
        _items = new List<InventoryItem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect(InventoryItem item)
    {
        Debug.Log("Add " + item.Name + " to inventory.");
        _items.Add(item);
    }
}
