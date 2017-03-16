using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour {

    private GameController gameController;

    // Use this for initialization
    void Start () {
        gameController = GameObject.Find("_GameController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UnitClicked() {

        // Obtain index from position at Content panel grid
        int index = this.transform.GetSiblingIndex();

        // Get prefab object of clicked unit
        GameObject unitPrefab = gameController.getPrefabOfUnit(index);

        // Instantiate it at the midlane
        GameObject clone = Instantiate(unitPrefab, new Vector3(0, 10, -230), Quaternion.identity);

        // Set clone as active
        clone.GetComponent<Unit>().isActive = true;

        // ... and remove from unit panel
        gameController.removeUnit(index);
    }
}
