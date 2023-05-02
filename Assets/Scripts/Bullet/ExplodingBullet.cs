using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingBullet : BulletScript
{
    public Explosion explosion;    


    protected override void Start()
    {
        base.Start();
        if (explosion != null)
        {
            explosion = GameObject.Instantiate(explosion);
        }
    }


    protected override void explode()
    {        
        Vector3 explodePos = transform.position;
        base.explode();
        if (explosion != null)
        {
            explosion.explode(explodePos, direction);
        }
    }    
}
