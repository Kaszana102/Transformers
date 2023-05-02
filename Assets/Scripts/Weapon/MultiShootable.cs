using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiShootable : Shootable
{


    protected List<Shootable> childWeapons;

    // Start is called before the first frame update
    protected override void Start()
    {
        //get all child weapons
        childWeapons = new List<Shootable>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childWeapons.Add(transform.GetChild(i).GetComponent<Shootable>());
        }
    }

    // Update is called once per frame
    protected override void Update()
    {

    }

    public override void shoot()
    {
        foreach (Shootable weapon in childWeapons)
        {            
            weapon.shoot();
        }
    }
}
