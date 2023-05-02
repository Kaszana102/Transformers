using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// it doesn't use most of the weaponScript variables but 
/// it's interface is useful :/
/// </summary>
public class MultiWeaponScript : WeaponScript
{
    protected WeaponScript[] childWeapons;

    // Start is called before the first frame update
    protected override void Start()
    {
        //get all child weapons
        childWeapons = new WeaponScript[transform.childCount];
        for (int i = 0; i < childWeapons.Length; i++)
        {
            childWeapons[i] = transform.GetChild(i).GetComponent<WeaponScript>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
       
    }

    public override void shoot()
    {
        foreach (WeaponScript weapon in childWeapons)
        {
            weapon.shoot();
        }
    }
}
