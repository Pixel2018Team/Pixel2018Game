using Global;
using UnityEngine;
using UnityEngine.UI;

public class RepaireController : MonoBehaviour
{
    private readonly string GONZUELA_TAG = "gonzuela";
    private bool _isActive = false;
    private SpriteController _spriteController;
    private Image _image;
    private GameObject _gonzuela;

    public float timeToRepaire = 5.0f;
    public float remainingTime = 5.0f;
    public bool repairing = false;

    public InputMapping.PlayerTag playerTag;

    void Start()
    {
        _spriteController = gameObject.GetComponentInChildren<SpriteController>();
        _image = gameObject.GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == GONZUELA_TAG && remainingTime > 0)
        {
            _isActive = true;
            _spriteController.SetActive(true);
            _gonzuela = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GONZUELA_TAG && remainingTime > 0)
        {
            _isActive = false;
            _spriteController.SetActive(false);
        }
    }

    void Update()
    {
        if (repairing)
        {
            remainingTime -= Time.deltaTime;
            _image.fillAmount = (timeToRepaire - remainingTime) / timeToRepaire;
            if (remainingTime < 0)
            {
                var controller = _gonzuela.GetComponent<TopDownController>();
                controller.enabled = true;
                repairing = false;
            }
        }
        else if (_isActive && remainingTime > 0 && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            var controller = _gonzuela.GetComponent<TopDownController>();
            controller.enabled = false;
            repairing = true;
            _spriteController.SetActive(false);
            _image.enabled = true;
        }
    }
}
