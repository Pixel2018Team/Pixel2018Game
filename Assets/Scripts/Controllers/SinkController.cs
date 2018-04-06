using Global;
using UnityEngine;
using UnityEngine.UI;

public class SinkController : MonoBehaviour
{
    private readonly string GONZUELA = "gonzuela";
    private bool isActive = false;

    public float washed = 0;
    public float doneWashing = 10.0f;
    public InputMapping.PlayerTag playerTag;
    private Image _image;
    private SpriteController _spriteController;

    void Start()
    {
        _spriteController = gameObject.GetComponentInChildren<SpriteController>();
        _image = gameObject.GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == GONZUELA)
        {
            _spriteController.SetActive(true);
            isActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == GONZUELA)
        {
            _spriteController.SetActive(false);
            isActive = false;
        }
    }

    private void Update()
    {
        if (isActive && Input.GetButton(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            _spriteController.SetActive(false);
            washed += Time.deltaTime;
            _image.fillAmount = washed / doneWashing;
        }
    }
}
