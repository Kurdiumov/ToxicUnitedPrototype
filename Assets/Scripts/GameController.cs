using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject UnitArrayPool;

    private List<Unit> AvailableUnits;

	// Use this for initialization
	void Start () {
        AvailableUnits = new List<Unit>();
        
    }

    public void addUnit(Unit unit) {
        //UnitArrayPool.
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
