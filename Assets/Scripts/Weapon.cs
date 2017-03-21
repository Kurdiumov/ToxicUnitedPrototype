using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum weaponType
    {
        Ak47,
        Shotgun,
        UMP45,
        M4A1,
        Sniper,
    }


    public AudioSource Sound;
    public float AttackSpeed;
    public int Attack;
    public float AttackRange;
    public float ReloadTime;
    public int BulletCapacity;
    public float Accuracy;

    public int BulletsLeft { get; private set; }
    public float TimeFromLastShoot { get; private set; }
    private bool _reloading;
    private float _reloadTimePassed;


    //Run this in Start method in Unit and Enemy classes
    public static Weapon create (weaponType WeaponType, GameObject _weaponPrefab, Transform unitPosition)
    {
        switch (WeaponType)
        {
            case weaponType.Ak47:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/Ak-47"), unitPosition);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 3f, _weaponPrefab.transform.localPosition.y + 1, _weaponPrefab.transform.localPosition.z + 4);
                break;
            case weaponType.Shotgun:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/Shotgun"), unitPosition);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 2.8f, _weaponPrefab.transform.localPosition.y, _weaponPrefab.transform.localPosition.z + 3.3f);
                break;
            case weaponType.UMP45:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/UMP-45"), unitPosition);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 2.5f, _weaponPrefab.transform.localPosition.y - 1, _weaponPrefab.transform.localPosition.z + 2);
                break;
            case weaponType.M4A1:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/M4A1"), unitPosition);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x - 3f, _weaponPrefab.transform.localPosition.y, _weaponPrefab.transform.localPosition.z - 4);
                break;
            case weaponType.Sniper:
                _weaponPrefab = (GameObject)Instantiate(Resources.Load("Weapons/SniperRifle"), unitPosition);
                _weaponPrefab.transform.Translate(_weaponPrefab.transform.localPosition.x + 3f, _weaponPrefab.transform.localPosition.y + 1, _weaponPrefab.transform.localPosition.z);
                break;
            default:
                throw new ArgumentException();
        }

        return  _weaponPrefab.GetComponent<Weapon>();
    }


    // Use this for initialization
    void Start()
    {
        BulletsLeft = BulletCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        TimeFromLastShoot -= Time.deltaTime;

        if (_reloading)
        {
            _reloadTimePassed -= Time.deltaTime;
            if (_reloadTimePassed <= 0)
            {
                FinishReload();
            }
        }
    }

    public void Shoot()
    {
        if (!_reloading)
        {
            TimeFromLastShoot = AttackSpeed;

            Debug.Log("Shooting");
            Sound.PlayOneShot(Sound.clip);

            BulletsLeft--;
        }
    }

    public void Reload()
    {
        if(!_reloading)
            StartReload();
    }

    private void StartReload()
    {
        _reloading = true;
        _reloadTimePassed = Time.deltaTime + ReloadTime;
        Debug.Log("Reloading");
        //TODO: Reload Sound here
    }

    private void FinishReload()
    {
        _reloading = false;
        BulletsLeft = BulletCapacity;
    }
}
