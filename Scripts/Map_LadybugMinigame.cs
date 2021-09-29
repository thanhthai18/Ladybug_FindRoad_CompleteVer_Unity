using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map_LadybugMinigame : MonoBehaviour
{
    [SerializeField] private List<Square_LadybugMinigame> square = new List<Square_LadybugMinigame>();
    [SerializeField] private List<FadeFX_LadybugMinigame> line = new List<FadeFX_LadybugMinigame>();
    private int CountSquareComplete;
    private Tween tut;


    private void Start()
    {
        line = GetComponentsInChildren<FadeFX_LadybugMinigame>().ToList();
        Invoke("ShowTutorial", 0.5f);

        for (int i = 0; i < square.Count; i++)
        {
            if (square[i].CompareTag("Item"))
            {
                square[i].angle += 90;
                square[i].Rotation();
                //GameController_LadybugMinigame.instance.fullStep++;
            }
            else
            {
                int r = Random.Range(1, 4);
                float a = r * 90;
                square[i].angle += a;
                square[i].Rotation();
                //GameController_LadybugMinigame.instance.fullStep += (4 - r);
            }
        }
    }

    void ShowTutorial()
    {
        var index = GameController_LadybugMinigame.instance.tutorialIndex;
        if (index == 0 || index == 1 || index == 2)
        {
            GameController_LadybugMinigame.instance.tutorial.transform.position = square[0].transform.position;
            GameController_LadybugMinigame.instance.tutorial.gameObject.SetActive(true);
            tut = GameController_LadybugMinigame.instance.tutorial.transform.DOScale(1.2f, 1f).OnComplete(() =>
            {
                GameController_LadybugMinigame.instance.tutorial.transform.DOScale(0.9f, 0.5f);
            });
            tut.SetLoops(-1);
        }
    }

    void CallMove()
    {
        GameController_LadybugMinigame.instance.MoveLadybug();
    }
    void DelayCorrectSound()
    {
        if (SoundController.instance != null)
        {
            SoundController.instance.gameSoundMusic.volume = 0.1f;
            SoundController.instance.PlayOneShot(SoundController.instance.gameSoundMusic, GameController_LadybugMinigame.instance.listSoundAction[1]);       
        }
    }

    private void Update()
    {
        for (int i = 0; i < square.Count; i++)
        {
            if (square[i].CompareTag("Item"))
            {
                if (square[i].angle % 180 == 0)
                {
                    square[i].isComplete = true;
                }
                else
                {
                    square[i].isComplete = false;
                }
            }
            if (square[i].CompareTag("Enemy"))
            {
                if (square[i].angle % 360 == 0)
                {
                    square[i].isComplete = true;
                }
                else
                {
                    square[i].isComplete = false;
                }
            }
        }

        for (int j = 0; j < square.Count; j++)
        {
            if (square[j].isComplete)
            {
                if (!square[j].isLock)
                {
                    CountSquareComplete++;
                    square[j].isLock = true;
                }
            }
            else
            {
                if (square[j].isLock)
                {
                    CountSquareComplete--;
                    square[j].isLock = false;
                }
            }
        }

        if (CountSquareComplete == square.Count)
        {
            GameController_LadybugMinigame.instance.isNextLv = true;
            for (int i = 0; i < line.Count; i++)
            {
                line[i].FadeFx();
                Invoke(nameof(DelayCorrectSound), 0.5f);
            }

            if (GameController_LadybugMinigame.instance.countWin <= 3)
            {
                GameController_LadybugMinigame.instance.countWin++;
                Debug.Log(GameController_LadybugMinigame.instance.countWin);
                CountSquareComplete = 0;
            }
            if (GameController_LadybugMinigame.instance.countWin <= 4)
            {
                Invoke("CallMove", 1.8f);
                GameController_LadybugMinigame.instance.timeBar.isTiming = false;
                GameController_LadybugMinigame.instance.star++;
                GameController_LadybugMinigame.instance.newStar = Instantiate(GameController_LadybugMinigame.instance.starPrefab, GameController_LadybugMinigame.instance.currentLadybug.transform.position, Quaternion.identity);
                GameObject newStarObject = GameController_LadybugMinigame.instance.newStar;
                newStarObject.transform.DOScale(2, 0.5f);
                newStarObject.transform.DOMove(new Vector2(GameController_LadybugMinigame.instance.clock.transform.position.x - 1, GameController_LadybugMinigame.instance.clock.transform.position.y - 1), 1f).OnComplete(() =>
                {
                    newStarObject.transform.DOMove(GameController_LadybugMinigame.instance.clock.transform.position, 0.3f);
                    newStarObject.transform.DOScale(0.55f, 0.3f).OnComplete(()=>
                    {
                        newStarObject.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(1, 0.3f);
                        newStarObject.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(1, 0.3f);
                        newStarObject.transform.GetChild(2).GetComponent<SpriteRenderer>().DOFade(1, 0.3f);

                        newStarObject.transform.GetChild(0).transform.DOPunchRotation(new Vector3(180, 180, 180), 0.5f);
                        newStarObject.transform.GetChild(1).transform.DOPunchRotation(new Vector3(180, 180, 180), 0.5f);
                        newStarObject.transform.GetChild(2).transform.DOPunchRotation(new Vector3(180, 180, 180), 0.5f).OnComplete(() =>
                        {
                            newStarObject.transform.GetChild(0).GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
                            newStarObject.transform.GetChild(1).GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
                            newStarObject.transform.GetChild(2).GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
                        });
                    });                  
                });

            }
        }
    }
}