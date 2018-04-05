using Global;
using UnityEngine;

public class TrapActionController : MonoBehaviour
{
    public int trapCount = 3;
    public InputMapping.PlayerTag playerTag;

    void Update()
    {
        if (Input.GetButtonDown(InputMapping.GetInputName(playerTag, InputMapping.Input.B)) && trapCount > 0)
        {
            --trapCount;
            PlaceTrap();
        }
    }

    private void PlaceTrap()
    {
        GameObject trap = Instantiate(Resources.Load("Trap", typeof(GameObject))) as GameObject;
        trap.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
    }
}
