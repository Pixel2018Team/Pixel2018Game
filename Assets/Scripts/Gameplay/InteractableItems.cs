using UnityEngine;
using System;

public class InteractableItems : MonoBehaviour
{
    [Serializable]
    public enum Type { Knock, Start, Fix, Clean };

    public Type _type = Type.Knock;

    private float _timer = 0f;
    private bool _flag = false;
    private bool _reaction = false;
    private MeshRenderer _sprite;
    private Animator _animator;
    private Vector3 _originalPos;

    // Use this for initialization
    private void Start()
    {
        _sprite = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
        _originalPos = this.transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if(_timer > 0)
        {
            _timer -= Time.deltaTime;
        }

        if(_flag && _timer<= 0)
        {
            _flag = false;
            Reaction();
        }
    }

    /// <summary>
    /// Method to handle the interaction with the object according to its type
    /// </summary>
    public void Interact()
    {
        switch (_type)
        {
            case Type.Knock:
                // Move the object to have it fall
                LevelManager.Instance._chaos += 5;
                _type = Type.Clean;
                break;
            case Type.Clean:
                this.transform.position = _originalPos; // Puts the object back in its original place
                LevelManager.Instance._chaos -= 5;
                _type = Type.Knock;
                break;
            case Type.Start:
                LevelManager.Instance._chaos += 5;
                // Start the timer until the fire/flood
                _timer = 5f;
                _flag = true;
                _type = Type.Fix;
                break;
            case Type.Fix:
                LevelManager.Instance._chaos -= 5;
                if(_reaction) LevelManager.Instance._chaos -= 5;
                _type = Type.Start;
                Fix();
                break;
        }
    }

    /// <summary>
    /// This method is used to create reactions like the oven catching fire
    /// </summary>
    public void Reaction()
    {
        LevelManager.Instance._chaos += 5;
        _reaction = true;
        _sprite.enabled = true;
        _animator.enabled = true;
    }

    public void Fix()
    {
        _reaction = false;
        _sprite.enabled = false;
        _animator.enabled = false;
    }
}
