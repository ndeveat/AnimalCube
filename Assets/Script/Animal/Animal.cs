using UnityEngine;
using System.Collections;

public class Animal : MonoBehaviour {

    public AnimalType animalType;
    [HideInInspector]
    public AnimalType lastType;
    public bool 초식 = false;
    public int power;
    public int level = 1;
    public int moveDistance = 0;
    [HideInInspector]
    public int maxDistance = 0;

    public Vector2 matrixPos;

    public Vector3 masterPosition;

    public TextMesh levelText;
    void Awake ()
    {
        power = (int)animalType;
        lastType = animalType;
        maxDistance = moveDistance;
        levelText = transform.Find("3Dtext").GetComponent<TextMesh>();
    }

    void Update()
    {
        if (level >= 4)
            level = 4;

        levelText.text = "Lv " + level.ToString();
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Gun") && !초식)
        {
            GunScript.instance.numOfGuns += 1;
            Destroy(col.gameObject);
        }
    }
}
