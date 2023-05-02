using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : VehicleScript
{
    

    public CarScript() : base()
    {

    }


    public override void Forward()
    {
        transform.position += transform.forward * movSpeed
            * (sprinting > 0 ? sprintMultiplier : 1)
            * Time.deltaTime;


        if (righting > 0)
        {
            transform.rotation *= Quaternion.Euler(0,
                rotSpeed * Time.deltaTime * (sprinting > 0 ? sprintMultiplier : 1),
                0);
        }

        if (lefting > 0)
        {
            transform.rotation *= Quaternion.Euler(0,
            -rotSpeed * Time.deltaTime * (sprinting > 0 ? sprintMultiplier : 1),
            0);
        }
    }


    public override void Backward()
    {
     
        transform.position -= transform.forward * movSpeed
            * (sprinting>0 ? sprintMultiplier : 1)
            * Time.deltaTime;
        

        if (righting > 0)
        {
            transform.rotation *= Quaternion.Euler(0,
                -rotSpeed * Time.deltaTime * (sprinting > 0 ? sprintMultiplier : 1),
                0);
        }

        if (lefting > 0)
        {
            transform.rotation *= Quaternion.Euler(0,
                rotSpeed * Time.deltaTime * (sprinting > 0 ? sprintMultiplier : 1),
                0);
        }
    }    
}
