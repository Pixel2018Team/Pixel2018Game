using Global;
using UnityEngine;

public class KidController : MonoBehaviour
{
    private SpriteRenderer _tag;

    public bool isBad = false;
    public bool isTag = false;
    public InputMapping.PlayerTag playerTag;

    void Start()
    {
        _tag = GetComponentInChildren<SpriteRenderer>();
    }

    public void GetTagged()
    {
        isTag = true;
        _tag.enabled = true;
        var animator = GetComponent<Animator>();
        animator.SetBool("crying", true);
        // Play animation
    }

    void Update()
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
