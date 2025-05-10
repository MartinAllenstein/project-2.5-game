using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;

    private Rigidbody rb;
    private Vector3 movement;

    //Animation Parameters
    private const string IS_WALK_PARAM = "IsWalk";
    private const string IS_JUMP_PARAM = "IsJump";
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        
        movement = new Vector3(x, 0, z).normalized;
        
        animator.SetBool(IS_WALK_PARAM, movement != Vector3.zero);

        //Flip Player
        if (x != 0 && x != 0)
        {
            playerSprite.flipX = true;
        }

        if (x != 0 && x > 0)
        {
            playerSprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + movement * speed * Time.fixedDeltaTime);
    }
}