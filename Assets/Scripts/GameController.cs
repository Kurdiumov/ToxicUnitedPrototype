﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject UnitArrayPool;

    public GameObject UnitPanelTemplate;

    private List<GameObject> AvailableUnits;

    public PlayerController player;
    
    
    public float turnDuration = 6.0f;
    private float timeLeft;

    public static bool roundRunning = false;


    public List<int> waves;
    int currentWave;
    

	// Use this for initialization
	void Start () {
        // Init empty unit array
        AvailableUnits = new List<GameObject>();

        // Load units:
        waves = new List<int>();
        waves.Add(4);
        waves.Add(4);
        waves.Add(5);
        waves.Add(4);
        currentWave = 0;
        spawnUnits(waves[0]);

        //Setting starting mode of player
        player.SwitchControl(PlayerController.Mode.Strategic);
        roundRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if( roundRunning)
        {
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);
            if ( timeLeft < 0)
            {
                endRound();
            }
        }
    }

    //Start the round
    public void startRound()
    {
        player.SwitchControl(PlayerController.Mode.FirstPerson);
        roundRunning = true;
        timeLeft = turnDuration;
    }


    // End the round
    public void endRound()
    {
        player.SwitchControl(PlayerController.Mode.Strategic);
        roundRunning = false;

        currentWave++;
        if(currentWave >= waves.Count)
        {
            endGame();
        }
        else
        {
            spawnUnits(waves[currentWave]);
        }
    }

    //Adding list of units, would useful for future
    public void spawnUnits(int n)
    {
        for(int i =0; i < n; i++)
        {
            addUnit("TestUnit");
        }
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

        AvailableUnits.Add(unitPrefab);
    }

    public GameObject getPrefabOfUnit(int position) {
        return AvailableUnits[position];
    }

    public void removeUnit(int position) {
        // Remove from array
        AvailableUnits.RemoveAt(position);
        // and delete GameObject (unit image in panel)
        Destroy(UnitArrayPool.transform.GetChild(position).gameObject);
    }


    // Put for future, ending assault
    public void endGame()
    {

    }
	
	
}
