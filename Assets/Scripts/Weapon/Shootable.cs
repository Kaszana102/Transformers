using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shootable : MonoBehaviour
{
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {

    }

    public virtual void shoot() //wektor wzd³u¿ którego bedzie sie pocisk przemieszcza³
    {        
    }

    public virtual void shoot(Vector3 direction) //wektor wzd³u¿ którego bedzie sie pocisk przemieszcza³
    {
    }

    public virtual void shoot(Vector3 direction, Vector3 targetPos)
    {
    }
}
