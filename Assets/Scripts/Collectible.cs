using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public InventoryItem Item;
    private Inventory _inventory;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        _inventory.Collect(Item);
        Destroy(this.gameObject);
	}
}
