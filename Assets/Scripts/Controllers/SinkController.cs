using Global;
using UnityEngine;

public class SinkController : MonoBehaviour
{
    private readonly string GONZUELA = "gonzuela";
    private bool isActive = false;

    public int washed = 0;
    public InputMapping.PlayerTag playerTag;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GONZUELA)
        {
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GONZUELA)
        {
            isActive = false;
        }
    }

    private void FixedUpdate()
    {
        if (isActive && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            ++washed;
        }
    }
}
