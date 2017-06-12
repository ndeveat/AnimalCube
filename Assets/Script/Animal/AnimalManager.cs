using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager instance;
    public AnimalType mainCreateType = AnimalType.Squarle;
    public AnimalType highestType;
    public GameObject Gun;

    public GameObject newAnimalFox;
    public GameObject newAnimalBear;
    public GameObject newAnimalLion;
    public GameObject newAnimalHyena;

    GameObject newAnimal;

    public Dictionary<AnimalType, bool> newAnimals;

    public int GunDropProbability = 1;

    Vector3 onPos;
    Vector3 endPos;

    [HideInInspector]
    public GestureType gestureType;

    [HideInInspector]
    public AnimalBlock[,] animals;

    const int animalsx = 7;
    const int animalsy = 7;

    public Text debugText;

    [SerializeField]
    GameObject animalState;
    [SerializeField]
    Text animalName;
    [SerializeField]
    Text animalAccount;

    Camera mainCamera;

    Vector3 mainCameraOriginalPosition;

    GameObject zoomTargetAnimal;

    bool isZoom = false;
    bool isNoCreate;
    bool cheat;

    float cameraTime = 0.0f;

    Transform thisTransform;

    [SerializeField]
    GameObject tilesParent;


    // -----------

    void Awake()
    {
        Time.timeScale = 1;

        instance = this;

        animals = new AnimalBlock[animalsy, animalsx];

        newAnimals = new Dictionary<AnimalType, bool>();
        newAnimals.Add(AnimalType.Fox, false);
        newAnimals.Add(AnimalType.Hyena, false);
        newAnimals.Add(AnimalType.Lion, false);
        newAnimals.Add(AnimalType.Bear, false);

        mainCameraOriginalPosition = Camera.main.transform.position;

        gestureType = GestureType.NONE;

        Init();
    }

    public void Init()
    {
        TileInitialize();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
            cheat = true;
        else if (Input.GetKeyUp(KeyCode.Z))
            cheat = false;

        if (!GunScript.instance.shootClick)
            Gesture();
    }

    void TileInitialize()
    {
        StartCoroutine(TileAnimator());
    }

    IEnumerator TileAnimator()
    {
        for (int i = 0; i < animalsy; i++)
        {
            for (int j = 0; j < animalsx; j++)
            {
                string str = "tile_" + Random.Range(0, 4).ToString();

                GameObject tile = (GameObject)Resources.Load(str);
                tile = Instantiate(tile);
                tile.transform.parent = tilesParent.transform;
                tile.transform.position = new Vector3(j * 0.52f, 0.5f, i * 0.52f);
                tile.transform.localScale = new Vector3(0.5f, 0.5f, 0.11f);

                animals[i, j] = tile.GetComponent<AnimalBlock>();
                animals[i, j].tilepos = new Vector2(j, i);
                animals[i, j].name = i.ToString() + " " + j.ToString();

                yield return new WaitForSeconds(0.025f);
            }
        }

        for (int i = 0; i < animalsy; i++)
        {
            for (int j = 0; j < animalsx; j++)
            {
                animals[i, j].value = 0;
            }
        }

        mainCamera = Camera.main;

        thisTransform = mainCamera.transform;

        highestType = mainCreateType;

        ButtonManager.instance.SetListNumbers();

        //FirstAnimalCreate();
        AnimalCreate(1);
        AnimalTileReset();
    }

    // 블럭들을 움직이고 재 설정한다.
    public void Resetting()
    {
        bool resettingDefault = false;
        switch (gestureType)
        {
            case GestureType.DOWN:
                {
                    #region DOWN
                    for (int x = 0; x < animalsx; x++)
                    {
                        for (int y = 0; y < animalsy; y++)
                        {
                            for (int my = y; my >= 0; my--)
                            {
                                int tmy = my - 1;
                                if (tmy >= 0)
                                {
                                    if (animals[my, x].animalAnimal != null)
                                    {
                                        if (animals[my, x].animalAnimal.moveDistance < 1)
                                            break;

                                        DecreaseMoveCount(x, my);

                                        if (animals[tmy, x].value == 0)
                                        {
                                            Swap(x, my, x, tmy);
                                        }
                                        else if (animals[tmy, x].animalAnimal != null)
                                        {
                                            if (animals[tmy, x].animalAnimal.animalType == animals[my, x].animalAnimal.animalType && animals[tmy, x].animalAnimal.animalType == AnimalType.Bear && animals[my, x].animalAnimal.animalType == AnimalType.Bear && animals[tmy, x].animalAnimal.level == 4 && animals[my, x].animalAnimal.level == 4)
                                            {
                                                return;
                                            }

                                            if (animals[tmy, x].animalAnimal.animalType == animals[my, x].animalAnimal.animalType &&
                                                animals[tmy, x].animalAnimal.level == animals[my, x].animalAnimal.level)
                                            {
                                                animals[tmy, x].value *= 2;
                                                animals[my, x].value = 0;

                                                DecreaseMoveCount(tmy, x);

                                                if (animals[tmy, x].animalAnimal.moveDistance < 1)
                                                    break;

                                                SwapAnimal(x, my, x, tmy);
                                            }
                                            else if (animals[tmy, x].animalAnimal.초식 != animals[my, x].animalAnimal.초식)
                                            {
                                                if (animals[tmy, x].animalAnimal.초식)
                                                    CatchAnimal(tmy, x, my, x);
                                                else
                                                    CatchAnimal(my, x, tmy, x);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                break;
            case GestureType.UP:
                {
                    #region UP
                    for (int x = 0; x < animalsx; x++)
                    {
                        for (int y = animalsy; y >= 0; y--)
                        {
                            for (int my = y; my < animalsy; my++)
                            {
                                int tmy = my + 1;
                                if (tmy < animalsy)
                                {
                                    if (animals[my, x].animalAnimal != null)
                                    {
                                        if (animals[my, x].animalAnimal.moveDistance < 1)
                                            break;

                                        DecreaseMoveCount(x, my);

                                        if (animals[tmy, x].value == 0)
                                        {
                                            Swap(x, my, x, tmy);
                                        }
                                        else if (animals[tmy, x].animalAnimal != null)
                                        {
                                            if (animals[tmy, x].animalAnimal.animalType == animals[my, x].animalAnimal.animalType && animals[tmy, x].animalAnimal.animalType == AnimalType.Bear && animals[my, x].animalAnimal.animalType == AnimalType.Bear && animals[tmy, x].animalAnimal.level == 4 && animals[my, x].animalAnimal.level == 4)
                                            {
                                                return;
                                            }

                                            if (animals[tmy, x].animalAnimal.animalType == animals[my, x].animalAnimal.animalType && animals[tmy, x].animalAnimal.level == animals[my, x].animalAnimal.level)
                                            {
                                                animals[tmy, x].value *= 2;
                                                animals[my, x].value = 0;

                                                SwapAnimal(x, my, x, tmy);
                                            }
                                            else if (animals[tmy, x].animalAnimal.초식 != animals[my, x].animalAnimal.초식)
                                            {
                                                if (animals[tmy, x].animalAnimal.초식)
                                                    CatchAnimal(tmy, x, my, x);
                                                else
                                                    CatchAnimal(my, x, tmy, x);
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    #endregion
                }
                break;
            case GestureType.RIGHT:
                {
                    #region RIGHT
                    for (int y = 0; y < animalsy; y++)
                    {
                        for (int x = animalsx; x >= 0; x--)
                        {
                            for (int mx = x; mx < animalsx; mx++)
                            {
                                int tmx = mx + 1;
                                if (tmx < animalsx)
                                {
                                    if (animals[y, mx].animalAnimal != null)
                                    {
                                        if (animals[y, mx].animalAnimal.moveDistance < 1)
                                            break;

                                        DecreaseMoveCount(mx, y);

                                        if (animals[y, tmx].value == 0)
                                        {
                                            Swap(mx, y, tmx, y);
                                        }
                                        else if (animals[y, tmx].animalAnimal != null)
                                        {
                                            if (animals[y, tmx].animalAnimal.animalType == animals[y, mx].animalAnimal.animalType && animals[y, tmx].animalAnimal.animalType == AnimalType.Bear && animals[y, mx].animalAnimal.animalType == AnimalType.Bear && animals[y, tmx].animalAnimal.level == 4 && animals[y, mx].animalAnimal.level == 4)
                                            {
                                                return;
                                            }

                                            if (animals[y, tmx].animalAnimal.animalType == animals[y, mx].animalAnimal.animalType && animals[y, tmx].animalAnimal.level == animals[y, mx].animalAnimal.level)
                                            {
                                                animals[y, tmx].value *= 2;
                                                animals[y, mx].value = 0;

                                                SwapAnimal(mx, y, tmx, y);
                                            }
                                            else if (animals[y, tmx].animalAnimal.초식 != animals[y, mx].animalAnimal.초식)
                                            {
                                                if (animals[y, tmx].animalAnimal.초식)
                                                    CatchAnimal(y, tmx, y, mx);
                                                else
                                                    CatchAnimal(y, mx, y, tmx);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                break;
            case GestureType.LEFT:
                {
                    #region LEFT
                    for (int y = 0; y < animalsy; y++)
                    {
                        for (int x = 0; x < animalsx; x++)
                        {
                            for (int mx = x; mx >= 0; mx--)
                            {
                                int tmx = mx - 1;
                                if (tmx >= 0)
                                {
                                    if (animals[y, mx].animalAnimal != null)
                                    {
                                        if (animals[y, mx].animalAnimal.moveDistance < 1)
                                            break;

                                        DecreaseMoveCount(mx, y);

                                        if (animals[y, tmx].value == 0)
                                        {
                                            Swap(mx, y, tmx, y);
                                        }
                                        else if (animals[y, tmx].animalAnimal != null)
                                        {
                                            if (animals[y, tmx].animalAnimal.animalType == animals[y, mx].animalAnimal.animalType && animals[y, tmx].animalAnimal.animalType == AnimalType.Bear && animals[y, mx].animalAnimal.animalType == AnimalType.Bear && animals[y, tmx].animalAnimal.level == 4 && animals[y, mx].animalAnimal.level == 4)
                                            {
                                                return;
                                            }

                                            if (animals[y, tmx].animalAnimal.animalType == animals[y, mx].animalAnimal.animalType && animals[y, tmx].animalAnimal.level == animals[y, mx].animalAnimal.level)
                                            {
                                                animals[y, tmx].value *= 2;
                                                animals[y, mx].value = 0;

                                                SwapAnimal(mx, y, tmx, y);
                                            }
                                            else if (animals[y, tmx].animalAnimal.초식 != animals[y, mx].animalAnimal.초식)
                                            {
                                                if (animals[y, tmx].animalAnimal.초식)
                                                    CatchAnimal(y, tmx, y, mx);
                                                else
                                                    CatchAnimal(y, mx, y, tmx);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                break;
            default:
                {
                    resettingDefault = true;
                }
                break;
        }

        if (!resettingDefault)
        {
            ResetMoveCount();
            AnimalTileReset();
            GunCreate();
        }
    }

    public void Print()
    {
        if (debugText == null)
            return;

        string text = "";

        for (int i = animalsy - 1; i >= 0; i--)
        {
            for (int j = 0; j < animalsx; j++)
            {
                text += animals[i, j].value.ToString();
                text += " ";
            }
            text += "\n";
        }
        debugText.text = text;
    }

    void GunCreate()
    {
        if (Random.Range(0, GunDropProbability) != 0)
            return;

        System.Random r = new System.Random();

        int _x = 0, _y = 0;
        do
        {
            _x = r.Next(animalsx);
            _y = r.Next(animalsy);
        }
        while (animals[_y, _x].value != 0);

        animals[_y, _x].SetGun(Gun);
    }

    public void AnimalTileReset()
    {
        for (int i = 0; i < animalsy; i++)
        {
            for (int j = 0; j < animalsx; j++)
            {
                animals[i, j].tilepos = new Vector2(j, i);
                animals[i, j].Move();
            }
        }
    }

    public void Swap(int x, int y, int mx, int my)
    {
        int temp = animals[y, x].value;
        Animal tempAnimal = animals[y, x].animalAnimal;

        animals[y, x].value = animals[my, mx].value;
        animals[y, x].animalAnimal = animals[my, mx].animalAnimal;

        if (animals[y, x].animalAnimal != null)
            animals[y, x].animalAnimal.matrixPos = new Vector2(x, y);

        animals[my, mx].value = temp;
        animals[my, mx].animalAnimal = tempAnimal;
        if (animals[my, mx].animalAnimal != null)
            animals[my, mx].animalAnimal.matrixPos = new Vector2(mx, my);
    }

    public void CatchAnimal(int y, int x, int my, int mx /*int 초식y,int 초식x, int 육식y, int 육식x*/)
    {
        SoundManager.instance.sum.Play();

        if ((int)animals[y, x].animalAnimal.animalType - 4 > (int)animals[my, mx].animalAnimal.animalType)
        {
            animals[y, x].value = 0;
        }
        else
        {
            animals[my, mx].value = 0;

            if (ButtonManager.instance.bts[(int)animals[y, x].animalAnimal.animalType - 4].transform.Find("Text").GetComponent<Text>().text != "" &&
                System.Convert.ToInt32(ButtonManager.instance.bts[(int)animals[y, x].animalAnimal.animalType - 4].transform.Find("Text").GetComponent<Text>().text) > 0)
            {
                ButtonManager.instance.bts[(int)animals[y, x].animalAnimal.animalType - 4].transform.Find("Text").GetComponent<Text>().text = (System.Convert.ToInt32(ButtonManager.instance.bts[(int)animals[y, x].animalAnimal.animalType - 4].transform.Find("Text").GetComponent<Text>().text) - 1).ToString();
            }

            ScoreManager.instance.score += 100 * (int)(animals[y, x].animalAnimal.animalType - 3);
        }

        Animal tempAnimal = animals[y, x].animalAnimal;
        animals[y, x].animalAnimal = animals[my, mx].animalAnimal;
        animals[my, mx].animalAnimal = tempAnimal;

        int levelPlus = 0;
        if (animals[y, x].value == 0)
        {
            levelPlus = animals[y, x].animalAnimal.level;

            if (animals[y, x].animalAnimal.gameObject != null)
            {
                Destroy(animals[y, x].animalAnimal.gameObject, 0.3f);
                animals[y, x].tilepos = new Vector2(mx, my);
                animals[y, x].Move();
                animals[y, x].animalAnimal = null;
            }
            animals[my, mx].AnimalScaleAnimation(0.4f);
            animals[my, mx].animalAnimal.level += levelPlus;
        }

        if (animals[my, mx].value == 0)
        {
            levelPlus = animals[my, mx].animalAnimal.level;

            if (animals[my, mx].animalAnimal.gameObject != null)
            {
                Destroy(animals[my, mx].animalAnimal.gameObject, 0.3f);
                animals[my, mx].tilepos = new Vector2(x, y);
                animals[my, mx].Move();
                animals[my, mx].animalAnimal = null;
            }
            animals[y, x].AnimalScaleAnimation(0.4f);
            animals[y, x].animalAnimal.level += levelPlus;
        }
    }

    public void SwapAnimal(int x, int y, int mx, int my)
    {
        bool check = false;

        if (animals[y, x].animalAnimal.level == 4 && !animals[y, x].animalAnimal.초식 && (int)animals[y, x].animalAnimal.animalType < (int)AnimalType.Gorilla)
            check = true;

        Animal tempAnimal = animals[y, x].animalAnimal;
        animals[y, x].animalAnimal = animals[my, mx].animalAnimal;
        animals[my, mx].animalAnimal = tempAnimal;

        int levelPlus = 0;

        if (animals[y, x].value == 0)
        {
            levelPlus = animals[y, x].animalAnimal.level;

            if (animals[y, x].animalAnimal.gameObject != null)
            {
                Destroy(animals[y, x].animalAnimal.gameObject, 0.3f);
                animals[y, x].tilepos = new Vector2(mx, my);
                animals[y, x].Move();
                animals[y, x].animalAnimal = null;
            }
            animals[my, mx].AnimalScaleAnimation(0.4f);
            animals[my, mx].animalAnimal.level += 1;

            if (!animals[my, mx].animalAnimal.초식 && check)
            {
                //Destroy(animals[y, x].animalAnimal.gameObject);
                Destroy(animals[my, mx].animalAnimal.gameObject);
                UpgradeCheck(mx, my);
            }
        }

        if (animals[my, mx].value == 0)
        {
            levelPlus = animals[my, mx].animalAnimal.level;

            if (animals[my, mx].animalAnimal.gameObject != null)
            {
                Destroy(animals[my, mx].animalAnimal.gameObject, 0.3f);
                animals[my, mx].tilepos = new Vector2(x, y);
                animals[my, mx].Move();
                animals[my, mx].animalAnimal = null;
            }
            animals[y, x].AnimalScaleAnimation(0.4f);
            animals[y, x].animalAnimal.level += 1;

            if (!animals[y, x].animalAnimal.초식 && check)
            {
                Destroy(animals[y, x].animalAnimal.gameObject);
                //Destroy(animals[my, mx].animalAnimal.gameObject);
                UpgradeCheck(x, y);
            }
        }
    }

    public void UpgradeCheck(int ux, int uy)
    {
        if (mainCreateType == AnimalType.Gorilla)
            return;

        mainCreateType += 1; //최고보다 높을경우로 수정
        ButtonManager.instance.OnlySetListNumbers();
        animals[uy, ux].StartAnimalInit(mainCreateType - 5);

        if (!newAnimals[mainCreateType - 4])
        {
            newAnimals[mainCreateType - 4] = true;

            switch (mainCreateType)
            {
                case AnimalType.Squarle:
                    newAnimal = newAnimalFox;
                    newAnimalFox.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalFox, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Sheep:
                    newAnimal = newAnimalHyena;
                    newAnimalHyena.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalHyena, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Girrafe:
                    newAnimal = newAnimalLion;
                    newAnimalLion.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalLion, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Gorilla:
                    newAnimal = newAnimalBear;
                    newAnimalBear.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalBear, new Vector3(1, 1, 1), 0.7f);
                    break;
            }

            Invoke("OutNewAnimal", 3);
        }
    }

    void DecreaseMoveCount(int x, int y)
    {
        if (animals[y, x].animalAnimal != null)
        {
            if (animals[y, x].animalAnimal.초식)
            {
                if (animals[y, x].animalAnimal.animalType > mainCreateType)
                {
                    if (animals[y, x].animalAnimal.moveDistance > 0)
                        animals[y, x].animalAnimal.moveDistance -= 1;
                    else
                        return;
                }
            }
        }
    }

    void ResetMoveCount()
    {
        for (int y = 0; y < animalsy; y++)
        {
            for (int x = 0; x < animalsx; x++)
            {
                if (animals[y, x].animalAnimal != null)
                {
                    animals[y, x].animalAnimal.moveDistance = animals[y, x].animalAnimal.maxDistance;
                }
            }
        }
    }

    public void FirstAnimalCreate()
    {
        System.Random r = new System.Random();

        for (int i = 0; i < 1; i++)
        {
            int _x = 0, _y = 0;

            do
            {
                _x = r.Next(animalsx);
                _y = r.Next(animalsy);
            }
            while (animals[_y, _x].value != 0);

            animals[_y, _x].value = (int)r.Next(1, 3) * 2;

            if (i == 0)
                animals[_y, _x].StartAnimalInit();

            Invoke("NoCreateDeactive", 0.5f);
        }
        if (!newAnimals[mainCreateType - 4])
        {
            newAnimals[mainCreateType - 4] = true;

            switch (mainCreateType)
            {
                case AnimalType.Squarle:
                    newAnimal = newAnimalFox;
                    newAnimalFox.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalFox, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Sheep:
                    newAnimal = newAnimalHyena;
                    newAnimalHyena.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalHyena, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Girrafe:
                    newAnimal = newAnimalLion;
                    newAnimalLion.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalLion, new Vector3(1, 1, 1), 0.7f);
                    break;
                case AnimalType.Gorilla:
                    newAnimal = newAnimalBear;
                    newAnimalBear.transform.localScale = new Vector3(0, 0, 0);
                    iTween.ScaleTo(newAnimalBear, new Vector3(1, 1, 1), 0.7f);
                    break;
            }

            Invoke("OutNewAnimal", 3);
        }
    }

    // 한 턴이 끝날때마다 생성한다.
    // 게임 종료도 이곳에서 처리한다.
    public void AnimalCreate(int count)
    {
        int num1 = 0;

        if (!CountZero())
        {
            // 초식 동물을 생성한다.
            System.Random r = new System.Random();

            for (int i = 0; i < count; i++)
            {
                int _x = 0, _y = 0;
                do
                {
                    _x = r.Next(animalsx);
                    _y = r.Next(animalsy);
                }
                while (animals[_y, _x].value != 0);

                num1 = (int)mainCreateType;

                animals[_y, _x].value = (int)r.Next(1, 3) * 2;

                if (cheat)
                    mainCreateType = mainCreateType + 1;
                else
                    mainCreateType = (Random.Range(0, 10) != 0) ? (AnimalType)Random.Range(4, (int)mainCreateType + 1) : mainCreateType + 1;

                animals[_y, _x].AnimalInit();

                mainCreateType = (AnimalType)num1;

                Invoke("NoCreateDeactive", 0.5f);
            }
        }
        else
        {
            Invoke("GameOver", 0.5f);
        }

        Print();
    }

    void OutNewAnimal()
    {
        iTween.ScaleTo(newAnimal, new Vector3(0, 0, 0), 0.4f);
    }

    public bool CountZero()
    {
        int zero = 0;
        int zero2 = 0;

        for (int i = 0; i < animalsy; i++)
        {
            for (int j = 0; j < animalsx; j++)
            {
                if (animals[i, j].value == 0)
                    zero++;
                else if (animals[i, j].animalAnimal != null && !animals[i, j].animalAnimal.초식)
                    zero2++;
            }
        }

        if (zero2 == 0)
        {
            Invoke("GameOver", 0.5f);
        }

        return zero == 0 ? true : false;
    }

    void Gesture()
    {
        if (GunScript.instance.shootClick)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            onPos = Input.mousePosition;
            endPos = Input.mousePosition;
            gestureType = GestureType.NONE;
        }

        if (Input.GetMouseButtonUp(0) && gestureType == GestureType.NONE)
        {
            endPos = Input.mousePosition;

            float dist = Vector3.Distance(onPos, endPos);

            if (dist > 60)
            {
                if (!isNoCreate)
                {
                    isNoCreate = true;

                    // DOWN
                    if (endPos.x < onPos.x && endPos.y < onPos.y)
                        gestureType = GestureType.DOWN;
                    // UP
                    if (endPos.x > onPos.x && endPos.y > onPos.y)
                        gestureType = GestureType.UP;
                    // RIGHT
                    if (endPos.x > onPos.x && endPos.y < onPos.y)
                        gestureType = GestureType.RIGHT;
                    // LEFT
                    if (endPos.x < onPos.x && endPos.y > onPos.y)
                        gestureType = GestureType.LEFT;

                    Resetting();

                    AnimalCreate(1);

                    if(isZoom)
                    {
                        iTween.MoveTo(Camera.main.gameObject, zoomTargetAnimal.GetComponent<Animal>().masterPosition + new Vector3(5, 5, -5), 2.5f);
                        cameraTime = 0.0f;
                    }
                }
            }
            else
            {
                // // 오브젝트 클릭
                // 
                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(endPos);

                if (Physics.Raycast(ray.origin, ray.direction, out hit))
                {
                    if (hit.transform.CompareTag("Animal") || hit.transform.CompareTag("MainAnimal"))
                    {
                        Debug.Log(hit.transform.tag);

                        zoomTargetAnimal = hit.transform.gameObject;

                        iTween.MoveTo(Camera.main.gameObject, hit.transform.position + new Vector3(5, 5, -5), 2.5f);
                        isZoom = true;
                        cameraTime = 0.0f;
                    }
                }
                else
                {
                    iTween.MoveTo(Camera.main.gameObject, mainCameraOriginalPosition, 2.5f);
                    isZoom = false;
                    cameraTime = 0.0f;
                }


                if (isZoom)
                {
                    cameraTime += Time.deltaTime / 2.5f;
                    Camera.main.orthographicSize = Mathf.Lerp(2, 5, cameraTime);
                }
                else
                {
                    cameraTime += Time.deltaTime / 2.5f;
                    Camera.main.orthographicSize = Mathf.Lerp(5, 2, cameraTime);
                }
            }
        }
    }

    public void NoCreateDeactive()
    {
        isNoCreate = false;
    }

    public void GameOver()
    {
        print("Game Over");
        EventManager.instance.GameOver();
    }
}
