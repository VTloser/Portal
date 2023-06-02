using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(-Vector3.forward * Time.deltaTime * 5);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * 5);
        }
    }
}
