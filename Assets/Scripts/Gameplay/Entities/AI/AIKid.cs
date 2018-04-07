using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIKid : MonoBehaviour
{

    public TopDownKidsController controllerReference;
    public List<GameObject> wayPoints;
    public int maxIdleTime; //seconds
    public float distanceMargin; //margin used to decide if the npc has reached a waypoint

    private NavMeshAgent navMeshAgent;
    private AIState state;
    private float currentIdleTimer;
    private int nextIdleEndTime; //seconds
    private bool canChooseWaypoint;
    private Vector3 targetWaypoint;

    public enum AIState
    {
        Idle,
        GoingToWayPoint,
        ReachedWaypoint,
    }

    public void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Use this for initialization
    void Start()
    {
        wayPoints = WaypointsManager.GetWaypointsManager().waypoints;
        StartIdle();
    }

    // Update is called once per frame
    void Update()
    {
        //Behaviour when not following a player and not saved yet

        if (state == AIState.Idle)
        {
            currentIdleTimer += Time.deltaTime;
            var elapsedIdleSecs = currentIdleTimer % 60;
            //DebugLogger.Log("Idle current time = "+elapsedIdleSecs+"s", Enum.LoggerMessageType.Important);
            //DebugLogger.Log("I will move at " + nextIdleEndTime + "s", Enum.LoggerMessageType.Important);
            //choose a new destination when reaching the nextIdleEndTime
            if (elapsedIdleSecs >= nextIdleEndTime && canChooseWaypoint)
            {
                canChooseWaypoint = false;
                ChooseNextWaypoint();
            }
        }

        else if (state == AIState.GoingToWayPoint)
        {
            //Debug.Log("remaining distance : " + Vector3.Distance(transform.position, targetWaypoint));
            //If waypoint reached, trigger idle state
            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= distanceMargin)
            {
                StartIdle();
            }

            //Else continue moving
            else
            {

            }
        }
    }

    //Set the idle state
    public void StartIdle()
    {
        state = AIState.Idle;
        canChooseWaypoint = true;
        nextIdleEndTime = Random.Range(0, maxIdleTime);
        DebugLogger.Log(gameObject.name + "starts idling'!", Enum.LoggerMessageType.Error);
    }

    //Choose the next waypoint to move to
    public void ChooseNextWaypoint()
    {
        if (wayPoints != null && wayPoints.Count > 0)
        {
            var randomIdx = Random.Range(0, wayPoints.Count);
            targetWaypoint = wayPoints[randomIdx].transform.position;
            DebugLogger.Log(gameObject.name + "'s destination = " + targetWaypoint, Enum.LoggerMessageType.Important);
            navMeshAgent.SetDestination(targetWaypoint);
            state = AIState.GoingToWayPoint;
            DebugLogger.Log(gameObject.name + "starts moving !", Enum.LoggerMessageType.Important);
        }
    }

    public void EnableDisableAgent(bool enable)
    {
        if(navMeshAgent != null)
        {
            navMeshAgent.enabled = enable;
        }
    }
}
