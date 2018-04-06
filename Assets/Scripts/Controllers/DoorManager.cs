using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance = null;

    // Singleton logic
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(this);
    }

    // Use this for initialization
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {

    }

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
