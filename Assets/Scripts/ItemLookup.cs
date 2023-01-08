using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NameLookupPair
{
    public string Name;
    public GameObject Obj;
}

public class ItemLookup : MonoBehaviour
{
    public List<NameLookupPair> KeyValuePairs;
    public Dictionary<string, GameObject> ItemTextureLookup;
    // Start is called before the first frame update
    void Start()
    {
        ItemTextureLookup = new Dictionary<string, GameObject>();
        foreach (var kvp in KeyValuePairs)
        {
            ItemTextureLookup[kvp.Name] = kvp.Obj;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
