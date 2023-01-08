using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GeneType {HOMOZYGOUS, HETEROZYGOUS, HOMOZYGOUS_RECESSIVE}
[System.Serializable]
public class Gene
{
    public string Name;
    public GeneType Type;
    public float DominantValue;
    public float RecessiveValue;
}

[System.Serializable]
public class InventoryItem
{
    public string Name;
    public int Quantity = 1;
    public float Value = 0;
    public List<Gene> Genes;

    public float GetGrowthSpeed()
    {
        float speed = 1.0f;

        if (Genes == null)
            return speed;

        foreach (var gene in Genes)
        {
            if (gene.Name.ToLower().Contains("speed"))
            {
                if (gene.Type != GeneType.HOMOZYGOUS_RECESSIVE)
                {
                    speed *= gene.DominantValue;
                }
                else
                {
                    speed *= gene.RecessiveValue;
                }
            }
        }

        return speed;
    }

	public float GetYield()
	{
		float speed = 1.0f;

		if (Genes == null)
			return speed;

		foreach (var gene in Genes)
		{
			if (gene.Name.ToLower().Contains("yield"))
			{
				if (gene.Type != GeneType.HOMOZYGOUS_RECESSIVE)
				{
					speed *= gene.DominantValue;
				}
				else
				{
					speed *= gene.RecessiveValue;
				}
			}
		}

		return speed;
	}
}

public class Inventory : MonoBehaviour
{
    public GameObject InventoryScreen;
    public GameObject Toolbar;
    public GameObject SellBreedPanel;

    public TMPro.TMP_Text CashDisplay;

    public InputAction OpenInventoryAction;

    private PlayerController _player;

    public float CashBalance = 0;

    private List<InventorySlot> _slots;
    // Start is called before the first frame update
    void Start()
    {
        _slots = GetComponentsInChildren<InventorySlot>().ToList();
        _player = FindObjectOfType<PlayerController>();

        InventoryScreen.SetActive(false);
        SellBreedPanel.SetActive(false);
    }

	private void OnEnable()
	{
        OpenInventoryAction.Enable();
	}

	private void OnDisable()
	{
        OpenInventoryAction.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        if (OpenInventoryAction.WasPressedThisFrame())
        {
            InventoryScreen.SetActive(!InventoryScreen.activeInHierarchy);
            SellBreedPanel.SetActive(InventoryScreen.activeInHierarchy);
            if (!InventoryScreen.activeInHierarchy)
            {
				_player.PlayerRightClicked.Enable();
				_player.PlayerClicked.Enable();
				_player.PlayerControls.Enable();
			}
            else
            {
                _player.PlayerRightClicked.Disable();
                _player.PlayerClicked.Disable();
                _player.PlayerControls.Disable();
            }
        }

        CashDisplay.text = "$" + Math.Round(CashBalance, 2).ToString();
    }

    public void Collect(InventoryItem item)
    {
        foreach (var slot in _slots)
        {
            if (IsSlotSuitable(slot, item))
            {
                slot.GetInventoryItem().Quantity += item.Quantity;
                return;
            }
        }

        foreach (var slot in _slots)
        {
            if (slot.GetInventoryItem() == null)
            {
                slot.SetInventoryItem(item);
                return;
            }
        }
    }

	public bool CanRemove(InventoryItem item)
	{
		foreach (var slot in _slots)
		{
			if (slot.GetInventoryItem()?.Name == item.Name)
			{
                return true;
			}
		}

        return false;
	}

    public void Remove(InventoryItem item)
    {
		foreach (var slot in _slots)
		{
			if (slot.GetInventoryItem()?.Name == item.Name)
			{
				slot.GetInventoryItem().Quantity--;
				if (slot.GetInventoryItem().Quantity <= 0)
					slot.SetInventoryItem(null);
			}
		}
	}

    public bool IsSlotSuitable(InventorySlot slot, InventoryItem item)
    {
        var slotItem = slot.GetInventoryItem();
        if (slotItem?.Name == item.Name)
        {
            if (slotItem.Genes?.Count == item.Genes.Count)
            {
                for (int i = 0; i < slotItem.Genes.Count; i++)
                {
                    if (slotItem.Genes[i].Type != item.Genes[i].Type)
                        return false;
                }
            }
            return true;
        }
        return false;
	}
}
