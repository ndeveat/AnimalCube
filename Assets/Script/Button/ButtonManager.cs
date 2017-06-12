using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;

    public Image[] bts;
    public string[] wanted_lists; // 동물이 얼마나 필요한가

    AnimalType lastType;
    float timer = 1.0f;

    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(AnimalManager.instance != null)
            ChangeColor();

        if (CountTexts())
            SetListNumbers();
    }

    public void SetListNumbers()
    {
        for (int j = 0; j < wanted_lists[(int)AnimalManager.instance.mainCreateType - 4].ToCharArray().Length; j++)
        {
            bts[j].transform.Find("Text").GetComponent<Text>().text = wanted_lists[(int)AnimalManager.instance.mainCreateType - 4][j].ToString();

            if (System.Convert.ToInt32(bts[j].transform.Find("Text").GetComponent<Text>().text) == 0)
                bts[j].transform.Find("Text").GetComponent<Text>().text = "";
        }

        AnimalManager.instance.FirstAnimalCreate();
    }

    public void OnlySetListNumbers()
    {
        for (int j = 0; j < wanted_lists[(int)AnimalManager.instance.mainCreateType - 4].ToCharArray().Length; j++)
        {
            bts[j].transform.Find("Text").GetComponent<Text>().text = wanted_lists[(int)AnimalManager.instance.mainCreateType - 4][j].ToString();

            if (System.Convert.ToInt32(bts[j].transform.Find("Text").GetComponent<Text>().text) == 0)
                bts[j].transform.Find("Text").GetComponent<Text>().text = "";
        }
    }

    bool CountTexts()
    {
        int count = 0;

        for (int j = 0; j < wanted_lists[(int)AnimalManager.instance.mainCreateType - 4].Length; j++)
        {
            if (bts[j].transform.Find("Text").GetComponent<Text>().text != "" && System.Convert.ToInt32(bts[j].transform.Find("Text").GetComponent<Text>().text) > 0)
            {
                count++;
            }
        }
        
        if (count <= 0)
            return true;

        return false;
    }

    void ChangeColor()
    {
        for (int i = 0; i < (int)AnimalManager.instance.mainCreateType - 3; i++)
        {
            bts[i].transform.Find("Image").gameObject.SetActive(true);
            bts[i].transform.Find("Lock").gameObject.SetActive(false);

            if (i == 3)
            {
                bts[i + 1].transform.Find("Image").gameObject.SetActive(true);
                bts[i + 1].transform.Find("Lock").gameObject.SetActive(false);
            }
        }

        lastType = AnimalManager.instance.mainCreateType;
    }
}
