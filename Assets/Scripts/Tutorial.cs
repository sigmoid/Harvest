using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Tutorial : MonoBehaviour
{
    public RawImage Image;
    public List<Texture> ImageList;

    private int _selectedIdx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Next()
    {
        _selectedIdx++;

        if (_selectedIdx >= ImageList.Count)
            _selectedIdx = 0;

        UpdateImage();
    }

    public void Previous()
    {
		_selectedIdx--;

		if (_selectedIdx < 0)
			_selectedIdx = ImageList.Count - 1;

		UpdateImage();
	}

    private void UpdateImage()
    {
        Image.texture = ImageList[_selectedIdx];
    }
}
