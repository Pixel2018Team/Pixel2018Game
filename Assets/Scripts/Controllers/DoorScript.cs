using UnityEngine;

public class DoorManager : MonoBehaviour
{

    /// <summary>
    /// Do not use this method to close the door! Use Interact() instead.
    /// </summary>
    public void Close()
    {
        // Code to close the door
    }

    /// <summary>
    /// This method is used for the interaction with the door
    /// </summary>
    public void Interact()
    {
        LevelManager.Instance.CloseAllDoors();
        // Code to open this door
    }
}
