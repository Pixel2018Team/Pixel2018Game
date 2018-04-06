using Global;
using UnityEngine;

public class RepaireController : MonoBehaviour
{
    private readonly string GONZUELA = "gonzuela";
    private bool isActive = false;

    public float timeToRepaire = 5.0f;

    public InputMapping.PlayerTag playerTag;

    void Start()
    {

    }

    public bool isBroken()
    {
        return timeToRepaire > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GONZUELA)
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

    void Update()
    {
        if (isActive && isBroken() && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            timeToRepaire -= Time.deltaTime;
        }
    }
}
