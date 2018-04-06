using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private bool _open = true;

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
