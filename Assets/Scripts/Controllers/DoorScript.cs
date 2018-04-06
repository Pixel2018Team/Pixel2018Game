using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool hasToOpen = false;
    public bool isRotating = false;
    public float rotSpeed;
    public bool test;
    private Vector3 startDir, openDir, closeDir;

    public void Awake()
    {
        closeDir = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        openDir = new Vector3(transform.right.x, transform.right.y, transform.right.z);
        Debug.DrawRay(transform.position, closeDir, Color.green, 1200);

        Debug.DrawRay(transform.position, openDir, Color.yellow, 1200);
    }

    public void OpenClose(bool open)
    {
        hasToOpen = open;
        Debug.Log("open = " + open);

        if (hasToOpen)
        {
            startDir = closeDir;
        }

        else
        {
            startDir = openDir;
        }

        isRotating = true;
    }

    public void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (test)
        {
            Debug.Log("test " + test);
            OpenClose(!hasToOpen);
            test = false;
        }

        if (isRotating)
        {
            if (hasToOpen)
            {
                Debug.Log("signed angle = " + Vector3.SignedAngle(transform.forward, openDir, transform.up));
                transform.Rotate(transform.up, rotSpeed * Time.deltaTime);

                if (Vector3.SignedAngle(transform.forward, openDir, transform.up) <= 0)
                {
                    isRotating = false;
                }
                Debug.Log("angle from starting angle = " + Vector3.Angle(transform.forward, openDir));
            }
            else
            {
                transform.Rotate(transform.up, -rotSpeed * Time.deltaTime);
                if (Vector3.SignedAngle(transform.forward, closeDir, transform.up) >= 0)
                {
                    isRotating = false;
                }
                Debug.Log("angle from starting angle = " + Vector3.Angle(transform.forward, closeDir));
            }




        }
    }

    /// <summary>
    /// This method is used for the interaction with the door
    /// </summary>
    public void Interact()
    {
        LevelManager.Instance.OpenAllDoors();
        // TODO: Code to close this door
        //this._open = false;
    }
}
