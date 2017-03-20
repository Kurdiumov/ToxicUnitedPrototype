using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Type;

    public string Name;
    public Texture2D Image;
    public int Health;
    public bool isActive = false;
    public Weapon Weapon;
    //public GameObject prefab;

    private List<Unit> UnitsInRange;
    private SphereCollider _sphereCollider;
    private void Start()
    {
        UnitsInRange = new List<Unit>();
        _sphereCollider =  gameObject.GetComponentInChildren<SphereCollider>();
        _sphereCollider.radius = Weapon.AttackRange;
    }

    private void Update()
    {
        if (Weapon.TimeFromLastShoot <= 0 && UnitsInRange.Count > 0 )
        {
            Unit target = ChoseUnitTarget();
            TryAttack(target);
        }
    }


    public void GetDamage(int damage)
    {
        this.Health -= damage;
        if (this.Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddUnitInRange(Unit unit)
    {
        UnitsInRange.Add(unit);
    }

    public void RemoveUnitInRange(Unit unit)
    {
        if(UnitsInRange.Contains(unit))
            UnitsInRange.Remove(unit);
    }

    private Unit ChoseUnitTarget()
    {
        int randomUnit = Random.Range(0, UnitsInRange.Count - 1);
        return UnitsInRange.ElementAt(randomUnit);
    }

    private void TryAttack(Unit unit)
    {
        if (unit == null) //TODO: Fix here.
            return;

        transform.LookAt(unit.transform);
        //TODO: Shooting chanse depends on distance and Weapon accuracy
        if (Weapon.BulletsLeft > 0)
        {
            Weapon.Shoot();
        }
        else
        {
            Weapon.Reload();
            return;
        }
        int randomNumber = Random.Range(0, 3);

        if (randomNumber != 2)  //chanse 2/3
        {
            Debug.Log("Attacking unit " + unit.name);
            unit.GetDamage(Weapon.Attack);
        }
        else
        {
            Debug.Log("miss");
        }
    }



}
