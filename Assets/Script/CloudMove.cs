using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour
{
    public float speed;

	void Update ()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        if(transform.localPosition.x < -780)
        {
            transform.localPosition = new Vector3(780, transform.localPosition.y);
        }
	}
}
