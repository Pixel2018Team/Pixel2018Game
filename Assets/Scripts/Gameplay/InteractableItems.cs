using UnityEngine;
using System;

public class InteractableItems : MonoBehaviour
{
    [Serializable]
    public enum Type { Knock, Start, Pickup, Open, Fix, Clean, Close, Drop };
    public Type _type = Type.Knock;
    private float _timer = 0f;
    private bool _flag = false;
    private MeshRenderer _sprite;
    private Animator _animator;

    // Use this for initialization
    private void Start()
    {
        _sprite = GetComponent<MeshRenderer>();
        _animator = GetComponent<Animator>();
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
                // Deplacer l'objet pour qu'il tombe
                LevelManager.Instance._chaos += 5;
                _type = Type.Clean;
                break;
            case Type.Clean:
                LevelManager.Instance._chaos -= 5;
                _type = Type.Knock;
                break;
            case Type.Open:
                // Bouger l'objet avec une animation
                break;
            case Type.Close:
                // Faire le deplacement inverse a Open
                // S'assurer qu'une seule porte est ouverte
                break;
            case Type.Pickup:
                _type = Type.Drop;
                Pickup();
                break;
            case Type.Drop:
                LevelManager.Instance._chaos += 5;
                // drop behaviour
                break;
            case Type.Start:
                LevelManager.Instance._chaos += 5;
                // Demarrer un timer
                _timer = 5f;
                _flag = true;
                _type = Type.Fix;
                break;
            case Type.Fix:
                LevelManager.Instance._chaos -= 5;
                _type = Type.Start;
                Fix();
                break;
        }
    }

    public GameObject Pickup()
    {
        return this.gameObject;
    }

    /// <summary>
    /// This method is used to create reactions like the oven catching fire
    /// </summary>
    public void Reaction()
    {
        _sprite.enabled = true;
        _animator.enabled = true;
    }

    public void Fix()
    {
        _sprite.enabled = false;
        _animator.enabled = false;
    }
}
