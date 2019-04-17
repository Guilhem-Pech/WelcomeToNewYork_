using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float horizontalSpeed = 40.0f;
    public float verticalSpeed = 40.0f;
    // Update is called once per frame
    public float speed = 5.0f;
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            //transform.rotation = (0, speed * Time.deltaTime, 0);
            transform.Rotate(0,90,0, Space.World);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Rotate(1, 0, 0, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-1, 0, 0, Space.World);
        }

        if (Input.GetKey(KeyCode.Z))
        {
            transform.position += new Vector3(0, speed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.X))
        {
            transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.KeypadPlus))
        {
            speed += 0.3f;
        }
        if (Input.GetKey(KeyCode.KeypadMinus))
        {
            speed -= 0.3f;
        }
    }
}
