using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private float _clock = 0.0f;

    public Sprite[] actionSprites;
    public float interval = 1.0f;
    public bool active = true;
    public int spriteIndex = 0;

    void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetActive(bool value)
    {
        active = value;
        if (!value)
        {
            _renderer.sprite = null;
        }
    }

    void Update()
    {
        if (active)
        {
            _clock += Time.deltaTime;
            if(_clock > interval)
            {
                _clock = 0.0f;
                ++spriteIndex;
                if(spriteIndex >= actionSprites.Length)
                {
                    spriteIndex = 0;
                }
                _renderer.sprite = actionSprites[spriteIndex];
            }
        }
    }
}
