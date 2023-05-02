using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Player : TransformerScript
{
    public GameObject CameraCenter;
    public Camera Camera;
    public Animator cameraAnimator;
    private float cameraRotSpeed = 5f;
    public RectTransform GunPoint;

    private bool idle = true;
    float idleTime = 2f;
    float lastAction = -1;

    bool GunPointFocused = false;


    VehicleType seletedVehicleType = VehicleType.NONE;

    protected override void Update()
    {
        UpdateGunPoint();
        base.Update();        
        if (!idle)
        {
            if (Time.time > lastAction + idleTime)
            {
                idle = true;
            }
        }        
    }

    void UpdateGunPoint()
    {
        if (!GunPointFocused)
        {
            
            GunPoint.localPosition = Vector3.Lerp(GunPoint.localPosition, Vector3.zero, 0.5f);            
            GunPoint.localScale = Vector3.Lerp(GunPoint.localScale, Vector3.one, 0.5f);                      
        }
    }


    void UpdateGunPoint(Vector3 pointToTarget)
    {
        Vector3 screenPos = Camera.WorldToScreenPoint(pointToTarget);                

        GunPoint.position = Vector2.Lerp(GunPoint.position, screenPos, 0.5f);        
    }


    protected override void RobotAction()
    {
        CheckKeyboardRobot();
        CheckMouseRobot();
    }

    private void CheckKeyboardRobot()
    {
        //movement
        if (Input.GetKeyDown("w") ||
            Input.GetKeyDown("s") ||
            Input.GetKeyDown("a") ||
            Input.GetKeyDown("d"))
        {
            SetWalking(true);
        }
        else if (Input.GetKey("w") ||
                Input.GetKey("s") ||
                Input.GetKey("a") ||
                Input.GetKey("d"))
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey("w")) dir += new Vector2(0, 1);
            if (Input.GetKey("s")) dir -= new Vector2(0, 1);
            if (Input.GetKey("a")) dir -= new Vector2(1, 0);
            if (Input.GetKey("d")) dir += new Vector2(1, 0);

            WalkTowards(dir);
            SetAimDirection(dir);
            StopIdle();

        }
        else if (Input.GetKeyUp("w") ||
                Input.GetKeyUp("s") ||
                Input.GetKeyUp("a") ||
                Input.GetKeyUp("d"))
        {
            SetWalking(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            SetRunning(true);
        }


        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            SetRunning(false);

        }

        //transform
        if (Input.GetKeyDown("e"))
        {
            if (Transform()) StopIdle();
        }

        
        //search for vehicles
        if (Input.GetKey("q"))
        {
            //send ray to collide with vehicle
            seletedVehicleType = RayForVehicle();
        }

        if (Input.GetKeyUp("q"))
        {
            if (seletedVehicleType != VehicleType.NONE)
            {
                StopIdle();
                ChangeVehicle(seletedVehicleType);
            }
            GunPointFocused = false;
        }
    }



    VehicleType RayForVehicle()
    {        
        RaycastHit hit;
        Ray ray = Camera.ScreenPointToRay(new Vector3(Camera.pixelWidth / 2, Camera.pixelHeight / 2, 0));        
        
        if (Physics.SphereCast(ray.origin, 10, ray.direction, out hit, Mathf.Infinity))
        {
            
            GameObject hitObject = hit.collider.gameObject;

            
            if ((1 << hitObject.layer & LayerMask.GetMask("Vehicle")) != 0)
            {
                GunPointFocused = true;
                UpdateGunPoint(hitObject.transform.position);
                return hitObject.GetComponent<VehicleCityAi>().type;
            }
                                              
        }
        else
        {
            GunPointFocused = false;
        }
        return VehicleType.NONE;
    }


    const float maxPitch = 70;
    const float minPitch = -70;
    private void CheckMouseRobot()
    {

        //rotation of camera
        float yaw = Input.GetAxis("Mouse X") * cameraRotSpeed;
        float pitch = Input.GetAxis("Mouse Y") * cameraRotSpeed;

        Vector3 camRot = CameraCenter.transform.localEulerAngles;

        if (idle)
        {
            camRot.y += yaw;
        }
        else
        {
            // left/right rotation of whole character
            transform.localRotation = Quaternion.Euler(transform.localEulerAngles + new Vector3(0, yaw, 0));
        }

        camRot.x = ClampAngle(minPitch, maxPitch, camRot.x - pitch);

        CameraCenter.transform.localRotation = Quaternion.Euler(camRot);



        //aim
        if (Input.GetMouseButtonDown(1))
        {            
            CamAimMode();            
            StopIdle();
        }

        if (Input.GetMouseButtonUp(1))
        {
            CamNormalMode();            
        }

        //shoot
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.ScreenPointToRay(new Vector3(Camera.pixelWidth/2,Camera.pixelHeight/2,0));
            int layerMask = LayerMask.GetMask("Player");
            layerMask = ~layerMask; //everything but player
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, layerMask))
            {                
                Shoot(hit.point);
            }
            else
            {
                Shoot(ray.origin + ray.direction*1000);
                
            }
            
        }
    }


    /// <summary>
    ///  min i max s¹ sprowadzane do zakresu <0,360)
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private float ClampAngle(float min, float max, float t)
    {
        min = min % 360;
        min = min < 0 ? min + 360 : min;
        max = max % 360;

        t = t % 360;
        t = t < 0 ? t + 360 : t;


        if (min <= max)
        {
            if (t < min)
            {
                return min;
            }
            else if (t > max)
            {
                return max;
            }
            else
            {
                return t;
            }
        }
        else
        {
            if (t < min && t > max)
            {
                if (Mathf.Abs(t - min) <= Mathf.Abs(t - max))
                {
                    return min;
                }
                else
                {
                    return max;
                }
            }
            else
            {
                return t;
            }
        }
    }


    private void StopIdle()
    {
        lastAction = Time.time;
        if (idle)
        {
            idle = false;

            //also rotate to the camera
            float camRoty = CameraCenter.transform.rotation.eulerAngles.y;

            CameraCenter.transform.localRotation = Quaternion.Euler(Vector3.Scale(CameraCenter.transform.localEulerAngles, new Vector3(1, 0, 1)));

            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, camRoty, 0));
        }
    }



    protected override void VehicleAction()
    {
        CheckKeyboardVehicle();
        CheckMouseVehicle();
    }


    private void CheckKeyboardVehicle()
    {
        //transform
        if (Input.GetKeyDown("e"))
        {
            Transform();
        }

        if (Input.GetKey("w"))
        {
            vehicleScript.Forward();
        }

        if (Input.GetKey("d"))
        {
            vehicleScript.Right();
        }

        if (Input.GetKey("a"))
        {
            vehicleScript.Left();
        }

        if (Input.GetKey("s"))
        {
            vehicleScript.Backward();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            vehicleScript.Sprint();
        }


    }


    private void CheckMouseVehicle()
    {
        float yaw = Input.GetAxis("Mouse X") * cameraRotSpeed;
        float pitch = Input.GetAxis("Mouse Y") * cameraRotSpeed;

        Vector3 camRot = CameraCenter.transform.localEulerAngles;

        camRot.y += yaw;

        camRot.x = ClampAngle(minPitch, maxPitch, camRot.x - pitch);

        CameraCenter.transform.localRotation = Quaternion.Euler(camRot);
    }




    ///cameras functions -------------------------------------------------------------------------------------    

    public void CamNormalMode()
    {
        cameraAnimator.Play("Cam.Robot");
        SetAim(false);
    }


    public void CamAimMode()
    {
        cameraAnimator.Play("Cam.RobotAim");
        SetAim(true);
    }
}
