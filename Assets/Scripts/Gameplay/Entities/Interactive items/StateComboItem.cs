using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this class if your interactable should play an animation upon activation
public class StateComboItem : InteractableItem {

    public GameObject staticModel;
    public GameObject animatedModel;

	public override void TriggerActionOnCombo(Enum.ComboAnimType animType)
    {
        if (animType == Enum.ComboAnimType.StaticToAnimated)
        {
            SwapStaticToAnimatedRenderer();
        }

        else if (animType == Enum.ComboAnimType.AnimatedToStatic)
        {
            SwapAnimatedToStaticRenderer();
        }
    }

    public void SwapStaticToAnimatedRenderer()
    {
        if(staticModel != null && animatedModel != null)
        {
            staticModel.GetComponent<MeshRenderer>().enabled = false;
            animatedModel.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void SwapAnimatedToStaticRenderer()
    {
        if (staticModel != null && animatedModel != null)
        {
            staticModel.GetComponent<MeshRenderer>().enabled = true;
            animatedModel.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
