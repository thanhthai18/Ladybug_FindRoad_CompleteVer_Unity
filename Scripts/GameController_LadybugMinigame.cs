using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoObserver;
using DG.Tweening;

public class GameController_LadybugMinigame : MonoBehaviour
{
    //Singleton
    public static GameController_LadybugMinigame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Destroy(instance);
        }
    }
    //end Singleton


    [SerializeField] private List<Map_LadybugMinigame> map = new List<Map_LadybugMinigame>();
    [SerializeField] private Map_LadybugMinigame currentMap;
    [SerializeField] private Ladybug_LadybugMinigame ladyBug;
    [SerializeField] private WaypointController_LadybugMinigame waypointController;
    [SerializeField] private Transition_LadybugMinigame transition;
    private List<Map_LadybugMinigame> mapRandom = new List<Map_LadybugMinigame>();
    private List<Map_LadybugMinigame> mapFirstPlay = new List<Map_LadybugMinigame>();
    public Ladybug_LadybugMinigame currentLadybug;
    public SpriteRenderer tutorial;
    private int[] numOfRandom;
    public int index_NumOfRandom;
    public int level;
    public int countWin;
    public bool isNextLv;
    public bool isWin = false;
    public bool isLose;
    public bool isBegin;
    public bool isIntro;
    [SerializeField] private Camera cameraMain;
    //public int fullStep;
    //public int myStep;
    public int tutorialIndex;
    private int ran1, ran2, ran3;
    public TimeBar_LadybugMinigame timeBar;
    public Coroutine timeCoroutine;
    public int star;
    public GameObject starPrefab;
    public GameObject clock;
    public GameObject newStar;


    [Header("Audio")]
    public AudioClip soundBG;
    public List<AudioClip> listSoundAction;

    private void Start()
    {
        UtilitisMinigame.AddMenuMinigame();
        isBegin = true;
        isNextLv = false;
        isIntro = false;
        isLose = false;
        tutorial.gameObject.SetActive(false);

        for (int i = 0; i < map.Count; i++)
        {
            map[i].gameObject.SetActive(false);
        }
        //fullStep = 0;
        //myStep = 0;
        index_NumOfRandom = 0;
        level = 0;
        countWin = 1;
        tutorialIndex = -1;
        star = 0;
        SetUpMap();
        SetSizeCamera();


        if (SoundController.instance != null)
            SoundController.instance.PlayMusic(SoundController.instance.backgroundMusic, soundBG, true);

    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1.0f / Screen.height;

        cameraMain.orthographicSize *= f1 / f2;
    }

    public void SetUpMap()
    {
        ran1 = Random.Range(0, 3);
        ran2 = Random.Range(3, 6);
        ran3 = Random.Range(6, 9);
        mapRandom.Add(map[ran1]);
        mapRandom.Add(map[ran2]);
        mapRandom.Add(map[ran3]);
        numOfRandom = new int[3] { ran1, ran2, ran3 };
        isIntro = true;
        isWin = false;
        tutorialIndex = ran1;

        for (int i = 0; i < mapRandom.Count; i++)
        {
            mapRandom[i].gameObject.SetActive(false);
        }
        currentMap = mapRandom[level];
        currentMap.gameObject.SetActive(true);
        SpawnLadyBug();

        timeBar.max_time = 5;
        timeBar.current_time = 5;

        timeBar.Bar.fillAmount = timeBar.current_time / timeBar.max_time;



    }


    void SpawnLadyBug()
    {
        for (int i = 0; i < waypointController.transform.childCount; i++)
        {
            if (i == numOfRandom[index_NumOfRandom])
            {
                waypointController.transform.GetChild(i).gameObject.SetActive(true);
            }
            else
                waypointController.transform.GetChild(i).gameObject.SetActive(false);
        }

        ladyBug.waypointThisLevel = waypointController.allWaypointThisLevel[numOfRandom[index_NumOfRandom]];
        currentLadybug = Instantiate(ladyBug);
    }


    public void MoveLadybug()
    {
        if (SoundController.instance != null)
        {
            SoundController.instance.gameSoundMusic.volume = 1;
            SoundController.instance.PlayOneShot(SoundController.instance.gameSoundMusic, listSoundAction[4]);

        }
        currentLadybug.Move();
    }


    public void SetNextLv()
    {
        isIntro = false;
        level++;
        index_NumOfRandom++;
        NextLevel(level, index_NumOfRandom);
    }

    public void NextLevel(int level, int index)
    {
        currentMap.gameObject.SetActive(false);
        mapRandom[level].gameObject.SetActive(true);
        currentMap = mapRandom[level];
        SpawnLadyBug();
        if (level == 1)
        {
            timeBar.max_time = 6;
            timeBar.current_time = 6;
            timeBar.Bar.fillAmount = timeBar.current_time / timeBar.max_time;
            timeBar.isTiming = true;
        }
        if (level == 2)
        {
            timeBar.max_time = 10;
            timeBar.current_time = 10;
            timeBar.Bar.fillAmount = timeBar.current_time / timeBar.max_time;
            timeBar.isTiming = true;
        }
        
        Invoke(nameof(DelayCoutingTime), 2f);
    }

    void DelayCoutingTime()
    {
        timeBar.isTiming = true;
    }

    public void TransitionLevelStart()
    {
        transition.LoadTransitionStart();
        if (SoundController.instance != null)
        {
            SoundController.instance.PlayOneShot(SoundController.instance.gameSoundMusic, listSoundAction[5]);
        }
    }

    public void TransitionLevelEnd()
    {
        transition.LoadTransitionEnd();
    }

    void PostMenuWin()
    {
        if (SoundController.instance != null)
        {
            SoundController.instance.PlayOneShot(SoundController.instance.uiSoundMusic, SoundController.instance.gamesSoundMusic[12]);
        }
        //if ((myStep - fullStep) < 4)
        //{
        //    this.PostEvent(EventID.OnEndMinigame, 3);
        //}
        //if ((myStep - fullStep) >= 4 && (myStep - fullStep) <= 7)
        //{
        //    this.PostEvent(EventID.OnEndMinigame, 2);
        //}
        //if ((myStep - fullStep) > 7)
        //{
        //    this.PostEvent(EventID.OnEndMinigame, 1);
        //}
        this.PostEvent(EventID.OnEndMinigame, star);
    }
    void PostMenuLose()
    {
        this.PostEvent(EventID.OnEndMinigame, star);
    }


    private void Update()
    {
        if (countWin == 4)
        {
            isWin = true;
        }

        if (isWin)
        {
            countWin = -1;
            isWin = false;
            isBegin = false;
            Invoke(nameof(PostMenuWin), 5.5f);
        }
        if (isLose)
        {
            countWin = -1;
            isLose = false;
            isBegin = false;
            PostMenuLose();
        }
    }



}
