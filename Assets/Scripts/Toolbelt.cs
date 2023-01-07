using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class Tool
{
    public string Name;
}

public class Toolbelt : MonoBehaviour
{
    public InputAction Scroll;

    public List<Tool> Tools;

    private int _selectedToolIdx = 0;

    // Start is called befo re the first frame update
    void Start()
    {
        
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
        if (scroll > 0)
        {
            _selectedToolIdx++;
            if (_selectedToolIdx >= Tools.Count)
                _selectedToolIdx = 0;
        }

        if (scroll < 0)
        {
            _selectedToolIdx--;
            if (_selectedToolIdx < 0)
                _selectedToolIdx = Tools.Count - 1;
        }
    }

    public Tool GetCurrentTool()
    {
        return Tools[_selectedToolIdx];
    }
}
