using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : Shootable
{

    public int magazineMaxSize;
    protected int bulletsNumber;
    public int damage;
    /// <summary>    czas pomiedzy ka¿dym z pocisków. </summary>
    public float fireRate;
    public float regSpeed; // time between creating new bullets
    // Start is called before the first frame update
    public float regDelay;

    protected BulletScript[] bullets;
    public GameObject bulletPrefab;
    public ParticleSystem shootParticles;

    protected float lastShoot, lastGotBullet;
    protected int bulletIndex=0;
    protected bool regDelayPassed;

    /// <summary>    max number of bullets </summary>
    protected int hiddenBullets;


    protected override void Start()
    {
        bulletsNumber = magazineMaxSize;
        lastGotBullet = Time.time;

        hiddenBullets = calcHiddenBulletNumber();

        createBullets(hiddenBullets);        

        if(shootParticles!= null)
        {
            shootParticles.Stop();
        }
        regDelayPassed = true;
    }

    /// <summary>
    /// MS*FR+RD+RS to najkrótszy czas w którym wystrzelimy ponownie pierwszy pocisk w magazynku
    /// jeœli do tego momentu nie zabraknie ukrytych pocisków, to na pewno w innych wystarczy
    /// 
    /// rozwi¹zujemy równanie mag(MS*FR+RD+RS)>=0
    /// gdzie mag(t) to iloœæ pocisków w magazynku po danym czasie
    /// 
    ///          /  X-ceil(LT/FR) , dla t>=LT
    /// mag(t)= |
    ///          \  X-ceil(t/FR) , dla reszty
    ///  gdzie X to rozmiar magazynku
    ///         
    /// 
    /// funkcja ta zwraca minimalne X dla którego nierównoœæ ta jest spe³niona
    /// </summary>
    /// <returns></returns>
    protected  virtual int calcHiddenBulletNumber() {
        float LT = bulletPrefab.GetComponent<BulletScript>().getLifetime();
        float t = magazineMaxSize * fireRate + regDelay + regSpeed;

        /*
        if (t>= LT)
        {
            return Mathf.CeilToInt(LT/fireRate); //upper boundary
        }
        else
        {
            float timeToshootTheSameBullet = magazineMaxSize*fireRate + regDelay + regSpeed;

            return Mathf.CeilToInt(timeToshootTheSameBullet / fireRate);
        } 
          
        */
        return Mathf.CeilToInt(LT / fireRate);

    }



    // Update is called once per frame
    protected override void Update()
    {        
        addAmmo();
    }

    public override void shoot() //wektor wzd³u¿ którego bedzie sie pocisk przemieszcza³
    {
        shoot(transform.forward);
    }

    public override void shoot(Vector3 direction) //wektor wzd³u¿ którego bedzie sie pocisk przemieszcza³
    {        
        
        if (canShoot())
        {
            for (int i = 0; i <= hiddenBullets && bullets[bulletIndex].GetComponent<BulletScript>().getActive() == true; i++)
            {
                bulletIndex++;
                if (bulletIndex >= hiddenBullets)
                {
                    bulletIndex = 0;
                }                
            }
            
            bullets[bulletIndex].setActive(transform.position, direction);            
            lastShoot = Time.time;
            bulletsNumber--;
            regDelayPassed = false;
            if (shootParticles != null)
            {
                shootParticles.Play();
            }
        }         
    }

    public override void shoot(Vector3 direction, Vector3 targetPos)
    {
        if (canShoot())
        {
            for (int i = 0; i <= hiddenBullets && bullets[bulletIndex].GetComponent<BulletScript>().getActive() == true; i++)
            {
                bulletIndex++;
                if (bulletIndex >= hiddenBullets)
                {
                    bulletIndex = 0;
                }
            }

            bullets[bulletIndex].setActive(transform.position, direction,targetPos);
            lastShoot = Time.time;
            bulletsNumber--;
            regDelayPassed = false;
            if (shootParticles != null)
            {
                shootParticles.Play();
            }
        }
    }


    protected virtual void addAmmo()
    {        
        if (regDelayPassed) { 

            if (bulletsNumber < magazineMaxSize)
            {
                if (lastGotBullet + regSpeed < Time.time)
                {
                    bulletsNumber++;
                    lastGotBullet = Time.time;
                }
            }
        }
        else
        {
            //delay after shoot
            if (lastShoot + regDelay < Time.time)
            {
                regDelayPassed = true;
                lastGotBullet = Time.time;
            }
        }
    }
    protected void createBullets(int n)
    {
        bullets = new BulletScript[n];
        for (int i = 0; i < n; i++)
        {
            bullets[i]=Object.Instantiate(bulletPrefab).GetComponent<BulletScript>();            
            bullets[i].transform.position = new Vector3(0, -100, 0);
        }        
    }

    public virtual bool canShoot()
    {
        if (lastShoot + fireRate < Time.time && bulletsNumber > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// return the bullets number in the magazine
    /// </summary>
    /// <returns></returns>
    public int getBulletsNumber()
    {
        return bulletsNumber;
    }


    public string getName()
    {
        return this.gameObject.ToString();
    }
}
