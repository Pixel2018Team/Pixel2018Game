using Global;
using UnityEngine;

public class RepaireController : MonoBehaviour
{
    private readonly string GONZUELA_TAG = "gonzuela";
    private bool _isActive = false;
    private SpriteController _spriteController;
    private GameObject _gonzuela;

    public float timeToRepaire = 5.0f;
    public bool repairing = false;

    public InputMapping.PlayerTag playerTag;

    void Start()
    {
        _spriteController = gameObject.GetComponentInChildren<SpriteController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GONZUELA_TAG)
        {
            _isActive = true;
            _spriteController.SetActive(true);
            _gonzuela = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GONZUELA_TAG)
        {
            _isActive = false;
            _spriteController.SetActive(false);
        }
    }

    void Update()
    {
        if (repairing)
        {
            timeToRepaire -= Time.deltaTime;
            if(timeToRepaire < 0)
            {
                var controller = _gonzuela.GetComponent<TopDownController>();
                controller.enabled = true;
                repairing = false;
            }
        }
        else if (_isActive && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            var controller = _gonzuela.GetComponent<TopDownController>();
            controller.enabled = false;
            repairing = true;
        }
    }
}
