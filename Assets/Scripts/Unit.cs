using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int Type;

    public string Name;
    public Texture2D Image;
    public int Health;
    public int Attack;
    public int AttackRange;
    public int AttackSpeed;
    public int Speed;
    public Weapon.weaponType WeaponType;

    public bool isActive = false;

    public List<Vector3> route;
    private int activeNode = 1;

    private Weapon _weapon;
    private GameObject _weaponPrefab;

    private void Start()
    {
        this._weapon = Weapon.create(WeaponType, _weaponPrefab, this.gameObject.transform);
        if (Speed == 0)
            Speed = 10;
        if (Health == 0)
            Health = 10;
    }

    private void Awake()
    {
        route = new List<Vector3>();
    }

    private void Update()
    {
        if (isActive && activeNode < route.Count)
            moveUnit();

    }

    private void moveUnit()
    {
        this.transform.Translate(Vector3.Normalize(route[activeNode] - this.transform.position) * Time.deltaTime * Speed);
        if (Vector3.Distance(this.gameObject.transform.position, route[activeNode]) < 10)
            activeNode++;
    }

    public void GetDamage(int damage)
    {
        this.Health -= damage;
        Debug.Log("Getting damage " + damage + " HP: " + Health);
        if (this.Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
