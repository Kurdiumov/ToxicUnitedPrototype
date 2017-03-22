using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldController : MonoBehaviour
{

    public Material defaultFieldMaterial;
    public Material activeFieldMaterial;
    public Material targetFieldMaterial;

    private List<GameObject> fields;

    private bool highlightSpawns = false;
    private List<GameObject> spawns;

    private bool highlightTargets = false;
    private List<GameObject> targets;

    private List<GameObject> route;
    private List<GameObject> tempRoute;

    // Use this for initialization
    void Awake()
    {
        fields = new List<GameObject>();

        // Load all fields to 1D array
        for (int j = 1; j <= 12; j++)
        {
            for (int i = 0; i < 17; i++)
            {
                GameObject row = GameObject.Find("Row (" + j + ")");
                fields.Add(row.transform.GetChild(i).gameObject);
            }
        }

        // Load fields into helper lists
        spawns = new List<GameObject>();
        foreach (GameObject field in fields)
        {
            if (field.GetComponent<Field>().isSpawn)
            {
                spawns.Add(field);
            }
        }

        targets = new List<GameObject>();
        foreach (GameObject field in fields)
        {
            if (field.GetComponent<Field>().isTarget)
            {
                targets.Add(field);
            }
        }

        route = new List<GameObject>();
        tempRoute = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // While last added field is NOT a target
        if (tempRoute.Count == 0 || !tempRoute[tempRoute.Count - 1].GetComponent<Field>().isTarget)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    GameObject clickedField = hit.transform.gameObject;
                    if (clickedField.gameObject.tag == "Field" && clickedField.GetComponent<Field>().isCorrectMove)
                    {
                        // Add field to the route
                        tempRoute.Add(clickedField);

                        // If spawn then turn off spawn highlight
                        if (clickedField.GetComponent<Field>().isSpawn)
                        {
                            toggleHighlightSpawn();
                        }

                        // If target then turn off target highlight
                        if (clickedField.GetComponent<Field>().isTarget)
                        {
                            toggleHighlightTarget();
                        }

                        // Find and mark next available spots, remove prev
                        clickedField.GetComponent<Field>().isCorrectMove = false;
                        if (tempRoute.Count > 1)
                        {
                            markNextMoves(clickedField, tempRoute[tempRoute.Count - 2]);
                        }
                        else
                        {
                            markNextMoves(clickedField, clickedField);
                        }
                    }
                }
            }
        }
        else
        {
            route = tempRoute;
        }
    }

    private void markNextMoves(GameObject currField, GameObject prevField)
    {
        // Hide prev moves
        int index = 0;
        foreach (GameObject field in fields)
        {
            if (field.Equals(prevField))
            {
                break;
            }
            index++;
        }
        Debug.Log(index);

        if (index % 17 != 16 && !fields[index + 1].GetComponent<Field>().isObstacle)
            markField(fields[index + 1], false);
        if (index % 17 != 0 && !fields[index - 1].GetComponent<Field>().isObstacle)
            markField(fields[index - 1], false);
        if (index < 186 && !fields[index + 17].GetComponent<Field>().isObstacle)
            markField(fields[index + 17], false);
        if (index > 16 && !fields[index - 17].GetComponent<Field>().isObstacle)
            markField(fields[index - 17], false);

        // Mark new moves
        if (!currField.GetComponent<Field>().isTarget)
        {
            index = 0;
            foreach (GameObject field in fields)
            {
                if (field.Equals(currField))
                {
                    break;
                }
                index++;
            }

            if (index % 17 != 16 && !fields[index + 1].GetComponent<Field>().isObstacle)
                markField(fields[index + 1], true);
            if (index % 17 != 0 && !fields[index - 1].GetComponent<Field>().isObstacle)
                markField(fields[index - 1], true);
            if (index < 186 && !fields[index + 17].GetComponent<Field>().isObstacle)
                markField(fields[index + 17], true);
            if (index > 16 && !fields[index - 17].GetComponent<Field>().isObstacle)
                markField(fields[index - 17], true);
        }
    }

    private void markField(GameObject field, bool status)
    {
        if (status == false)
        {
            field.GetComponent<MeshRenderer>().material = defaultFieldMaterial;
            field.GetComponent<Field>().isCorrectMove = false;
        }
        else
        {
            field.GetComponent<MeshRenderer>().material = activeFieldMaterial;
            field.GetComponent<Field>().isCorrectMove = true;
        }
    }

    public void toggleHighlightSpawn()
    {
        highlightSpawns = !highlightSpawns;
        foreach (GameObject spawn in spawns)
        {
            if (highlightSpawns)
            {
                spawn.GetComponent<MeshRenderer>().material = activeFieldMaterial;
                spawn.GetComponent<Field>().isCorrectMove = true;
            }
            else
            {
                spawn.GetComponent<MeshRenderer>().material = defaultFieldMaterial;
                spawn.GetComponent<Field>().isCorrectMove = false;
            }
        }

    }

    public void toggleHighlightTarget()
    {
        highlightTargets = !highlightTargets;
        foreach (GameObject target in targets)
        {
            if (highlightTargets)
            {
                target.GetComponent<MeshRenderer>().material = targetFieldMaterial;
                target.GetComponent<Field>().isCorrectMove = true;
            }
            else
            {
                target.GetComponent<MeshRenderer>().material = defaultFieldMaterial;
                target.GetComponent<Field>().isCorrectMove = false;
            }
        }

    }

    public IEnumerator selectRoute()
    {
        Debug.Log("Start coroutine");
        // Init empty route
        tempRoute = new List<GameObject>();

        // Clear route
        route = new List<GameObject>();

        Debug.Log("Show spawns");
        // Show available spawns
        toggleHighlightSpawn();

        // Show available targets
        toggleHighlightTarget();

        yield break;


    }

    public List<GameObject> getRoute()
    {
        return route;
    }
}
