using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladybug_LadybugMinigame : MonoBehaviour
{
    public AllWaypointThisLevel_LadybugMinigame waypointThisLevel;
    private float speed = 11f;
    private Tween ladybugMovement;
    private Vector3[] roadLadybug;
    private int angleZ;
    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_ShakeStart, anim_Run, anim_BodyShake;


    private void Start()
    {
        anim.state.Complete += AnimComplete;
        if (GameController_LadybugMinigame.instance.isIntro)
        {
            PlayAnim(anim, anim_ShakeStart, true);
        }

        angleZ = 0;
        if (GameController_LadybugMinigame.instance.isBegin)
        {
            Invoke("SetPos", 0.1f);
        }
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        if (trackEntry.Animation.Name == anim_ShakeStart)
        {
            //PlayAnim(anim, anim_BodyShake, true);
        }
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    public void SetPos()
    {
        transform.position = waypointThisLevel.road[0];
    }

    public void Move()
    {
        roadLadybug = waypointThisLevel.road;
        float distance = 0;
        for (int i = 1; i < roadLadybug.Length; i++)
        {
            distance += (roadLadybug[i - 1] - roadLadybug[i]).magnitude;
        }

        PlayAnim(anim, anim_Run, true);

        ladybugMovement = transform.DOPath(roadLadybug, distance / speed, PathType.Linear).SetEase(Ease.Linear);
        ladybugMovement.OnComplete(() =>
        {
            if (GameController_LadybugMinigame.instance.countWin <= 3)
            {
                if (GameController_LadybugMinigame.instance.level < 2)
                {
                    Invoke("LevelUp", 1f);
                    Destroy(gameObject, 1f);
                    GameController_LadybugMinigame.instance.TransitionLevelStart();
                    Invoke("PlayTransitionEnd", 0.933f);
                    PlayAnim(anim, anim_BodyShake, true);
                }
            }
            if (GameController_LadybugMinigame.instance.countWin == -1)
            {
                PlayAnim(anim, anim_ShakeStart, true);
            }

            GameController_LadybugMinigame.instance.isNextLv = false;
            ladybugMovement.Kill();
        });
    }

    void LevelUp()
    {
        GameController_LadybugMinigame.instance.SetNextLv();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Point"))
        {
            angleZ += 90;
            gameObject.transform.DORotate(new Vector3(0, 0, angleZ), 0.2f);
        }
        if (collision.gameObject.CompareTag("Toy"))
        {
            angleZ -= 90;
            gameObject.transform.DORotate(new Vector3(0, 0, angleZ), 0.2f);
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            if (SoundController.instance != null)
            {
                SoundController.instance.gameSoundMusic.Stop();
            }
        }
    }

    void PlayTransitionEnd()
    {
        Destroy(GameController_LadybugMinigame.instance.newStar);
        GameController_LadybugMinigame.instance.TransitionLevelEnd();
    }


}




