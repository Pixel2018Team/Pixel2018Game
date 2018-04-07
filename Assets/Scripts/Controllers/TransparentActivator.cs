using UnityEngine;

public class TransparentActivator : MonoBehaviour
{
    public Material transparentMaterial;

    private string KID_TAG = "kid";
    private string GONZUELA_TAG = "gonzuela";
    private Material previousMaterial;
    private int bodyCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == KID_TAG || other.tag == GONZUELA_TAG)
        {
            if (bodyCount == 0)
            {
                previousMaterial = gameObject.GetComponent<Renderer>().material;
                gameObject.GetComponent<Renderer>().material = transparentMaterial;
            }
            ++bodyCount;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == KID_TAG || other.tag == GONZUELA_TAG)
        {
            --bodyCount;
            if(bodyCount == 0)
            {
                gameObject.GetComponent<Renderer>().material = previousMaterial;
            }
        }
    }
}
