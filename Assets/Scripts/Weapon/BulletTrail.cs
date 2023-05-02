using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    GameObject bullet;
    protected bool moving = false;

    ParticleSystem trailParticleSystem;
    float trailLifeTimeAfterHit;
    // Start is called before the first frame update
    void Start()
    {
        trailParticleSystem = GetComponent<ParticleSystem>();
        trailLifeTimeAfterHit = trailParticleSystem.startLifetime;

        hide();
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {            
            keepTrailInPlace();
        }
    }


    protected void keepTrailInPlace()
    {
        transform.position = bullet.transform.position;
        transform.rotation = bullet.transform.rotation;        
    }


    public void setActive()
    {
        gameObject.SetActive(true);
        trailParticleSystem.Play();
        moving = true;
    }

    protected IEnumerator hideTrail()
    {
        float startTime = Time.time;        
        while (Time.time < startTime + trailLifeTimeAfterHit)
        {
            yield return null;
        }

        gameObject.SetActive(false);

    }


    public void hide()
    {
        trailParticleSystem.Clear();
        gameObject.SetActive(false);
        moving = false;
    }

    public void setInactive()
    {
        trailParticleSystem.Stop();
        moving = false;              
        StartCoroutine(hideTrail());
    }


    public void setBullet(GameObject bullet)
    {
        this.bullet = bullet;
    }
}
