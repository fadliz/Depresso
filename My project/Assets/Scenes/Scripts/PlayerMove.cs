using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private float fallThreshold;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider2d;
    private Animator anim;
    private bool falling;
    private bool tofall;

    private void Awake()
    {
        //grab reference to rigidbody & animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //horizontal input move rigidbody times speed
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        if (body.velocity.y < -fallThreshold)
        {
            body.velocity = new Vector2(body.velocity.x, -fallThreshold);
        }


        //check direction, bigger than float 0.01 is right direction
        if (horizontalInput > 0.01f)
        {
            transform.localScale = Vector3.one;
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        //jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("falling", isFalling());
        anim.SetBool("tofall", tofall);
    }

    private bool isFalling()
    {
        if (!isGrounded() && (body.velocity.y > -1.5f && body.velocity.y < 1.5f))
        {
            tofall = true;
            
        }
        if (!isGrounded() && body.velocity.y < -0.5f)
        {
            falling = true;
            tofall = false;
        }
        else
        {
            falling = false;
        }
        return falling;
    }

        private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private bool isGrounded()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, platformLayerMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
        } 
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (boxCollider2d.bounds.extents.x), rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }
}
