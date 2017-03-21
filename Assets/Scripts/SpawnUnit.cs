using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour {

    private GameController gameController;
    private FieldController fieldController;

    private List<GameObject> route;

    private bool waiting;
    private int unitIndex;
    private GameObject unitPrefab;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("_GameController").GetComponent<GameController>();
        fieldController = GameObject.Find("_GameController").GetComponent<FieldController>();

        route = new List<GameObject>();
        waiting = false;
        unitIndex = 0;
        unitPrefab = null;
    }
	
	// Update is called once per frame
	void Update () {
        if (waiting) {

            route = fieldController.getRoute();

            if (route.Count > 1) {

                // Instantiate it at the beginning of route
                GameObject clone = Instantiate(unitPrefab, route[0].gameObject.transform.position, Quaternion.identity);

                // Set route
                foreach (GameObject node in route) {
                    clone.GetComponent<Unit>().route.Add(node.transform.position);
                }

                // Set clone as active
                clone.GetComponent<Unit>().isActive = true;

                // ... and remove from unit panel
                gameController.removeUnit(unitIndex);
                waiting = false;
            }
        }
	}

    public void UnitClicked() {

        // Obtain index from position at Content panel grid
        unitIndex = this.transform.GetSiblingIndex();

        // Get prefab object of clicked unit
        unitPrefab = gameController.getPrefabOfUnit(unitIndex);

        Debug.Log("Start route selection");
        // Select route (delegated to FieldController)
        StartCoroutine(fieldController.selectRoute());

        Debug.Log("Wait for route");
        Debug.Log(route.Count);

        waiting = true;
    }
}
