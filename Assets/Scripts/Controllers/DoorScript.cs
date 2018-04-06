using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool _open = true;

    /// <summary>
    /// Do not use this method to close the door! Use Interact() instead.
    /// </summary>
    public void Open()
    {
        if (_open) return;
        else
        {
            // TODO: Code to open the door
            this._open = true;
        }
    }

    /// <summary>
    /// This method is used for the interaction with the door
    /// </summary>
    public void Interact()
    {
        LevelManager.Instance.OpenAllDoors();
        // TODO: Code to close this door
        this._open = false;
    }
}
