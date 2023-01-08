using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

public class Store : MonoBehaviour
{
    public GameObject Slot;
    private InventorySlot _slot;

    public Slider UISlider;
    public TMPro.TMP_Text AmountLabel;

    private Inventory _inventory;


    // Start is called before the first frame update
    void Start()
    {
        _slot = Slot.GetComponent<InventorySlot>();
        _inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        var item = _slot.GetInventoryItem();

        if (item != null)
        {
            AmountLabel.text = Mathf.RoundToInt(item.Quantity * UISlider.value).ToString();
        }
    }
    public void Confirm()
    {
		var item = _slot.GetInventoryItem();

		if (item != null)
		{
			int amount = Mathf.RoundToInt(item.Quantity * UISlider.value);
            item.Quantity -= amount;
            _inventory.CashBalance += item.Value * amount;

            if (item.Quantity <= 0)
            {
                var objToDestroy = Slot.gameObject.GetComponentInChildren<DraggableItem>().gameObject;
                GameObject.Destroy(objToDestroy);
            }
		}
	}
}
