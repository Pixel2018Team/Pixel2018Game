using Global;
using UnityEngine;

public class KidController : MonoBehaviour
{
    public SpriteController _cryLeft;
    public SpriteController _cryRight;

    public bool isBad = false;
    public bool isTag = false;
    public float freezeTime = 10.0f;
    public float currentTime;

    public InputMapping.PlayerTag playerTag;

    void Start()
    {
    }

    public void GetTagged()
    {
        isTag = true;
        _cryLeft.SetActive(true);
        _cryRight.SetActive(true);
        var animator = GetComponent<Animator>();
        animator.SetBool("crying", true);
        transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        var controller = GetComponent<TopDownKidsController>();
        controller.enabled = false;

        currentTime = freezeTime;
    }

    void Update()
    {
        if (isTag)
        {
            currentTime -= Time.deltaTime;
            if(currentTime < 0)
            {
                var controller = GetComponent<TopDownKidsController>();
                controller.enabled = true;
                var animator = GetComponent<Animator>();
                animator.SetBool("crying", false);

                isTag = false;
                _cryLeft.SetActive(false);
                _cryRight.SetActive(false);
            }
        }
        else
        {
            if (Input.GetButtonDown(InputMapping.GetInputName(playerTag, InputMapping.Input.Y)))
            {
                isBad = true;
            }
            else if (Input.GetButtonUp(InputMapping.GetInputName(playerTag, InputMapping.Input.Y)))
            {
                isBad = false;
            }
        }
    }
}
