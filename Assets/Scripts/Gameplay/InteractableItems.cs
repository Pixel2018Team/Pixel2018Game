﻿using UnityEngine;
using System;

public class InteractableItems : MonoBehaviour
{
    [Serializable]
    public enum Type { Knock, Fix, Start, Clean };

    public Type _type;

    private float _timer = 0f;
    private bool _flag = false;
    private bool _reaction = false;
    private MeshRenderer _meshRend;
    private Animator _animator;
    private Vector3 _originalPos;

    // Use this for initialization
    private void Start()
    {
        _meshRend = GetComponent<MeshRenderer>();
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
    /// <param name="Direction">The direction in which the object should fall. Only to be used with the Knock state.</param>
    public void Interact(Vector3 Direction = new Vector3())
    {
        switch (_type)
        {
            case Type.Knock:
                // Move the object to have it fall
                this.transform.position += Direction;
                LevelManager.Instance.AddChaos(5);
                _type = Type.Clean;
                break;
            case Type.Clean:
                this.transform.position = _originalPos; // Puts the object back in its original place
                LevelManager.Instance.AddChaos(-5);
                _type = Type.Knock;
                break;
            case Type.Start:
                LevelManager.Instance.AddChaos(5);
                // Start the timer until the fire/flood
                _timer = 5f;
                _flag = true;
                _type = Type.Fix;
                break;
            case Type.Fix:
                LevelManager.Instance.AddChaos(-5);
                if (_reaction) LevelManager.Instance.AddChaos(-5);
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
        LevelManager.Instance.AddChaos(5);
        _reaction = true;
        _meshRend.enabled = true;
        _animator.enabled = true;
    }

    public void Fix()
    {
        _reaction = false;
        _meshRend.enabled = false;
        _animator.enabled = false;
    }
}
