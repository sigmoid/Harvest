using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Transform ParentAfterDrag;

	public InventoryItem InventoryItem;

	public void OnBeginDrag(PointerEventData eventData)
	{
		ParentAfterDrag = transform.parent;
		transform.SetParent(transform.root);
		transform.SetAsLastSibling();
		GetComponent<Image>().raycastTarget = false;

	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Mouse.current.position.ReadValue();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.SetParent(ParentAfterDrag);
		GetComponent<Image>().raycastTarget = true;
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
