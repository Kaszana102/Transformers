using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingExplosion : GrowingExplosion
{

    public float speed=1f;
    private Vector3 direction;

    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }


    protected override void Update()
    {
        if (active)
        {

            //transform.position += direction * speed * Time.deltaTime;

            rb.MovePosition(transform.position + direction * speed * Time.deltaTime);


            if (startTime + explodingTime < Time.time)
            {
                //close damaging window
                active = false;
                StartCoroutine(shrink());
            }
        }
    }


    public override void explode(Vector3 pos, Vector3 direction)
    {        
        direction.y = 0;
        this.direction = direction.normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        explode(pos);
    }

}
