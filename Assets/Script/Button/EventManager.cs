using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;
    public GameObject animalTrail;
    public Transform cursor;

    bool isMainStart;
    bool isGameStart;

    [SerializeField]
    Animator canvas;

    [SerializeField]
    GameObject mainUI;
    [SerializeField]
    GameObject gameUI;

    bool isOption = false;
    bool isCredit = false;

    public bool isReplayScene = false;

    private void Awake()
    {
        instance = this;

        isMainStart = false;
        isGameStart = false;

        if(isReplayScene)
        {
            isMainStart = true;
            isGameStart = true;

            animalManager.gameObject.SetActive(true);
            
            mainUI.SetActive(false);
            gameUI.SetActive(true);

            if (isPause)
            {
                PauseOffMain();
            }

            gameUI.GetComponent<Animator>().Play("GameUIOn");

            SoundManager.instance.SetBGM(SoundManager.instance.ingame);
        }

        SoundManager.instance.SetBGM(SoundManager.instance.main);
    }

    private void Update()
    {
        if (!isMainStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMainStart = true;
                // 시작 애니메이션
                canvas.Play("TouchToStart_InMain");
                animalTrail.SetActive(true);
            }
        }
    }


    [SerializeField]
    AnimalManager animalManager;
    public void GameStart()
    {
        isGameStart = true;

        animalTrail.SetActive(false);

        animalManager.gameObject.SetActive(true);
        
        mainUI.SetActive(false);
        gameUI.SetActive(true);

        if (isPause)
        {
            PauseOffMain();
        }

        gameUI.GetComponent<Animator>().Play("GameUIOn");

        SoundManager.instance.SetBGM(SoundManager.instance.ingame);
    }


    #region PAUSE
    bool isPause = false;
    // 인 게임
    [SerializeField]
    GameObject gamePause;
    [SerializeField]
    GameObject homeReturn;
    public void PauseButtonGame()
    {
        if (isPause)
        {
            isPause = false;
            PauseOffGame();
        }
        else
        {
            isPause = true;
            PauseOnGame();
        }
    }

    [SerializeField]
    GameObject lastScore;

    public void GameOver()
    {
        lastScore.SetActive(true);
        homeReturn.SetActive(true);

        Time.timeScale = 0;
    }

    void PauseOnGame()
    {
        gamePause.SetActive(true);
        homeReturn.SetActive(true);

        Time.timeScale = 0;
    }

    void PauseOffGame()
    {
        gamePause.SetActive(false);
        homeReturn.SetActive(false);

        Time.timeScale = 1;
    }

    [SerializeField]
    Animator mainPause;
    // 메인 메뉴
    public void PauseButtonMain()
    {
        if (!isGameStart)
        {
            if (isPause)
            {
                isPause = false;
                PauseOffMain();
            }
            else
            {
                isPause = true;
                PauseOnMain();
            }
        }
        else
        {
            PauseButtonGame();
        }
    }
    void PauseOnMain()
    {
        mainPause.Play("On");
    }
    void PauseOffMain()
    {
        mainPause.Play("Off");
    }
    #endregion

    [SerializeField]
    GameObject blackSpace;
    [SerializeField]
    GameObject option;
    [SerializeField]
    GameObject credit;

    // 음악 활성화 여부
    public static bool isEffect = true;
    public static bool isBackground = true;

    // Touch화면으로 간다.
    public void BackIntro()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("2048");
    }

    public void GoReplay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Replay");
    }

    public void CanvasStop()
    {
        canvas.Stop();
    }

    public void CreditButton()
    {
        blackSpace.SetActive(true);
        credit.SetActive(true);
        option.SetActive(false);

        isOption = false;
        isCredit = true;
    }

    public void OptionButton()
    {
        blackSpace.SetActive(true);
        credit.SetActive(false);
        option.SetActive(true);

        isOption = true;
        isCredit = false;
    }


    // -73.5 -> 73.5
    public void EffectButton(GameObject target)
    {
        if (isEffect)
        {
            isEffect = false;
            iTween.MoveTo(target, iTween.Hash("islocal", true, "x", 73.5f, "time", 0.3f));
        }
        else
        {
            isEffect = true;
            iTween.MoveTo(target, iTween.Hash("islocal", true, "x", -73.5f, "time", 0.3f));
        }
    }
    public void BackgroundButton(GameObject target)
    {
        if (isBackground)
        {
            isBackground = false;
            iTween.MoveTo(target, iTween.Hash("islocal", true, "x", 73.5f, "time", 0.3f));
        }
        else
        {
            isBackground = true;
            iTween.MoveTo(target, iTween.Hash("islocal", true, "x", -73.5f, "time", 0.3f));
        }
    }

    public void MainPlayButtonShutDownOff(GameObject target)
    {
        CanvasStop();
        target.GetComponent<Animator>().Play("Off");
        GameStart();
    }
    public void MainPlayButtonShutDownOn(GameObject target)
    {
        target.GetComponent<Animator>().Play("On");
    }

    public void ShutDown(GameObject target)
    {
        target.gameObject.SetActive(false);
    }

    public void ShutDownUI()
    {
        isCredit = false;
        isOption = false;

        blackSpace.SetActive(false);
        credit.SetActive(false);
        option.SetActive(false);
    }
}