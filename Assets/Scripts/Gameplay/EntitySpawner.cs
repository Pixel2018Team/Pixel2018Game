using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{

    public static EntitySpawner _instance = null;
    public GameObject planeReference;
    public GameObject playerPrefab;
    public GameObject humanAiPrefab;
    public GameObject robotPrefab;
    public GameObject safeZonePrefab;
    public GameObject spawnPointPrefab;
    public GameObject waypointPrefab;
    public int numberOfHumanBots;
    public int numberOfSafeZones;
    public int numberOfSpawnPoints;
    public int minDistanceBetweenFixedEntities = 50;
    public int numberOfWaypoints;

    public List<GameObject> players;
    public GameObject robot;
    public List<GameObject> spawnPoints;
    public List<GameObject> safeZones;
    public List<GameObject> humanBots;
    public List<GameObject> zombieBots;
    public List<GameObject> waypoints;
    public PlayerSpawnMethod playersSpawnPosition;

    public enum PlayerSpawnMethod
    {
        OnCenter,
        OnCorners,
        Random
    };

    private float leftBound, rightBound, topBound, botBound;

    public void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    public void Start()
    {

    }

    public void CreateSpawnPoints()
    {
        if (spawnPointPrefab != null)
        {
            GameObject spawnPointsList = new GameObject();
            spawnPointsList.name = "SpawnPoints";
            spawnPointsList.transform.position = Vector3.zero;

            for (int i = 0; i < numberOfSpawnPoints; i++)
            {
                var x = Random.Range(leftBound + 1, rightBound);
                var z = Random.Range(botBound + 1, topBound);
                var sp = Instantiate(spawnPointPrefab, new Vector3(x, planeReference.transform.position.y, z), Quaternion.identity);

                sp.transform.parent = spawnPointsList.transform;
                sp.name = "SpawnPoint" + i;

                AdjustEntityPosition(sp, 50, spawnPoints.Concat(safeZones), true);
                spawnPoints.Add(sp);
                ShiftYOnPlane(sp);
            }
        }
    }

    public void CreateSafeZones()
    {
        if (safeZonePrefab != null)
        {
            GameObject safeZonesList = new GameObject();
            safeZonesList.name = "SafeZones";
            safeZonesList.transform.position = Vector3.zero;

            for (int i = 0; i < numberOfSafeZones; i++)
            {
                var x = Random.Range(leftBound + 1, rightBound);
                var z = Random.Range(botBound + 1, topBound);
                var sz = Instantiate(safeZonePrefab, new Vector3(x, planeReference.transform.position.y, z), Quaternion.identity);

                sz.transform.parent = safeZonesList.transform;

                safeZones.Add(sz);

                AdjustEntityPosition(sz, minDistanceBetweenFixedEntities, safeZones, true);
            }
        }
    }

    public void CreatePlayers(int nbPlayers)
    {
        //Instantiate the human player prefab and add players to the list
        if (playerPrefab != null)
        {
            for (int i = 0; i < nbPlayers; i++)
            {
                var p = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

                var pShooterController = p.GetComponent<ShooterController>();

                switch (i)
                {
                    case 0:
                        pShooterController.playerTag = Global.InputMapping.PlayerTag.P1;
                        break;
                    case 1:
                        pShooterController.playerTag = Global.InputMapping.PlayerTag.P2;
                        break;
                    case 2:
                        pShooterController.playerTag = Global.InputMapping.PlayerTag.P3;
                        break;

                }

                //spawn in center
                if (playersSpawnPosition == PlayerSpawnMethod.OnCenter)
                {
                    if (i == 0)
                    {
                        p.transform.position = planeReference.transform.position;
                    }

                    if (i == 1)
                    {
                        p.transform.position = new Vector3(planeReference.transform.position.x - 3, 0, planeReference.transform.position.z);
                    }

                    if (i == 2)
                    {
                        p.transform.position = new Vector3(planeReference.transform.position.x + 3, 0, planeReference.transform.position.z);
                    }
                }

                //spawn in corners
                else if (playersSpawnPosition == PlayerSpawnMethod.OnCorners)
                {

                }

                //random
                else
                {

                }

                ShiftYOnPlane(p);
                p.GetComponent<Collider>().enabled = true;
            }
        }
    }

    public void CreateNPCs()
    {
        if (humanAiPrefab != null)
        {
            for (int i = 0; i < numberOfHumanBots; i++)
            {
                var npc = Instantiate(humanAiPrefab, Vector3.zero, Quaternion.identity);

                var randomSpawnIndex = Random.Range(0, spawnPoints.Count());

                var sp = spawnPoints[randomSpawnIndex];
                var spPosition = sp.transform.position;

                var xOffset = Random.Range(-5, 5);
                var zOffset = Random.Range(-5, 5);

                var spDirectionX = xOffset > 0 ? sp.transform.localScale.x / 2 : -sp.transform.localScale.x / 2;
                var spDirectionZ = zOffset > 0 ? sp.transform.localScale.z / 2 : -sp.transform.localScale.z / 2;

                npc.transform.position = new Vector3(spPosition.x + spDirectionX, planeReference.transform.position.y, spPosition.z + spDirectionZ);

                humanBots.Add(npc);
                ShiftYOnPlane(npc);
            }
        }
    }

    public void CreateWaypointsList()
    {
        if (planeReference != null)
        {
            Debug.Log("left bound = " + leftBound);

            Debug.DrawRay(new Vector3(leftBound, 0, topBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(leftBound, 0, botBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(rightBound, 0, topBound), Vector3.up, Color.red, 5000);
            Debug.DrawRay(new Vector3(rightBound, 0, botBound), Vector3.up, Color.red, 5000);

            GameObject wpList = new GameObject();
            wpList.transform.position = Vector3.zero;
            wpList.name = "Waypoints";

            for (int i = 0; i < numberOfWaypoints; i++)
            {
                var x = Random.Range(leftBound + 1, rightBound);
                var z = Random.Range(botBound + 1, topBound);
                var wp = Instantiate(waypointPrefab, new Vector3(x, planeReference.transform.position.y, z), Quaternion.identity);

                wp.transform.parent = wpList.transform;

                waypoints.Add(wp);
                ShiftYOnPlane(wp);
            }
        }
    }


    //Adjust the position of an element according to a minimal distance, away from other objects of a list (or two lists)
    public void AdjustEntityPosition(GameObject obj, float minimalDistance, IEnumerable<GameObject> objList, bool awayFromCenter)
    {
        var maxIterations = 150;
        var positionAdjusted = false;

        for (int i = 0; i < maxIterations; i++)
        {
            var x = Random.Range(leftBound + 1, rightBound);
            var z = Random.Range(botBound + 1, topBound);

            Vector3 randPosition = new Vector3(x, planeReference.transform.position.y, z);

            if (!objList.Any(o => Vector3.Distance(o.transform.position, randPosition) < minimalDistance))
            {
                if (awayFromCenter)
                {
                    if (Vector3.Distance(planeReference.transform.position, randPosition) < minimalDistance)
                    {
                        positionAdjusted = true;
                        Debug.Log("Found a suitable position away  from center");
                    }
                }
                else
                {
                    positionAdjusted = true;
                    Debug.Log("Found a suitable position");
                }
            }

            else
            {
                Debug.Log("Havent found a position for " + obj.name + " at iteration " + i);
            }

            if (positionAdjusted)
            {
                obj.transform.position = randPosition;
                Debug.Log(" position found for " + obj.name + " at iteration " + i);
                return;
            }
        }

    }

    //Shift an entity's position on Y so its bottom is placed on the plane's Y
    public void ShiftYOnPlane(GameObject obj)
    {
        obj.transform.position = new Vector3(obj.transform.position.x, planeReference.transform.position.y + (obj.transform.localScale.y / 2), obj.transform.position.z);

        Debug.Log("final position = " + obj.transform.position);
    }

    //Get the current entity manager
    public static EntitySpawner GetInstance()
    {
        return _instance;
    }

    public void SetPlaneReference(GameObject reference)
    {
        planeReference = reference;

        leftBound = planeReference.transform.localPosition.x - (planeReference.transform.localScale.x / 2);
        rightBound = planeReference.transform.localPosition.x + (planeReference.transform.localScale.x / 2);
        topBound = planeReference.transform.localPosition.y + (planeReference.transform.localScale.y / 2);
        botBound = planeReference.transform.localPosition.y - (planeReference.transform.localScale.y / 2);
    }

}
