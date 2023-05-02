using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    protected bool active = false;
    public int damage = 1;    
    protected GameObject parentGameObject;
    protected ParticleSystem particleSystem;

    public float explodingTime = 1.0f;
    protected float startTime;    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        parentGameObject=this.gameObject;

        if (GetComponent<ParticleSystem>() != null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
        parentGameObject.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (active)
        {
            if(startTime+explodingTime < Time.time)
            {
                //close damaging window
                active = false;                
            }
        }
    }


    /// <summary>
    /// move to certain global position and then explode
    /// </summary>
    /// <param name="pos"></param>
    public void explode(Vector3 pos)
    {
        transform.position=pos;
        explode();
    }

    public virtual void explode()
    {        
        parentGameObject.SetActive(true);
        active = true;        
        startTime = Time.time;
        if (particleSystem != null)
        {
            particleSystem.Play();
        }
    }


    public virtual void explode(Vector3 pos, Vector3 direction)
    {
        explode(pos);
    }



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (active)
        {
            Damagable damagable = other.gameObject.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.getDamage(damage);
            }            
        }
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (active)
        {
            Damagable damagable = other.gameObject.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.getDamage(damage);
            }
        }
    }


    public bool isActive()
    {
        return active;
    }

}
