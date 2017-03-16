using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public int Type;

    public string Name;
    public Texture2D Image;
    public int Health;
    public int Attack;
    public int AttackRange;
    public int AttackSpeed;

    public bool isActive = false;

    //public GameObject prefab;

    private void Start() {

    }

    private void Update() {
        if (isActive)
            moveUnit();
    }

    private void moveUnit() {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
    }
}
