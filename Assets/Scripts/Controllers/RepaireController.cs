using Global;
using UnityEngine;

public class RepaireController : MonoBehaviour
{
    private readonly string GONZUELA = "gonzuela";
    private bool _isActive = false;
    private SpriteController _spriteController;

    public float timeToRepaire = 5.0f;

    public InputMapping.PlayerTag playerTag;

    void Start()
    {
        _spriteController = gameObject.GetComponentInChildren<SpriteController>();
    }

    public bool isBroken()
    {
        return timeToRepaire > 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GONZUELA)
        {
            _isActive = true;
            _spriteController.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GONZUELA)
        {
            _isActive = false;
            _spriteController.SetActive(false);
        }
    }

    void Update()
    {
        if (_isActive && isBroken() && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            timeToRepaire -= Time.deltaTime;
        }
    }
}
