using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class VehicleCityAi : MonoBehaviour
{

    public Animator animControler;
    public VehicleType type;
    public bool alive=true;

    bool moving = true;


    RoadCheckPoint target;

    NavMeshAgent agent;


    // Start is called before the first frame update
    void Start()
    {
        if (alive)
        {
            float minDistance = float.PositiveInfinity;
            Road closestRoad = null;

            GameObject[] allRoads = GameObject.FindGameObjectsWithTag("Road");

            foreach (GameObject roadObj in allRoads)
            {
                if (Vector3.Distance(transform.position, roadObj.transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(transform.position, roadObj.transform.position);
                    closestRoad = roadObj.GetComponent<Road>();
                }
            }


            agent = GetComponent<NavMeshAgent>();
            target = closestRoad.GetStart(this.transform.position);
            agent.SetDestination(target.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            CheckDistance();
            animControler.SetBool("Moving", moving);
        }

    }


    void CheckDistance()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
        {
            target = target.GetNext();
            agent.SetDestination(target.transform.position);
        }
    }
}
