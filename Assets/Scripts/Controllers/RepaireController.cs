using Global;
using UnityEngine;
using UnityEngine.UI;

public class RepaireController : MonoBehaviour
{
    private readonly string GONZUELA_TAG = "gonzuela";
    private readonly string KID_TAG = "kid";
    public bool _isActive = false;
    public SpriteController _actionButton;
    public SpriteController _smoke;
    private Image _image;

    private GameObject _actor;
    public InputMapping.PlayerTag playerTag;

    public float timeForAction = 5.0f;
    public float remainingTime = 5.0f;
    public bool acting = false;
    public bool isBroken = false;

    public string breakSoundEventName;
    public string fixSoundEventName;

    public float chaosLevel = 0.0f;

    void Start()
    {
        _image = gameObject.GetComponentInChildren<Image>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_actor == null)
        {
            if (isBroken && other.tag == GONZUELA_TAG)
            {
                _isActive = true;
                _actionButton.SetActive(true);
                _actor = other.gameObject;
                playerTag = _actor.GetComponent<TopDownController>().playerTag;
            }
            else if(!isBroken && other.tag == KID_TAG)
            {
                var controller = other.gameObject.GetComponent<TopDownKidsController>();
                if (!controller.controlledByAI)
                {
                    _isActive = true;
                    _actionButton.SetActive(true);
                    _actor = other.gameObject;
                    playerTag = controller.playerTag;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_actor == other.gameObject)
        {
            if (!acting)
            {
                _isActive = false;
                _actionButton.SetActive(false);
                _actor = null;
            }
        }
    }

    public void CatchActing()
    {
        _actor.GetComponent<KidController>().GetTagged();

        var animator = _actor.GetComponent<Animator>();
        animator.SetBool("action", false);

        _actor = null;
        acting = false;
        _isActive = false;
        remainingTime = timeForAction;
        _image.fillAmount = 0.0f;
    }

    void Update()
    {
        if (isBroken)
        {
            chaosLevel += Time.deltaTime;
            if (chaosLevel > 1.0f)
            {
                chaosLevel = 0.0f;
                LevelManager.Instance.AddChaos(1);
            }
        }

        if (acting)
        {
            remainingTime -= Time.deltaTime;
            _image.fillAmount = (timeForAction - remainingTime) / timeForAction;
            if (remainingTime < 0)
            {
                if (isBroken)
                {
                    var controller = _actor.GetComponent<TopDownController>();
                    controller.enabled = true;
                }
                else
                {
                    var controller = _actor.GetComponent<TopDownKidsController>();
                    controller.enabled = true;
                }
                var animator = _actor.GetComponent<Animator>();
                animator.SetBool("action", false);
                acting = false;
                isBroken = !isBroken;

                if (isBroken)
                {
                    AkSoundEngine.PostEvent(breakSoundEventName, gameObject);
                }
                else
                {
                    AkSoundEngine.PostEvent(fixSoundEventName, gameObject);
                    LevelManager.Instance.AddChaos(-10);
                }

                _smoke.SetActive(isBroken);
                remainingTime = timeForAction;
                _image.fillAmount = 0.0f;
            }
        }
        else if (_isActive && Input.GetButtonDown(InputMapping.GetInputName(playerTag, InputMapping.Input.X)))
        {
            if (isBroken)
            {
                var controller = _actor.GetComponent<TopDownController>();
                if(controller != null)
                {
                    controller.enabled = false;
                }
            }
            else
            {
                var controller = _actor.GetComponent<TopDownKidsController>();
                controller.enabled = false;
            }
            _actor.GetComponent<Rigidbody>().velocity = Vector3.zero;
            _actor.transform.LookAt(new Vector3(transform.position.x, _actor.transform.position.y, transform.position.z));

            var animator = _actor.GetComponent<Animator>();
            animator.SetBool("action", true);

            _actionButton.SetActive(false);
            _image.enabled = true;
            acting = true;
        }
    }
}
