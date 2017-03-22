using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject UnitArrayPool;

    public GameObject UnitPanelTemplate;

    private List<Unit> AvailableUnits;

    public PlayerController player;

    public float turnDuration;
    private float timeLeft;

    public static bool roundRunning = false;


    public List<int> waves;
    int currentWave;

    private GameObject _startButton;

    private List<Unit> UnitsInBattlefield;

    // Use this for initialization
    void Start()
    {
        _startButton = GameObject.Find("Button").gameObject;
        
        // Init empty unit array
        AvailableUnits = new List<Unit>();
        UnitsInBattlefield = new List<Unit>();
        // Load units:
        waves = new List<int>();
        waves.Add(4);
        waves.Add(4);
        waves.Add(5);
        waves.Add(4);

        currentWave = 0;
        SpawnUnits(waves[0]);


        //Setting starting mode of player
        player.SwitchControl(PlayerController.Mode.Strategic);
        roundRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (roundRunning)
        {
            timeLeft -= Time.deltaTime;
            Debug.Log(timeLeft);
            if (timeLeft < 0 || UnitsInBattlefield.Count <= 0)
            {
                EndRound();
            }
        }

    }

    //Start the round
    public void StartRound()
    {
        if (UnitsInBattlefield.Count <= 0)
            return;

        _startButton.SetActive(false);
        player.SwitchControl(PlayerController.Mode.FirstPerson);
        roundRunning = true;
        timeLeft = turnDuration;

        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            var unit = Unit.GetComponent<Unit>();
            unit.isActive = true;
        }

        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var enemy = Enemy.GetComponent<Enemy>();
            enemy.isActive = true;
        }
    }


    // End the round
    public void EndRound()
    {
        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            var unit = Unit.GetComponent<Unit>();
            unit.isActive = false;
        }

        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var enemy = Enemy.GetComponent<Enemy>();
            enemy.isActive = false;
        }

        _startButton.SetActive(true);
        player.SwitchControl(PlayerController.Mode.Strategic);
        roundRunning = false;

        currentWave++;
        if (currentWave >= waves.Count)
        {
            EndGame();
        }
        else
        {
            SpawnUnits(waves[currentWave]);
        }
    }

    //Adding list of units, would useful for future
    public void SpawnUnits(int n)
    {
        var Units = GameObject.Find("Units");
        for (int i = 0; i < n; i++)
        {
            //addUnit("TestUnit");
            var rename =  (GameObject)Instantiate(Resources.Load("TestUnit"));
            
            rename.transform.parent = Units.transform;
            rename.transform.Translate(rename.transform.localPosition.x + 5*i, rename.transform.localPosition.y, rename.transform.localPosition.z );
        }

        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            var unit = Unit.GetComponent<Unit>();
            unit.isActive = false;
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

    public void AddUnitToBattlefield(Unit unit)
    {
        UnitsInBattlefield.Add(unit);
    }

    public void RemoveUnitFromBattlefield(Unit unit)
    {
        UnitsInBattlefield.Remove(unit);
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


    // Put for future, ending assault
    public void EndGame()
    {

    }


}
