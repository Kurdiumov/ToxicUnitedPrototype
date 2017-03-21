﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject UnitArrayPool;

    public GameObject UnitPanelTemplate;

    private List<Unit> AvailableUnits;

    // Use this for initialization
    void Start()
    {
        // Init empty unit array
        AvailableUnits = new List<Unit>();

        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            AddUnit(Unit.GetComponent<Unit>());
        }
    }

    //Add unit by passing it's prefab
    public void AddUnit(Unit unit)
    {
        
        // Find prefab by passed name
       // GameObject unitPrefab = GameObject.Find(UnitPrefabName);

        // UnitPanel will be "mutation" of generic Template - with specified texture
        GameObject UnitPanel = UnitPanelTemplate;

        // Load texture from prefab's script
        Texture2D image = unit.Image;

        // Set loaded texture as active one
        UnitPanel.GetComponent<RawImage>().texture = (Texture2D)image;

        // Instantiate mutated prefab
        Instantiate(UnitPanelTemplate, UnitArrayPool.transform);

        AvailableUnits.Add(unit);
        
    }

    public Unit GetPrefabOfUnit(int position)
    {
        return AvailableUnits[position];
    }

    public void RemoveUnit(int position)
    {
        // Remove from array
        AvailableUnits.RemoveAt(position);
        // and delete GameObject (unit image in panel)
        Destroy(UnitArrayPool.transform.GetChild(position).gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
