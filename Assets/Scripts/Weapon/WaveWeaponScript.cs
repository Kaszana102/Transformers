using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// weapon which all of child weapons, with offset time between each of them
/// </summary>
public class WaveWeaponScript : MultiWeaponScript
{
    public float delay = 1f;    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public override void shoot()
    {
        StartCoroutine(waveShoot());
    }


    IEnumerator waveShoot()
    {
        foreach (WeaponScript weapon in childWeapons)
        {
            weapon.shoot();
            lastShoot = Time.time;
            while (Time.time < lastShoot + delay)
            {
                yield return null;
            }
        }
    }


}
