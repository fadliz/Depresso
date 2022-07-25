using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
/*     // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } */

    [SerializeField] private float speed;
    //[SerializeField] private float maxspeed;
    [SerializeField] private float fallThreshold;
    private Rigidbody2D body;
    private Animator anim;
    private bool grounded;
    private bool falling;
    private int countCol;

    private void Awake()
    {
        //grab reference to rigidbody & animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        //horizontal input move rigidbody times speed
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);


        //check direction, bigger than float 0.01 is right direction
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            Jump();

        if (!grounded && body.velocity.y < -0.01f)
            falling = true;

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        anim.SetBool("falling", falling);
        Debug.Log(countCol);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed * 2);
        anim.SetTrigger("jump");
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Ground")
            countCol++;
            grounded = true;
            falling = false;
    }

    private void OnCollisionExit2D(Collision2D collision) 
    {
        if(collision.gameObject.tag == "Ground" )
            if(countCol > 1)
                countCol--;
            
            if(countCol == 1)
                grounded = false;
    }

/*     private bool isGrounded() {
       return transform.Find("GroundCheck").GetComponent<GroundCheck>().isGrounded;
    } */
}
