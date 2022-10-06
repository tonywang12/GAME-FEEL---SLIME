using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{   

    private bool grounded;
    public LayerMask groundLayer;
    public Joint2D joint;
    public Rigidbody2D body;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
                Debug.Log(grounded);

        grounded = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer);

        if(grounded)
        {
            joint.connectedBody = Physics2D.OverlapCircle(transform.position, 0.4f, groundLayer).GetComponent<Rigidbody2D>();
        } else
        {
             joint.connectedBody = body;
        }
        //joint.attachedRigidbody = 
    }
}
