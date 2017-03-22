using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

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
    private List<Enemy> EnemiesInRange;

    public Field field;

    private void Start()
    {
        EnemiesInRange = new List<Enemy>();
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
        {
            RaycastHit hit = new RaycastHit();
            Vector3 position = this.transform.position;
            Ray ray = new Ray(position, Vector3.down*10);
            Debug.DrawRay(position, Vector3.down * 10, Color.cyan, 20);
            int gridLayer = LayerMask.GetMask("Tiles");
            if (Physics.Raycast(ray, out hit, 100.0f, gridLayer))
            {
                GameObject actualField = hit.transform.gameObject;
                this.field = actualField.GetComponent<Field>();
            }
            moveUnit();
        }

        if (this.isActive && _weapon.TimeFromLastShoot <= 0 && EnemiesInRange.Count > 0)
        {
            Enemy target = ChoseEnemyTarget();
            TryAttack(target);
        }
    }

    private void moveUnit()
    {
        this.transform.Translate(Vector3.Normalize(route[activeNode] - this.transform.position) * Time.deltaTime * Speed * (float)field.movespeedFactor);
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


    public void AddEnemyInRange(Enemy enemy)
    {
        EnemiesInRange.Add(enemy);
    }

    public void RemoveEnemyInRange(Enemy enemy)
    {
        if (EnemiesInRange.Contains(enemy))
            EnemiesInRange.Remove(enemy);
    }

    private Enemy ChoseEnemyTarget()
    {
        int randomEnemy = Random.Range(0, EnemiesInRange.Count);
        return EnemiesInRange.ElementAt(randomEnemy);
    }

    private void TryAttack(Enemy enemy)
    {
        if (enemy == null)
        {
            EnemiesInRange.Remove(enemy);
            return;
        }

        // transform.LookAt(enemy.transform);

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
            Debug.Log("Attacking enemy " + enemy.name);
            enemy.GetDamage(_weapon.Attack);
        }
        else
        {
            Debug.Log("miss");
        }
    }
}
