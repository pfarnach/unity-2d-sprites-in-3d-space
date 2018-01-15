using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterMovement : MonoBehaviour
{
    
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private LayerMask collisionMask;

    [SerializeField]
    private float skinWidth = 0.1f;


    private BoxCollider boxCollider;
    //private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool isFlipped = false;


    void Awake()
    {
        //body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
    }

    void Move ()
    {
        // Adapted from https://www.reddit.com/r/Unity2D/comments/2f497k/zeldafinalfantasy_like_top_down_movement/
        Vector3 dir = new Vector3(
            Input.GetAxisRaw("Horizontal"), 
            0f, 
            Input.GetAxisRaw("Vertical")
        ).normalized * moveSpeed * Time.deltaTime;

        // Set animation
        anim.SetBool("isWalking", dir != Vector3.zero);

        DoFlip(dir);
        DoMove(dir);
    }

    void DoFlip (Vector3 dir)
    {
        // Flip character
        if (dir.x < 0 && !isFlipped)
        {
            isFlipped = true;
            Vector3 newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;
        }

        if (dir.x > 0 && isFlipped)
        {
            isFlipped = false;
            Vector3 newScale = transform.localScale;
            newScale.x = -newScale.x;
            transform.localScale = newScale;
        }
    }

    void DoMove(Vector3 dir)
    {
        RaycastHit hit;

        // Vertical movement
        Physics.BoxCast(
            boxCollider.bounds.center,
            (boxCollider.size / 2) - (Vector3.one * skinWidth),
            transform.forward * Mathf.Sign(dir.z),
            out hit,
            Quaternion.identity,
            skinWidth * 2,
            collisionMask
        );

        if (hit.collider == null)
            transform.Translate(0, 0, dir.z);
        else
            Debug.Log(hit.collider.name);


        // Horizontal movement
        Physics.BoxCast(
            boxCollider.bounds.center,
            (boxCollider.size / 2) - (Vector3.one * skinWidth),
            transform.right * Mathf.Sign(dir.x),
            out hit,
            Quaternion.identity,
            skinWidth * 2,
            collisionMask
        );

        Debug.DrawRay(transform.position,transform.right,Color.red);

        if (hit.collider == null)
            transform.Translate(dir.x, 0, 0);
    }

}
