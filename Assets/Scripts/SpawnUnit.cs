using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{

    private GameController gameController;
    private FieldController fieldController;

    private List<GameObject> route;

    private bool waiting;
    private int unitIndex;
    private Unit _unit;

    // Use this for initialization
    void Start()
    {
        gameController = GameObject.Find("_GameController").GetComponent<GameController>();
        fieldController = GameObject.Find("_GameController").GetComponent<FieldController>();

        route = new List<GameObject>();
        waiting = false;
        unitIndex = 0;
        _unit = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            route = fieldController.getRoute();

            if (route.Count > 1)
            {
                _unit.transform.position = route[0].gameObject.transform.position;
                _unit.transform.position += new Vector3(0.0f, 5f, 0.0f);

                // Set route
                foreach (GameObject node in route)
                {
                    _unit.GetComponent<Unit>().route.Add(node.transform.position);
                }

                // ... and remove from unit panel
                gameController.RemoveUnit(unitIndex);
                waiting = false;
                gameController.AddUnitToBattlefield(_unit.GetComponent<Unit>());
            }
        }
    }

    public void UnitClicked()
    {

        // Obtain index from position at Content panel grid
        unitIndex = this.transform.GetSiblingIndex();

        // Get prefab object of clicked unit
        _unit = gameController.GetPrefabOfUnit(unitIndex);

        Debug.Log("Start route selection");
        // Select route (delegated to FieldController)
        StartCoroutine(fieldController.selectRoute());

        Debug.Log("Wait for route");
        Debug.Log(route.Count);

        waiting = true;
    }
}
