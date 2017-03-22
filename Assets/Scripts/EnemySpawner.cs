using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool HasSpecifiedWeapon;
    public Weapon.weaponType WeaponType;
    public float TimeBetweenSpawn;
    public bool IsEnabled;

    private Enemy _enemyInstance;
    private float _timeLeft;
    private bool _startTimer;
    private GameObject _Enemies;
    // Use this for initialization
    void Start()
    {
        _Enemies = GameObject.Find("Enemies");
        _startTimer = false;
        if (IsEnabled)
            SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsEnabled && _enemyInstance == null)
        {
            if (_startTimer)
                _timeLeft = TimeBetweenSpawn;

            _startTimer = false;
            _timeLeft -= Time.deltaTime;
            if (_timeLeft <= 0)
            {
                _startTimer = true;
                SpawnEnemy();
                
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (IsEnabled && col.transform.tag == "Enemy" )
        {
            _enemyInstance = col.GetComponent<Enemy>();
        }
    }

    void SpawnEnemy()
    {
        var enemy = (GameObject)Instantiate(Resources.Load("Enemy"), gameObject.transform);
        
        enemy.transform.parent = _Enemies.transform;
        _enemyInstance = enemy.gameObject.GetComponent<Enemy>();

        if(HasSpecifiedWeapon)
            _enemyInstance.WeaponType = WeaponType;
        else
            _enemyInstance.WeaponType = (Weapon.weaponType)Random.Range(0, 5);
        
        _enemyInstance.IsEnabled = true;
    }
}
