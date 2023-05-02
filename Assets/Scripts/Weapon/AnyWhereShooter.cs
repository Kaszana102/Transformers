using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyWhereShooter : WeaponScript
{
    public int pelletsNumber;
    protected override void Start()
    {        
        base.Start();        
    }

    protected override int calcHiddenBulletNumber()
    {
        return base.calcHiddenBulletNumber()*pelletsNumber;
    }


    public override void shoot(Vector3 direction) //wektor wzd³u¿ którego bedzie sie pocisk przemieszcza³
    {

        if (canShoot())
        {            
            for (int pellet = 0; pellet < pelletsNumber; pellet++)
            {
                for (int i = 0; i <= hiddenBullets && bullets[bulletIndex].GetComponent<BulletScript>().getActive() == true; i++)
                {
                    bulletIndex++;
                    if (bulletIndex >= hiddenBullets)
                    {
                        bulletIndex = 0;
                    }
                }

                direction = spreadDirection(direction);


                bullets[bulletIndex].GetComponent<BulletScript>().setActive(transform.position, direction);
            }
            regDelayPassed = false;
            lastShoot = Time.time;
            bulletsNumber--;
        }
    }



    protected virtual Vector3 spreadDirection(Vector3 forward)
    {        

        return randVector();
    }

    private Vector3 randVector()
    {
        float x = Random.value * 2 - 1;
        float y = Random.value * 2 - 1;
        float z = Random.value * 2 - 1;
        Vector3 otp = new Vector3(x, y, z);        
        return otp.normalized;
    }

}

