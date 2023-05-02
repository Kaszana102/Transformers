using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : AnyWhereShooter
{
    public float spread; //angle

    protected float spreadRadius;//distance

    // Start is called before the first frame update
    protected override void Start()
    {        
        base.Start();

        //calculate spreadRadius
        spreadRadius = Mathf.Tan(spread/Mathf.PI);
    }

    protected override Vector3 spreadDirection(Vector3 forward)
    {
        Vector3 verticalOffset ;
        Vector3 horizontalOffset;

        verticalOffset = Random.Range(-spreadRadius, spreadRadius) * transform.up;
        horizontalOffset = Random.Range(-spreadRadius, spreadRadius) * transform.right;

        return forward+verticalOffset+horizontalOffset;
    }

}
