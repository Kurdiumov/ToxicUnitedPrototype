using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public enum weaponType
    {
        Ak47,
        Shotgun,
        UMP45,
        M4A1,
        Sniper,
    }

    public weaponType WeaponType;
    public string Name;
    public Texture2D Image;
    public int Health;
    public bool isActive = true;

    private  Weapon _weapon;
    private GameObject _weaponPrefab;

    private List<Unit> UnitsInRange;
    private SphereCollider _sphereCollider;

    private Quaternion _localRotation;
    private void Start()
    {
        SpawnWeapon();
        UnitsInRange = new List<Unit>();
        _sphereCollider =  gameObject.GetComponentInChildren<SphereCollider>();
        _sphereCollider.radius = _weapon.AttackRange;
        _localRotation = gameObject.transform.localRotation;
    }

    private void Update()
    {
        if (this.isActive && _weapon.TimeFromLastShoot <= 0 && UnitsInRange.Count > 0 )
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

    private void SpawnWeapon()
    {
        switch (WeaponType)
        {
            case weaponType.Ak47:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/Ak-47"), gameObject.transform);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 3f, _weaponPrefab.transform.localPosition.y + 1, _weaponPrefab.transform.localPosition.z + 4);
                break;
            case weaponType.Shotgun:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/Shotgun"), gameObject.transform);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 2.8f, _weaponPrefab.transform.localPosition.y, _weaponPrefab.transform.localPosition.z + 3.3f);
                break;
            case weaponType.UMP45:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/UMP-45"), gameObject.transform);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 2.5f, _weaponPrefab.transform.localPosition.y - 1, _weaponPrefab.transform.localPosition.z + 2);
                break;
            case weaponType.M4A1:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/M4A1"), gameObject.transform);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x - 3f, _weaponPrefab.transform.localPosition.y, _weaponPrefab.transform.localPosition.z - 4);
                break;
            case weaponType.Sniper:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/SniperRifle"), gameObject.transform);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 3f, _weaponPrefab.transform.localPosition.y + 1, _weaponPrefab.transform.localPosition.z);
                break;
            default:
                throw new ArgumentException();
        }

        this._weapon = _weaponPrefab.GetComponent<Weapon>();
    }

}
