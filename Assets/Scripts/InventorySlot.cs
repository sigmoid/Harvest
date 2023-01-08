using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Color HighlightedColor;
    public Color RegularColor;

    public GameObject ItemPrefab;

    private RawImage _image;

    private bool _selected = false;

    public event Action OnDropped;

	public void OnDrop(PointerEventData eventData)
	{
        OnDropped.Invoke();
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
            draggableItem.ParentAfterDrag = transform;
        }

        
	}

	// Start is called before the first frame update
	void Start()
    {
        _image = GetComponent<RawImage>();
        OnDropped = new Action(() => { });
    }

    // Update is called once per frame
    void Update()
    {
		var text = GetComponentInChildren<Text>();

        if (text != null && GetInventoryItem() != null)
        {
            text.text = GetInventoryItem().Quantity.ToString();
        }
    }

    public void Select()
    {
        _selected = true;
        _image.color = HighlightedColor;
    }

    public void Deselect()
    {
        _selected = false;
        _image.color = RegularColor;
    }

    public InventoryItem GetInventoryItem()
    {
        var child = GetComponentInChildren<DraggableItem>();
        if (child != null)
            return child.InventoryItem;

        return null;
    }

    public void SetInventoryItem(InventoryItem item)
    {

        var textureLookup = FindObjectOfType<ItemLookup>();
		var child = GetComponentInChildren<DraggableItem>();

		if (item == null && child != null)
		{
            Destroy(child.gameObject);
            return;
		}

		if (child != null)
            child.InventoryItem = item;
        else
        {
            var tmp = GameObject.Instantiate(textureLookup.ItemTextureLookup[item.Name], transform);
            tmp.GetComponent<DraggableItem>().InventoryItem = item;
		}
	}

    public void Take()
    {
        var child = GetComponentInChildren<DraggableItem>();

        if (child != null)
        {
            if (child.InventoryItem.Quantity > 0)
                child.InventoryItem.Quantity--;

            if(child.InventoryItem.Quantity <= 0)
                GameObject.Destroy(child.gameObject);
        }
    }

}
