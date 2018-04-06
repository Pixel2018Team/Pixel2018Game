using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteractableItems : MonoBehaviour
{
    [Serializable]
    public enum Type { Knock, Start, Pickup, Open };
    public Type _type = Type.Knock;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

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
                break;
            case Type.Open:
                // Bouger l'objet avec une animation
                break;
            case Type.Pickup:
                Pickup();
                break;
            case Type.Start:
                // Demarrer un timer
                break;
        }
    }

    public GameObject Pickup()
    {
        return this.gameObject;
    }
}
