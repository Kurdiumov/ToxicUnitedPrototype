using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject UnitArrayPool;

    public GameObject UnitPanelTemplate;

    private List<Unit> AvailableUnits;

	// Use this for initialization
	void Start () {
        AvailableUnits = new List<Unit>();
        addUnit("TestUnit");
        
    }

    //Add unit by passing it's prefab
    public void addUnit(string UnitPrefabName) {

        // Find prefab by passed name
        GameObject unitPrefab = GameObject.Find(UnitPrefabName);

        // UnitPanel will be "mutation" of generic Template - with specified texture
        GameObject UnitPanel = UnitPanelTemplate;

        // Load texture from prefab's script
        Texture2D image = unitPrefab.GetComponent<Unit>().Image;

        // Set loaded texture as active one
        UnitPanel.GetComponent<RawImage>().texture = (Texture2D) image;

        // Instantiate mutated prefab
        Instantiate(UnitPanelTemplate, UnitArrayPool.transform);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
