using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_LadybugMinigame : MonoBehaviour
{
    public Animator transition;

    public void LoadTransitionStart()
    {
        transition.SetBool("Start", true);
    }
    public void LoadTransitionEnd()
    {
        transition.SetBool("Start", false);
    }


}
