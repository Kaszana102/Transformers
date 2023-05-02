using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum ALIGNMENT
{
    GOOD,
    EVIL
}

public class TransformerScript : MonoBehaviour
{
    bool robotMode = true;
    ALIGNMENT aligment = ALIGNMENT.GOOD;

    protected VehicleScript vehicleScript = null;

    VehicleType vehicleType = VehicleType.NONE;

    public GameObject model;
    public Animator animator;
    public Shootable weapon;


    protected bool attackMode = false;
    protected float speed = 5;

    protected bool running =false;


    protected bool aiming = false;
    float aimingTime = 1f;
    float lastShot = -1;

    // Start is called before the first frame update
    void Start()
    {
        vehicleScript =  this.gameObject.AddComponent<VehicleScript>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        if (robotMode)
        {
            //move like a robot
            RobotAction();
            if(lastShot > 0)
            {
                if(Time.time > lastShot + aimingTime)
                {
                    SetAim(false);
                    lastShot = -1;
                }
            }

        }
        else
        {
            //move like an appriopriate vehicle
            VehicleAction();
        }
    }


    /// <summary>
    /// return true if can transform
    /// </summary>
    /// <returns></returns>
    protected bool Transform()
    {
        if (robotMode && vehicleType != VehicleType.NONE)
        {
            TransformToVehicle();
            robotMode = !robotMode;
            return true;
        }
        else if(!robotMode)
        {
            TransformToRobot();
            robotMode = !robotMode;
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// AI LOGIC for actions
    /// </summary>
    protected virtual void RobotAction()
    {

    }

    protected void SetWalking(bool state)
    {
        animator.SetBool("Walk", state);
    }

    protected void SetRunning(bool state)
    {
        running = state;
        animator.SetBool("Run", state);        
    }




    protected void Shoot(Vector3 targetPos)
    {
        if (!aiming) SetAim(true);
        weapon.shoot(targetPos - weapon.transform.position, targetPos);
        lastShot = Time.time;
    }

    protected void SetAim(bool state)
    {
        aiming = state;
        animator.SetBool("Aim", state);
        if (state)
        {
            lastShot = -1;
        }
    }



    /// <summary>
    /// x-right
    /// y-forward
    /// </summary>
    /// <param name="dir"></param>
    protected void WalkTowards(Vector2 dir)
    {

        if (running) dir *= 4;

        transform.position += (transform.forward*dir.y + transform.right*dir.x) * Time.deltaTime * speed;


        //rotate model
        float roty = -Vector2.SignedAngle(new Vector2(0,1), dir);
        Quaternion from = model.transform.localRotation;
        Quaternion to = Quaternion.Euler(0,roty,0);
        model.transform.localRotation = Quaternion.Lerp(from, to, 0.1f);
    }


    /// <summary>
    /// AI LOGIC for actions
    /// </summary>
    protected virtual void VehicleAction()
    {

    }

    private void TransformToRobot()
    {
        switch (vehicleType)
        {
            case VehicleType.FASTCAR:
                animator.SetBool("TransformFromFastCar",true);
                break;
        }
    }

    private void TransformToVehicle()
    {
        switch (vehicleType)
        {
            case VehicleType.FASTCAR:
                animator.SetBool("TransformToFastCar", true);
                break;
        }
    }

    protected void ChangeVehicle(VehicleType type)
    {

        if (type != vehicleType && type!= VehicleType.NONE)
        {            

            vehicleType = type;
            switch (vehicleType)
            {
                case VehicleType.FASTCAR:
                    animator.SetBool("ToFastCar", true);
                    Destroy(vehicleScript);
                    vehicleScript = VehicleScript.GetVehicleScriptByType(this.gameObject, vehicleType);                    
                    break;
            }
        }

    }
  
    /// <summary>
    /// sets animator parameters for aim animations
    /// </summary>
    /// <param name="dir"></param>
    protected void SetAimDirection(Vector2 dir)
    {
        if (dir.x < 0)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }
        else if(dir.x == 0)
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
        else
        {
            animator.SetBool("Left", false);
            animator.SetBool("Right", true);
        }


        if (dir.y > 0)
        {
            animator.SetBool("Forward", true);
            animator.SetBool("Backward", false);
        }
        else if (dir.y == 0)
        {
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", false);
        }
        else
        {
            animator.SetBool("Forward", false);
            animator.SetBool("Backward", true);
        }
    }

}
