using UnityEngine;

public abstract class State
{
    public abstract void Update(GameObject gameObject);
    public abstract void OnTriggerStay(GameObject gameObject, Collider other);
}
