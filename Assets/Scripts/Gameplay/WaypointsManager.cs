using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointsManager : MonoBehaviour {

    public static WaypointsManager _instance = null;
    public List<GameObject> waypoints;
    public int numberOfWaypoints;
    private float leftBound, rightBound, topBound, botBound;
    public GameObject planeReference;
    public GameObject waypointPrefab;

    public void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {

        //CreateWaypointsList();
    }

    public void CreateWaypointsList()
    {
        if(planeReference != null)
        {
            leftBound = planeReference.transform.localPosition.x - (planeReference.transform.localScale.x / 2);
            rightBound = planeReference.transform.localPosition.x + (planeReference.transform.localScale.x / 2);
            topBound = planeReference.transform.localPosition.y + (planeReference.transform.localScale.y / 2);
            botBound = planeReference.transform.localPosition.y - (planeReference.transform.localScale.y / 2);

            Debug.Log("left bound = " + leftBound);

            Debug.DrawRay(new Vector3(leftBound, 0, topBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(leftBound, 0, botBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(rightBound, 0, topBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(rightBound, 0, botBound), Vector3.up, Color.red, 5000);

            for (int i=0; i < numberOfWaypoints;i++)
            {
                var x = Random.Range(leftBound+1, rightBound);
                var z = Random.Range(botBound + 1, topBound);
                var wp = Instantiate(waypointPrefab, new Vector3(x, planeReference.transform.position.y, z), Quaternion.identity);


                waypoints.Add(wp);
            }
        }
    }

    public static WaypointsManager GetWaypointsManager()
    {
        return _instance;
    }
}
