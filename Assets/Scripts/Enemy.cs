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
    public int Attack;
    public int AttackRange;
    public int AttackSpeed;
    public float TimeBetweenShoots;
    public bool isActive = false;
    public AudioSource ShootSound;
    //public GameObject prefab;

    private List<Unit> UnitsInRange;
    private float _timeFromLastShoot;
    
    private void Start()
    {
        UnitsInRange = new List<Unit>();
    }

    private void Update()
    {
        _timeFromLastShoot -= Time.deltaTime;
        if (_timeFromLastShoot <= 0 && UnitsInRange.Count > 0 )
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
        //TODO: Shooting chanse depends on distance
        Shoot();
        int randomNumber = Random.Range(0, 3);

        if (randomNumber != 2)  //chanse 2/3
        {
            Debug.Log("Attacking unit " + unit.name);
            unit.GetDamage(this.Attack);
        }
        else
        {
            Debug.Log("miss");
        }
    }


    private void Shoot()
    {
        //TODO: Ammo here
        _timeFromLastShoot = TimeBetweenShoots;
        Debug.Log("Shooting");
        ShootSound.PlayOneShot(ShootSound.clip);
    }
}
