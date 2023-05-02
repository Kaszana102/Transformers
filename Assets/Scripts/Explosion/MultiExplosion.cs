using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// It can't have main particle system because
/// it triggers the childrens particle systems.
/// At least, as far as I know.
/// </summary>
public class MultiExplosion : Explosion
{
    public List<Explosion> explosions; //n explosion
    public List<float> explosionDelays; //n delays!

    protected int explosionIndex;    

    // Start is called before the first frame update
    protected override void Start()
    {
        parentGameObject = this.gameObject;                
        parentGameObject.SetActive(false);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (active)
        {
            if(Time.time > explosionDelays[explosionIndex] + startTime)
            {
                explosions[explosionIndex].explode();
                explosionIndex++;
                if(explosionIndex >= explosionDelays.Count)
                {
                    active = false;
                    explosionIndex = 0;
                }
            }            
        }
    }

    public override void explode()
    {
        parentGameObject.SetActive(true);
        active = true;        
        startTime = Time.time;
    }
}
