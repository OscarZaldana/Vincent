using UnityEngine;
using System.Collections;

public class CrayonSpinner : MonoBehaviour {

    public float moveSpeed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, Time.deltaTime * moveSpeed);
        //transform.Rotate(0, 0, Time.deltaTime * 25, Space.World);
    }
}
