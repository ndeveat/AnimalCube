using UnityEngine;
using System.Collections;

public class AnimalTrail : MonoBehaviour
{
    public Transform[] points;
    public float speed;

    private Vector3[] native;
    private Vector3[] lastVec;
    [SerializeField]
    private bool[] right;
	void OnEnable()
    {
        native = new Vector3[transform.childCount];
        lastVec = new Vector3[transform.childCount];
        right = new bool[transform.childCount];

        for (int i = 0; i < native.Length; i++)
        {
            native[i] = transform.GetChild(i).localPosition;
            lastVec[i] = native[i];
        }
    }
	
	void Update ()
    {
        for (int i = 0; i < native.Length; i++)
        {
            transform.GetChild(i).transform.Translate((right[i]) ? Vector3.right * speed * Time.deltaTime : Vector3.left * speed * Time.deltaTime);

            if ((right[i]) ? transform.GetChild(i).localPosition.x > Screen.width * 0.5f + 100 : transform.GetChild(i).localPosition.x < -Screen.width * 0.5f - 100)
            {
                transform.GetChild(i).localPosition = new Vector3(transform.GetChild(i).localPosition.x, transform.GetChild(i).localPosition.y - 200);

                right[i] = !right[i];
                lastVec[i] = transform.GetChild(i).localPosition;

                if(transform.GetChild(i).localPosition.y < -1200)
                    ResetPosition();
            }
        }

    }

    void ResetPosition()
    {
        for (int i = 0; i < native.Length; i++)
        {
            transform.GetChild(i).localPosition = native[i];
            right[i] = false;
        }
    }

    void OnDisable()
    {
        ResetPosition();
    }
}
