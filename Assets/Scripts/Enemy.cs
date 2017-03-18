using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Type;

    public string Name;
    public Texture2D Image;
    public int Health;
    public int Attack;
    public int AttackRange;
    public int AttackSpeed;

    public bool isActive = false;

    //public GameObject prefab;

    private void Start()
    {

    }

    private void Update()
    {

    }


    public void GetDamage(int damage)
    {
        this.Health -= damage;
        if (this.Health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
