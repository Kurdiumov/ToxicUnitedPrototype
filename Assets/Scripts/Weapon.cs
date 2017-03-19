using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AudioSource Sound;
    public float AttackSpeed;
    public int Attack;
    public float AttackRange;
    public float ReloadTime;
    public int BulletCapacity;


    public int BulletsLeft { get; private set; }
    public float TimeFromLastShoot { get; private set; }
    private bool _reloading;
    private float _reloadTimePassed;

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
        //TODO: Ammo here
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
