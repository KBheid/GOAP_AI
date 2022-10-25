using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float movementSpeed = 25f;
    public float upSpeed = 10f;

    public float pitchSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        
        float xMove = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float zMove = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        float yMove = 0;
        if (Input.GetKey(KeyCode.Space))
            yMove = upSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftShift))
            yMove = - upSpeed * Time.deltaTime;

        transform.position += new Vector3(xMove, yMove, zMove);

        float pitch = 0;
        if (Input.GetKey(KeyCode.DownArrow))
            pitch = pitchSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow))
            pitch = -pitchSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x + pitch, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
