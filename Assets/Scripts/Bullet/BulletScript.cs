using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    protected bool active;

    /// <summary>
    /// used only for activation XD
    /// </summary>    
    protected GameObject parentGameObject;


    /// <summary>
    /// speed vector , where the bullet is goind
    /// </summary>
    protected Vector3 direction;
    public float speed=10;
    public int damage = 1;
    public float lifetime = 3f;
    protected float startTime;
    protected Rigidbody rigidbody;


    //growing variables
    public float growingTime = 1f;
    public float finalScale = 1f;
    protected Vector3 additionalScale;
    protected bool scaling = false;
    protected bool doesntEverGrow;
    
    public float lifeTimeAfterHit = 0.1f;

    public GameObject hitParticlesSystemPrefab;
    protected GameObject hitParticlesSystemObject;
    protected ParticleSystem hitParticlesSystem;

    public GameObject trailPrefab;
    protected BulletTrail trail;    
    

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        parentGameObject = this.gameObject;

        rigidbody = GetComponent<Rigidbody>();

        //calcualte scale growth rate
        if (growingTime > 0) { 
            additionalScale = new Vector3(finalScale, finalScale, finalScale) / growingTime;
            doesntEverGrow = false;
        }
        else
        {
            doesntEverGrow = true;            
        }

        setupParticles();        

        hide();
    }

    protected virtual void setupParticles()
    {
        if (hitParticlesSystemPrefab != null)
        {
            hitParticlesSystemObject = GameObject.Instantiate(hitParticlesSystemPrefab);
            hitParticlesSystem = hitParticlesSystemObject.GetComponent<ParticleSystem>();
        }

        if(trailPrefab != null)
        {
            trail = GameObject.Instantiate(trailPrefab).GetComponent<BulletTrail>();
            trail.setBullet(gameObject);
        }
    }


    // Update is called once per frame
    protected virtual void Update()
    {        
        if (active)
        {
            transform.position+=direction*speed * Time.deltaTime;
            

            if (startTime + lifetime < Time.time)
            {
                explode();
            }
            scale();

        }        
    }

    public virtual void setActive(Vector3 startingPos, Vector3 direction)
    {
        parentGameObject.SetActive(true);
        if (hitParticlesSystem != null)
        {
            hitParticlesSystem.Stop();
        }

        

        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;

        transform.position=startingPos;        
        transform.rotation=Quaternion.LookRotation(direction,new Vector3(0,1,0));
        this.direction = direction.normalized;
        active = true;
        startTime = Time.time;

        //ready scaling
        transform.localScale = new Vector3(0, 0, 0);
        scaling = true;


        if (trail != null)
        {
            trail.setActive();           
        }
        
    }

    public virtual void setActive(Vector3 startingPos, Vector3 direction, Vector3 targetPos)
    {
        setActive(startingPos,direction);
    }

    public bool getActive()
    {
        return active;
    }

    public void hide()
    {
        transform.position = new Vector3(0, -100, 0);

        if (hitParticlesSystem != null)
        {
            hitParticlesSystem.Clear();
            hitParticlesSystemObject.SetActive(false);
        }

        parentGameObject.SetActive(false);
    }
    

    protected void inactivateEverythingNeeded()
    {
        if (trail != null)
        {
            trail.setInactive();            
        }


        active = false;
        transform.position = new Vector3(0, -100, 0);                

        parentGameObject.SetActive(false);
    }

    protected IEnumerator delayedSetInactive()
    {
        float startTime = Time.time;
        while (Time.time < startTime + lifeTimeAfterHit)
        {
            yield return null;
        }

        inactivateEverythingNeeded();
    }

    public void setInactive()
    {
        if (lifeTimeAfterHit <= 0f)
        {
            inactivateEverythingNeeded();
        }
        else
        {
            StartCoroutine(delayedSetInactive());
        }
    }

    public void setDamage(int damage)
    {
        this.damage = damage;
    }    

    public float getLifetime()
    {
        return lifetime;
    }


    protected void OnCollisionEnter(Collision other)
    {        
        Damagable damagable = other.gameObject.GetComponent<Damagable>();
        if (damagable != null)
        {
            damagable.getDamage(damage);
        }        
        explode();        
    }


    protected virtual void explode()
    {                
        //create BOOM        
        if (!(hitParticlesSystem is null))
        {
            hitParticlesSystemObject.SetActive(true);
            hitParticlesSystem.transform.position=transform.position;
            hitParticlesSystem.Play();                        
        }        

        setInactive();        
    }    


    protected void scale()
    {
        if (scaling)
        {
            if (!doesntEverGrow)
            {
                transform.localScale += additionalScale * Time.deltaTime;
                if (transform.localScale.x >= finalScale)
                {
                    scaling = false;
                    transform.localScale = new Vector3(finalScale, finalScale, finalScale);
                }
            }
            else
            {
                scaling = false;
                transform.localScale = new Vector3(finalScale, finalScale, finalScale);
            }
        }       
    }

}
