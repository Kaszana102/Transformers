using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;


public enum VehicleType
{
    NONE,
    FASTCAR    
}


public class VehicleScript : MonoBehaviour
{

    public VehicleType type;

    public float movSpeed = 1f;
    public float rotSpeed = 30f;

    public float sprintMultiplier = 3f;

    protected int lefting = 0, righting = 0, sprinting =0;

    public VehicleScript() : base()
    {        
     
    }

    private void Update()
    {
        if (righting > 0) righting--;
        if (lefting > 0) lefting--; 
        if (sprinting > 0) sprinting--;
    }

    virtual public void Forward()
    {

    }

    virtual public void Left()
    {
        lefting = 2;
    }

    virtual public void Right()
    {
        righting = 2;
    }

    virtual public void Backward()
    {

    }

    virtual public void Up()
    {

    }

    virtual public void Down()
    {

    }


    virtual public void Sprint()
    {
        sprinting = 2;
    }


    public static VehicleScript GetVehicleScriptByType(GameObject vehicleObject, VehicleType type)
    {
        switch (type)
        {
            case VehicleType.FASTCAR:
                return vehicleObject.AddComponent<CarScript>();                
            default:
                return vehicleObject.AddComponent<CarScript>();
        }
    }

}
