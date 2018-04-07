using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public bool hasToOpen = false;
    public bool isOpened = false;
    public bool isRotating = false;
    public float rotSpeed;
    public bool test;
    private Vector3 startDir, openDir, closeDir;
    private int numberOfKidsInbounds;
    public bool doorLocked;
    public float maxOpenTime = 1.5f;
    public float currentOpenTime = 0f;

    private void Awake()
    {
        closeDir = new Vector3(transform.forward.x, transform.forward.y, transform.forward.z);
        openDir = new Vector3(transform.right.x, transform.right.y, transform.right.z);
        Debug.DrawRay(transform.position, closeDir, Color.green, 1200);
        Debug.DrawRay(transform.position, openDir, Color.yellow, 1200);
        numberOfKidsInbounds = 0;
    }

    public void OpenClose(bool open)
    {
        if (!doorLocked)
        {
            hasToOpen = open;
            //Debug.Log("open = " + open);

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
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward, Color.red);

        if (test)
        {
            //Debug.Log("test " + test);
            OpenClose(!hasToOpen);
            test = false;
        }

        if (isRotating)
        {
            if (hasToOpen && !isOpened)
            {
                //Debug.Log("signed angle = " + Vector3.SignedAngle(transform.forward, openDir, transform.up));
                transform.Rotate(transform.up, rotSpeed * Time.deltaTime);

                if (Vector3.SignedAngle(transform.forward, openDir, transform.up) <= 0)
                {
                    isRotating = false;
                    currentOpenTime = 0f;
                    isOpened = true;
                }

               // Debug.Log("angle from starting angle = " + Vector3.Angle(transform.forward, openDir));
            }
            //has to close
            else
            {
                currentOpenTime += Time.deltaTime;
                var elapsedOpenSecs = currentOpenTime % 60;
                if (elapsedOpenSecs >= maxOpenTime)
                {
                    isOpened = false;
                    transform.Rotate(transform.up, -rotSpeed * Time.deltaTime);
                    if (Vector3.SignedAngle(transform.forward, closeDir, transform.up) >= 0)
                    {
                        isRotating = false;
                        
                    }
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
    }

    public void OnTriggerEnter(Collider col)
    {
        var obj = col.gameObject;
        if (obj.tag == "kid")
        {
            numberOfKidsInbounds++;

            if (!isOpened)
            {
                OpenClose(true);
            }
            
            /*if (obj.GetComponent<TopDownKidsController>().controlledByAI)
            {
                OpenClose(true);
            }*/
        }
    }

    public void OnTriggerExit(Collider col)
    {
        var obj = col.gameObject;
        if (obj.tag == "kid")
        {
            numberOfKidsInbounds--;

            if(numberOfKidsInbounds <= 0 && isOpened)
            {
                OpenClose(false);
            }

            if(numberOfKidsInbounds < 0)
            {
                numberOfKidsInbounds = 0;
            }
        }
    }
}
