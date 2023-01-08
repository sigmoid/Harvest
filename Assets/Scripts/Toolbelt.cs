using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Linq;

public class Toolbelt : MonoBehaviour
{
    public InputAction Scroll;

    private List<InventorySlot> _slots;

    private int _selectedToolIdx = 0;

    // Start is called befo re the first frame update
    void Start()
    {
        _slots = GetComponentsInChildren<InventorySlot>().ToList();
    }

	private void OnEnable()
	{
        Scroll.Enable();
	}

	private void OnDisable()
	{
        Scroll.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        var scroll = Scroll.ReadValue<float>();
        if (scroll < 0)
        {
            _slots[_selectedToolIdx].Deselect();

            _selectedToolIdx++;
            if (_selectedToolIdx >= _slots.Count)
                _selectedToolIdx = 0;

            _slots[_selectedToolIdx].Select();
        }

        if (scroll > 0)
        {
			_slots[_selectedToolIdx].Deselect();

			_selectedToolIdx--;
            if (_selectedToolIdx < 0)
                _selectedToolIdx = _slots.Count - 1;

			_slots[_selectedToolIdx].Select();
		}
    }

    public InventoryItem GetCurrentTool()
    {
        return _slots[_selectedToolIdx].GetInventoryItem();
    }

    public bool Remove()
    {
        if (_slots[_selectedToolIdx] == null)
            return false;

        _slots[_selectedToolIdx].GetInventoryItem().Quantity--;

        if (_slots[_selectedToolIdx].GetInventoryItem().Quantity <= 0)
        {
            GameObject.Destroy(_slots[_selectedToolIdx].GetComponentInChildren<DraggableItem>().gameObject);
        }

        return true;
    }
}
