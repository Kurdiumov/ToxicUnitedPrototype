using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionDetector : MonoBehaviour
{
    private Enemy _enemy;
    private Unit _unit;
	// Use this for initialization
	void Start ()
	{
        _enemy = gameObject.GetComponentInParent<Enemy>();
        _unit = gameObject.GetComponentInParent<Unit>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Unit" && _enemy != null)
        {
            Unit unit = col.transform.GetComponent<Unit>();
            _enemy.AddUnitInRange(unit);
            Debug.Log("Added");
        }
        else if(col.transform.tag == "Enemy" && _unit != null)
        {
            Enemy enemy = col.transform.GetComponent<Enemy>();
            _unit.AddEnemyInRange(enemy);
            Debug.Log("Enemy Added");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Unit" && _enemy != null)
        {
            Unit unit = col.transform.GetComponent<Unit>();
            _enemy.RemoveUnitInRange(unit);
            Debug.Log("Removed");
        }
        else if (col.transform.tag == "Enemy" && _unit != null)
        {
            Enemy enemy = col.transform.GetComponent<Enemy>();
            _unit.RemoveEnemyInRange(enemy);
            Debug.Log("Enemy Removed");
        }
    }
}
