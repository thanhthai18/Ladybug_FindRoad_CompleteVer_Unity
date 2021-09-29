using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeBar_LadybugMinigame : MonoBehaviour
{
    public Image Bar;
    public float max_time;
    public float current_time;
    public bool isTiming = false;

    private void Update()
    {
        if (isTiming)
        {
            current_time -= Time.deltaTime;
            if (current_time < 0)
            {
                current_time = 0;
                isTiming = false;
                GameController_LadybugMinigame.instance.isLose = true;
            }
            Bar.fillAmount = current_time / max_time;
        }
    }

}
