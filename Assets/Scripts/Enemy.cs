using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Weapon.weaponType WeaponType;
    public string Name;
    public Texture2D Image;
    public int Health;
    public bool IsEnabled;

    private bool _isActive;
    private Weapon _weapon;
    private GameObject _weaponPrefab;

    private List<Unit> UnitsInRange;
    private SphereCollider _sphereCollider;

    private Quaternion _localRotation;

    private void Start()
    {
        this._weapon = Weapon.create(WeaponType, _weaponPrefab, this.gameObject.transform);
        UnitsInRange = new List<Unit>();
        _sphereCollider =  gameObject.GetComponentInChildren<SphereCollider>();
        _sphereCollider.radius = _weapon.AttackRange;
        _localRotation = gameObject.transform.localRotation;
    }

    private void Update()
    {
        if (this.IsEnabled && this._isActive && _weapon.TimeFromLastShoot <= 0 && UnitsInRange.Count > 0 )
        {
            Unit target = ChoseUnitTarget();
            TryAttack(target);
        }
        else if(UnitsInRange.Count == 0)
        {
            transform.localRotation = _localRotation;
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
        int randomUnit = Random.Range(0, UnitsInRange.Count);
        return UnitsInRange.ElementAt(randomUnit);
    }

    private void TryAttack(Unit unit)
    {
        if (unit == null)
        {
            UnitsInRange.Remove(unit);
            return;
        }

        transform.LookAt(unit.transform);
        
        if (_weapon.BulletsLeft > 0)
        {
            _weapon.Shoot();
        }
        else
        {
            _weapon.Reload();
            return;
        }
        int randomNumber = Random.Range(0, 100);

        if (randomNumber <= _weapon.Accuracy) 
        {
            Debug.Log("Attacking unit " + unit.name);
            unit.GetDamage(_weapon.Attack);
        }
        else
        {
            Debug.Log("miss");
        }
    }

    public void SetActive(bool state)
    {
        _isActive = state;
    }
   
}
