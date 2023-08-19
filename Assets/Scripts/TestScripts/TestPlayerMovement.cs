using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovement : MonoBehaviour
{
    private Rigidbody playerBody;


    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVect;

        movementVect = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        playerBody.velocity = new Vector3(movementVect.x * 7, playerBody.velocity.y, movementVect.y * 7);

        if (Input.GetKey(KeyCode.J))
        {
            transform.Rotate(0,1,0);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(0, -1, 0);
        }
    }
}
