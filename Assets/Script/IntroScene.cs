using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    public Text text;
    public float timer = 0.2f;

    string str;
    string temp;
    int i = 0;

    void Awake()
    {
        Screen.SetResolution(542, 966, false);

        str = text.text;
        text.text = "";

        StartCoroutine(TextTyper());
    }

    public void SceneChange()
    {
        SceneManager.LoadScene("2048");
    }

    IEnumerator TextTyper()
    {
        temp += str[i];

        text.text = temp;

        yield return new WaitForSeconds(timer);

        i++;

        if(text.text == str)
        {
            yield return new WaitForSeconds(timer * 3);

            text.text = "";
            temp = "";
            i = 0;
        }

        StartCoroutine(TextTyper());
    }
}
