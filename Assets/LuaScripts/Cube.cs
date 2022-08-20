using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;


public class Cube : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    [LuaCallCSharp]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector3.up * 500);
        }
    }
}
