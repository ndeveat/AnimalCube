using UnityEngine;
using System.Collections;
using System;

public class AnimalBlock : MonoBehaviour
{
    public int value;
    public Vector2 tilepos;

    [HideInInspector]
    public Animal animalAnimal = null;

    private float firstY;
    private bool firstAnim = true;

    private Vector3 firstScale;
    // =========

    void Awake()
    {
        animalAnimal = null;
        firstY = transform.position.y;
    }

    void Update()
    {
        if(firstAnim)
        {
            var vec3 = transform.position;
            vec3.y -= 0.5f * 3 * Time.deltaTime;
            transform.position = vec3;

            if(transform.position.y <= firstY)
            {
                transform.position = new Vector3(transform.position.x, firstY, transform.position.z);
                firstAnim = false;
            }
        }
    }

    public void Dead()
    {
        iTween.ScaleTo(animalAnimal.gameObject, new Vector3(0.0f, 0.0f, 0.0f), 0.25f);
    }

    public void SetGun(GameObject gun)
    {
        GameObject gb = Instantiate(gun);
        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.x -= 0.1f;
        pos.z = y;
        pos.y = 0.1f;
        gb.transform.position = pos;
    }

    public void StartAnimalInit()
    {
        switch (AnimalManager.instance.mainCreateType)
        {
            case AnimalType.Squarle:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Fox")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Fox;
                break;
            case AnimalType.Sheep:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Hyena")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Hyena;
                break;
            case AnimalType.Girrafe:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Lion")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Lion;
                break;
            case AnimalType.Gorilla:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Bear")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Bear;
                break;
            default:
                return;
        }
        
        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.z = y;
        pos.y = 0.1f;
        animalAnimal.transform.position = pos;
        animalAnimal.matrixPos = tilepos;
        firstScale = animalAnimal.transform.Find("3Dtext").localScale / 1.5f;
        animalAnimal.transform.localScale = Vector3.zero;
        AnimalScaleAnimation(0.5f);
    }

    public void StartAnimalInit(AnimalType animalType)
    {
        switch (animalType)
        {
            case AnimalType.Fox:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Hyena")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Hyena;
                break;
            case AnimalType.Hyena:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Lion")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Lion;
                break;
            case AnimalType.Lion:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Bear")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Bear;
                break;
            default:
                return;
        }

        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.z = y;
        pos.y = 0.1f;
        animalAnimal.transform.position = pos;
        animalAnimal.matrixPos = tilepos;
        animalAnimal.transform.localScale = Vector3.zero;
        AnimalScaleAnimation(0.5f);
    }

    public void AnimalInit()
    {
        switch (AnimalManager.instance.mainCreateType)
        {
            case AnimalType.Squarle:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Squarle")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Squarle;
                break;
            case AnimalType.Sheep:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Sheep")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Sheep;
                break;
            case AnimalType.Girrafe:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Giraffe")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Girrafe;
                break;
            case AnimalType.Gorilla:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Gorilla")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Gorilla;
                break;
            case AnimalType.Human:
                animalAnimal = ((GameObject)Resources.Load("Prefabs/Animals/Human")).GetComponent<Animal>();
                animalAnimal = Instantiate(animalAnimal);
                animalAnimal.animalType = AnimalType.Human;
                break;
                //추후 사람도 추가;
        }

        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.z = y;
        pos.y = 0.1f;

        animalAnimal.transform.position = pos;
        animalAnimal.matrixPos = tilepos;
        animalAnimal.transform.localScale = Vector3.zero;
        AnimalScaleAnimation(0.5f);
    }

    public void AnimalNameReset()
    {
        if (animalAnimal == null)
            return;
        animalAnimal.name = "animal_" + tilepos.x.ToString() + tilepos.y.ToString();
    }

    // Invoke로 실행됩니다.
    public void AnimalScaleAnimation(float time)
    {
        Invoke("ScaleAnimation", time);
    }

    void ScaleAnimation()
    {
        if (animalAnimal == null)
            return;

        try
        {
            if (animalAnimal.transform != null && animalAnimal != null && animalAnimal.gameObject != null)
            {
                animalAnimal.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                if (animalAnimal.animalType >= AnimalType.Girrafe)
                {
                    iTween.ScaleTo(animalAnimal.gameObject, new Vector3(0.6f, 0.6f, 0.6f), 0.6f);
                    animalAnimal.transform.Find("3Dtext").localScale = new Vector3(0.03f / 1.5f, 0.03f / 1.5f);
                }
                else
                    iTween.ScaleTo(animalAnimal.gameObject, new Vector3(0.4f, 0.4f, 0.4f), 0.6f);

            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }

    #region MOVE
    public void Move()
    { 
        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.z = y;
        pos.y = 0.1f;
        if (animalAnimal == null)
            return;
        AnimalNameReset();

        animalAnimal.masterPosition = pos;

        iTween.MoveTo(animalAnimal.gameObject, pos, 0.5f);
    }

    public void Move(Vector2 tilepos)
    {
        this.tilepos = tilepos;
        Vector3 pos = TileposToPosition(tilepos);
        float y = pos.y;
        pos.z = y;
        pos.y = 0.1f;
        if (animalAnimal == null)
            return;
        AnimalNameReset();
        iTween.MoveTo(animalAnimal.gameObject, pos, 0.5f);
    }
    #endregion

    #region TO
    public static Vector2 TileposToPosition(Vector2 pos)
    {
        return pos *= 0.52f;
    }

    public static Vector2 PositionToTilepos(Vector2 pos)
    {
        return pos /= 0.52f;
    }
    #endregion
}
