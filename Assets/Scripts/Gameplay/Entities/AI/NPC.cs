using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public List<GameObject> wayPoints;
    public NPCHuman humanScript;
    protected NavMeshAgent navMeshAgent;
    public int maxIdleTime; //seconds

    private float currentIdleTimer;
    private int nextIdleEndTime; //seconds
    private bool canChooseWaypoint;
    private State state;
    private Vector3 targetWaypoint;

    public float distanceMargin; //margin used to decide if the npc has reached a waypoint

    public void Awake()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        Debug.Log("manager = " + WaypointsManager.GetWaypointsManager());

        humanScript = GetComponent<NPCHuman>();
    }

    public enum State
    {
        Idle,
        GoingToWayPoint,
        ReachedWaypoint,
        FollowingPlayer,
        Attacking,
        Panicking,
        Saved,
        Dead
    }

    // Use this for initialization
    void Start()
    {
        //wayPoints = WaypointsManager.GetWaypointsManager().waypoints;
        wayPoints = EntitySpawner.GetInstance().waypoints;
        StartIdle();
    }

    // Update is called once per frame
    void Update()
    {

        //Behaviour when following a player
        if (humanScript.playerToFollow != null)
        {
            if (state != State.FollowingPlayer)
            {
                state = State.FollowingPlayer;
            }
            transform.position = Vector3.Slerp(transform.position, humanScript.playerToFollow.transform.position - (humanScript.playerToFollow.transform.forward * 5), Time.deltaTime * 2);
            navMeshAgent.SetDestination(humanScript.playerToFollow.transform.position - (humanScript.playerToFollow.transform.forward * 5)); //stick behind the player
        }


        //Behaviour when not following a player and not saved yet
        if (!humanScript.isPanicked && !humanScript.isSaved)
        {
            if (state == State.Idle)
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

            else if (state == State.GoingToWayPoint)
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

            //If the state is panicking here, that's because the humanScript had the isPanicked = true but now it's over and the human is going to a waypoint
            else if (state == State.Panicking)
            {
                DebugLogger.Log("Panicked is finished, going to waypoint", Enum.LoggerMessageType.Important);
                state = State.GoingToWayPoint;
            }
        }

        else if (humanScript.isPanicked)
        {
            if (state != State.Panicking)
            {
                state = State.Panicking;
            }

            if (currentIdleTimer > 0f)
            {
                currentIdleTimer = 0f;
            }
        }

        else if (humanScript.isSaved)
        {
            if (state != State.Saved)
            {
                state = State.Saved;
            }
        }
    }

    //Set the idle state
    public void StartIdle()
    {
        state = State.Idle;
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
            state = State.GoingToWayPoint;
            DebugLogger.Log(gameObject.name + "starts moving !", Enum.LoggerMessageType.Important);
        }
    }
}
