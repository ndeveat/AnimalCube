using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GunScript : MonoBehaviour
{
    public static GunScript instance;

    public Image toggle;
    public Text count;
    public Sprite shooting;
    public bool shootClick = false;
    public int numOfGuns;

    public GameObject shootImage;

    private Sprite nature;
    private Animal temp;

    bool isLastTouch;
    Vector3 lastTouch;

    private bool notTouch;

    void Awake()
    {
        instance = this;
        isLastTouch = false;
        nature = toggle.sprite;
    }

    void Update()
    {
        count.text = numOfGuns.ToString();

        if (numOfGuns < 1)
            return;

        if (!isLastTouch)
        {
            if (shootClick)
            {
                shootImage.transform.position = Input.mousePosition;

                if (Input.GetMouseButtonDown(0) && !notTouch)
                {
                    shootImage.SetActive(true);
                    AnimalManager.instance.gestureType = GestureType.GUN;

                    Vector3 touch = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(touch);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, Mathf.Infinity) && hit.transform.tag == "Animal")
                    {
                        lastTouch = Input.mousePosition;
                        isLastTouch = true;
                        shootClick = false;

                        shootImage.SetActive(true);
                        shootImage.transform.position = Input.mousePosition;
                        shootImage.transform.parent.parent.parent.parent.GetComponent<Animator>().Play("shoot");

                        temp = hit.transform.GetComponent<Animal>();

                        SoundManager.instance.ingame.Stop();
                        SoundManager.instance.highnoon.Play();

                    }
                }
            }
        }
    }


    public ParticleSystem blood;

    public void ShotAnimal()
    {
        AnimalManager.instance.gestureType = GestureType.NONE;

        shootImage.SetActive(true);
        shootImage.transform.position = lastTouch;

        SoundManager.instance.ingame.Play();
        SoundManager.instance.bang.Play();

        var tempAnimal = AnimalManager.instance.animals[(int)temp.matrixPos.y, (int)temp.matrixPos.x];

        var tempBlood = Instantiate(blood);
        tempBlood.transform.parent = tempAnimal.animalAnimal.transform.parent;
        tempBlood.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        tempBlood.transform.localPosition = tempAnimal.animalAnimal.transform.localPosition + new Vector3(0, 0.5f, 0);

        tempAnimal.value = 0;
        tempAnimal.Dead();

        Destroy(tempBlood.gameObject, tempBlood.startLifetime);

        notTouch = true;

        // Invoke("Reset", 1);
    }

    public void Reset()
    {
        isLastTouch = false;
        notTouch = false;
        numOfGuns -= 1;
        Destroy(AnimalManager.instance.animals[(int)temp.matrixPos.y, (int)temp.matrixPos.x].animalAnimal.gameObject);
        AnimalManager.instance.animals[(int)temp.matrixPos.y, (int)temp.matrixPos.x].animalAnimal = null;
        shootImage.SetActive(false);
        shootClickOn();
    }

    public void shootClickOn()
    {
        if (numOfGuns < 1)
        {
            shootClick = false;
            toggle.sprite = (shootClick) ? shooting : nature;
            return;
        }

        shootClick = !shootClick;
        shootImage.SetActive(shootClick);

        isLastTouch = false;


        toggle.sprite = (shootClick) ? shooting : nature;
    }
}
