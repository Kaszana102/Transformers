using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// growing because it contains not empty gameobjects!
/// </summary>
public class GrowingExplosion : Explosion
{
    public float growingTime = 1f;    
    public float shrinkingTime = 2f;


    public AnimationCurve growingCurve;
    protected bool growing;


    protected override void Start()
    {
        base.Start();
        //explode();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (active)
        {
            if (startTime + explodingTime < Time.time)
            {
                //close damaging window
                active = false;
                StartCoroutine(shrink());
            }
        }
    }


    public override void explode()
    {
        base.explode();
        StartCoroutine(grow());
    }

    protected IEnumerator grow()
    {
        float growStartTime = Time.time;
        float scale;
        while (Time.time < growingTime + growStartTime)
        {
            scale = Mathf.Lerp(0, 1, (Time.time - growStartTime) / growingTime);
            scale = growingCurve.Evaluate(scale);
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        transform.localScale = Vector3.one;
    }

    protected IEnumerator shrink()
    {
        float growStartTime = Time.time;
        float scale;
        while (Time.time < shrinkingTime + growStartTime)
        {
            scale = Mathf.Lerp(1, 0, (Time.time - growStartTime) / growingTime);           
            transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }
}
