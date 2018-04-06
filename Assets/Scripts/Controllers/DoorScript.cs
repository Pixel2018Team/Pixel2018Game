using UnityEngine;

public class DoorManager : MonoBehaviour
{

    /// <summary>
    /// Do not use this method to close the door! Use Interact() instead.
    /// </summary>
    public void Open()
    {
        // Code to open the door
    }

    /// <summary>
    /// This method is used for the interaction with the door
    /// </summary>
    public void Interact()
    {
        LevelManager.Instance.OpenAllDoors();
        // Code to close this door
    }
}
