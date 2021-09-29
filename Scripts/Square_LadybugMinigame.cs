using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Square_LadybugMinigame : MonoBehaviour
{
    public float angleTrue;
    public float angle;
    public Vector3 pos, dir;
    RaycastHit2D hit;
    public bool isComplete;
    public bool isLock;
    

    void Start()
    {
        isComplete = false;
        isLock = false;
        
    }

    public bool CheckAngleTrue()
    {
        if ((int)(angle - angleTrue) % 180 == 0)
        {
            return true;
        }
        return false;
    }

    void CompleteRotate()
    {
        CheckAngleTrue();
    }

    public void Rotation()
    {
        transform.DORotate(new Vector3(0, 0, angle), 0.25f).OnComplete(CompleteRotate);
    }


    void Update()
    {
        if (!GameController_LadybugMinigame.instance.isNextLv && GameController_LadybugMinigame.instance.countWin != -1)
        {
            if (Input.GetMouseButtonDown(0))
            {            
                pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.Raycast(pos, Vector2.zero);
                if (hit && hit.collider != null)
                {
                    if (hit.collider.gameObject.Equals(gameObject))
                    {
                        if (SoundController.instance != null)
                        {
                            SoundController.instance.PlayOneShot(SoundController.instance.gameSoundMusic, GameController_LadybugMinigame.instance.listSoundAction[2]);
                        }
                        angle += 90;
                        transform.DORotate(new Vector3(0, 0, angle), 0.25f).OnComplete(CompleteRotate);
                        //GameController_LadybugMinigame.instance.myStep++;
                        if(GameController_LadybugMinigame.instance.tutorialIndex != -1)
                        {
                            GameController_LadybugMinigame.instance.tutorialIndex = -1;
                            GameController_LadybugMinigame.instance.tutorial.transform.DOKill();
                            GameController_LadybugMinigame.instance.tutorial.gameObject.SetActive(false);
                            GameController_LadybugMinigame.instance.timeBar.isTiming = true;
                        }
                        
                    }
                }
            }
        }

    }
}
