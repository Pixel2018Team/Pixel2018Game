using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPCHuman : MonoBehaviour
{
    public List<GameObject> wayPoints;
    public bool isPanicked;
    public bool searchForSafeDestinationEnabled;
    public bool outOfRangeFromEvilGuy;
    public float panicDetectionDistance;
    public float fleeTime;
    public float fleeSpeed;
    public float fleeMinDistance;
    public float fleeMaxDistance;
    public GameObject evilGuy; //quick n dirty
    protected NavMeshAgent navMeshAgent;
    private float baseSpeed;
    private float currentFleeTimer;
    public GameObject playerToFollow;
    public bool isSaved;

    // Use this for initialization
    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        baseSpeed = navMeshAgent.speed;
        outOfRangeFromEvilGuy = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSaved)
        {
            CheckDistanceToEvilGuyAndPanicMode();

            if (isPanicked)
            {
                Debug.DrawLine(transform.position, evilGuy.transform.position, Color.red, 0.1f);

                //if panicked but far enough from evil guy, check if it's time to choose a destination
                if (outOfRangeFromEvilGuy)
                {
                    DebugLogger.Log("out of range from evil guy, think timer has begin", Enum.LoggerMessageType.Important);
                    currentFleeTimer += Time.deltaTime;

                    var elapsedFleeSecs = currentFleeTimer % 60;

                    //When the initial flee time has been reached, choose a random point opposite to the evil guy direction to flee to
                    if (elapsedFleeSecs >= fleeTime)
                    {
                        DebugLogger.Log("think timer OK", Enum.LoggerMessageType.Important);
                        if (!searchForSafeDestinationEnabled)
                        {
                            DebugLogger.Log("Looking for new destination", Enum.LoggerMessageType.Important);
                            SearchForSafeDestination();
                        }
                    }
                }
            }

            else
            {
                if(evilGuy != null)
                {
                    Debug.DrawLine(transform.position, evilGuy.transform.position, Color.green, 0.1f);
                }
            }
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, navMeshAgent.destination, Time.deltaTime);
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        //tmp
        if(collision.gameObject.tag == TagsRef.PLAYER_TAG && !isPanicked && !isSaved && playerToFollow == null)
        {
            playerToFollow = collision.gameObject;
            var playerController = playerToFollow.GetComponent<PlayerController>();
            if (!playerController.followingAis.Any(ai => ai == gameObject))
            {
                playerToFollow.GetComponent<PlayerController>().followingAis.Add(gameObject);
            }
        }
    }

    public void LeavePlayer()
    {
        playerToFollow = null;
    }

    public void SaveHuman(GameObject safeZone)
    {
        LeavePlayer();
        isSaved = true;
        navMeshAgent.SetDestination(safeZone.transform.position);
    }

    public void CheckDistanceToEvilGuyAndPanicMode()
    {
        if(evilGuy != null)
        {
            //Debug.Log("distance to bad guy = " + Vector3.Distance(evilGuy.transform.position, transform.position));
            if (Vector3.Distance(evilGuy.transform.position, transform.position) <= panicDetectionDistance)
            {
                TriggerPanicMode();
            }

            else
            {
                if (!outOfRangeFromEvilGuy)
                {
                    outOfRangeFromEvilGuy = true;
                }
            }
        }
    }

    public void SearchForSafeDestination()
    {
        searchForSafeDestinationEnabled = true;

        Vector3 directionFromEvilGuy =  transform.position - evilGuy.transform.position;

        //Face the opposite direction from evil guy
        transform.rotation = Quaternion.LookRotation(directionFromEvilGuy);

        //Check if there are waypoints somewhere in the opposite direction

        //var wayPoints = WaypointsManager.GetWaypointsManager().waypoints;
        var wayPoints = EntitySpawner.GetInstance().waypoints;

        var candidateWaypoints = wayPoints.Where(wp => Vector3.Dot(transform.forward, (wp.transform.position - transform.position)) >= 0).ToList();

        //If there are candidates, take a random one as destination
        if(candidateWaypoints.Count() > 0)
        {
            var randomIndex = Random.Range(0, candidateWaypoints.Count());

            navMeshAgent.SetDestination(candidateWaypoints[randomIndex].transform.position);
            Debug.DrawLine(candidateWaypoints[randomIndex].transform.position, candidateWaypoints[randomIndex].transform.up, Color.cyan, 5000);
        }

        //Else go back to the normal wandering behaviour
        else
        {
            var randomIndex = Random.Range(0, wayPoints.Count());

            navMeshAgent.SetDestination(wayPoints[randomIndex].transform.position);
            Debug.DrawLine(wayPoints[randomIndex].transform.position, wayPoints[randomIndex].transform.up, Color.green, 5000);
        }


        navMeshAgent.speed = baseSpeed;

        DebugLogger.Log("Safe destination has been chosen ", Enum.LoggerMessageType.Important);
        isPanicked = false;
    }

    public void TriggerPanicMode()
    {
        isPanicked = true;
        outOfRangeFromEvilGuy = false;
        currentFleeTimer = 0f;
        searchForSafeDestinationEnabled = false;
        if(playerToFollow != null)
        {
            playerToFollow = null;
        }
        //Flee vector = opposite direction of the evil guy's
        Vector3 fleeDirection = (transform.position - evilGuy.transform.position).normalized;

        //Take a random point along this vector
        Vector3 randomPointInFleeDirection = Random.Range(fleeMinDistance, fleeMaxDistance) * fleeDirection;

        Debug.DrawLine(randomPointInFleeDirection, transform.position, Color.blue, 5000);

        //Face the flee vector and move
        transform.rotation = Quaternion.LookRotation(randomPointInFleeDirection);

        navMeshAgent.SetDestination(randomPointInFleeDirection);
        navMeshAgent.speed = fleeSpeed;
        DebugLogger.Log("FUUUU I'm panicked", Enum.LoggerMessageType.Important);

    }
}
