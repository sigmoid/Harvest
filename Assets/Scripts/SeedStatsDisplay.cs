using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SeedStatsDisplay : MonoBehaviour
{
    public TMP_Text StatsDisplay;
    public InventoryItem Item;
    // Start is called before the first frame update
    void Start()
    {
        Item = GetComponent<DraggableItem>().InventoryItem;
    }

    // Update is called once per frame
    void Update()
    {
		StatsDisplay.text = "S: " + Item.GetGrowthSpeed()+ "\n"+ "Y: " + Item.GetYield();
	}
}
