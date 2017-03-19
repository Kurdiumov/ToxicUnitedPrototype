using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionDetector : MonoBehaviour
{
    private Enemy _enemy;
	// Use this for initialization
	void Start ()
	{
        _enemy = gameObject.GetComponentInParent<Enemy>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.name == "TestUnitObject")
        {
            Unit unit = col.transform.GetComponent<Unit>();
            _enemy.AddUnitInRange(unit);
            Debug.Log("Added");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.name == "TestUnitObject")
        {
            Unit unit = col.transform.GetComponent<Unit>();
            _enemy.RemoveUnitInRange(unit);
            Debug.Log("Removed");
        }
    }
}
