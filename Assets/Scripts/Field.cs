using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public bool isSpawn = false;
    public bool isObstacle = false;
    public bool isTarget = false;
    public bool isFinalTarget = false;

    public double movespeedFactor = 0.7;

    public bool isCorrectMove = false;

    private FieldController fieldController;

    // Use this for initialization
    void Start()
    {
        fieldController = GameObject.Find("_GameController").GetComponent<FieldController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
