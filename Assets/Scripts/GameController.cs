using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public GameObject UnitArrayPool;
    public GameObject UnitPanelTemplate;
    public float TurnDuration;
    public PlayerController Player;
    public bool RoundRunning;
    private float timeLeft;

    private List<int> _waves;
    private int _currentWave;
    private GameObject _startButton;
    private List<Unit> _unitsInBattlefield;
    private List<Unit> _availableUnits;
    private Text _timerText;

    // Use this for initialization
    void Start()
    {
        _startButton = GameObject.Find("Button").gameObject;
        _timerText = GameObject.Find("TimerText").GetComponent<Text>();
        _timerText.enabled = false;
        // Init empty unit array
        _availableUnits = new List<Unit>();
        _unitsInBattlefield = new List<Unit>();
        // Load units:
        _waves = new List<int>();
        _waves.Add(8);
        _waves.Add(6);
        _waves.Add(7);
        _waves.Add(9);

        _currentWave = 0;
        SpawnUnits(_waves[0]);


        //Setting starting mode of Player
        Player.SwitchControl(PlayerController.Mode.Strategic);
        RoundRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (RoundRunning)
        {
            timeLeft -= Time.deltaTime;
            _timerText.text = "Time Left: " + (int)timeLeft;
            if (timeLeft < 0 || _unitsInBattlefield.Count <= 0)
            {
                EndRound();
            }
        }

    }

    //Start the round
    public void StartRound()
    {
        _timerText.enabled = true;
        if (_unitsInBattlefield.Count <= 0)
            return;

        _startButton.SetActive(false);
        Player.SwitchControl(PlayerController.Mode.FirstPerson);
        RoundRunning = true;
        timeLeft = TurnDuration;

        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            var unit = Unit.GetComponent<Unit>();
            unit.isActive = true;
        }

        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var enemy = Enemy.GetComponent<Enemy>();
            enemy.SetActive(true);
        }
    }


    // End the round
    public void EndRound()
    {
        _timerText.enabled = false;
        foreach (var Unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            var unit = Unit.GetComponent<Unit>();
            unit.isActive = false;
        }

        foreach (var Enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            var enemy = Enemy.GetComponent<Enemy>();
            enemy.SetActive(false);
        }

        RemoveAllUnits();
        _startButton.SetActive(true);
        Player.SwitchControl(PlayerController.Mode.Strategic);
        RoundRunning = false;

        _currentWave++;
        if (_currentWave >= _waves.Count)
        {
            EndGame();
        }
        else
        {
            SpawnUnits(_waves[_currentWave]);
        }
    }

    //Adding list of units, would useful for future
    public void SpawnUnits(int n)
    {
        var Units = GameObject.Find("Units");
        for (int i = 0; i < n; i++)
        {
            //addUnit("TestUnit");
            var rename = (GameObject)Instantiate(Resources.Load("TestUnit"));

            rename.gameObject.GetComponent<Unit>().WeaponType = (Weapon.weaponType)Random.Range(0, 5);
            rename.transform.parent = Units.transform;
            rename.transform.Translate(rename.transform.localPosition.x + 5 * i, rename.transform.localPosition.y, rename.transform.localPosition.z);
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

        _availableUnits.Add(unit);

    }

    public void AddUnitToBattlefield(Unit unit)
    {
        _unitsInBattlefield.Add(unit);
    }

    public void RemoveUnitFromBattlefield(Unit unit)
    {
        _unitsInBattlefield.Remove(unit);
    }

    public Unit GetPrefabOfUnit(int position)
    {
        return _availableUnits[position];
    }

    public void RemoveUnit(int position)
    {
        // Remove from array
        _availableUnits.RemoveAt(position);
        // and delete GameObject (unit image in panel)
        Destroy(UnitArrayPool.transform.GetChild(position).gameObject);
    }


    // Put for future, ending assault
    public void EndGame()
    {
        _currentWave = _waves.Count + 1;
        Debug.Log("You win!!");

    }


    public void RemoveAllUnits()
    {
        Debug.Log("Remoing units...");
        foreach (var unit in GameObject.FindGameObjectsWithTag("Unit"))
        {
            Destroy(unit.gameObject);
        }
        foreach (var unitImage in GameObject.FindGameObjectsWithTag("UnitImage"))
        {
            Destroy(unitImage.gameObject);
        }

        for (int i = 0; i < _availableUnits.Count; i++)
        {
            // Remove from array
            _availableUnits.RemoveAt(i);
            // and delete GameObject (unit image in panel)
            Destroy(UnitArrayPool.transform.GetChild(i).gameObject);
        }
        _unitsInBattlefield.Clear();
        _availableUnits.Clear();


        // UnitArrayPool
        // UnitPanelTemplate;
    }
}
