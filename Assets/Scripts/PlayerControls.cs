using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private int speed;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private LayerMask grassLayer;
    [SerializeField] private int stapsInGrass;
    [SerializeField] private int minStapsToEncounter;
    [SerializeField] private int maxStapsToEncounter;
    
    private Rigidbody rb;
    private Vector3 movement;
    private bool movingInGrass;
    private float stapTimer;
    private int stapsToEncounter;

    //Animation Parameters
    private const string IS_WALK_PARAM = "IsWalk";
    private const string IS_JUMP_PARAM = "IsJump";

    //Scenes
    private const string BATTEN_SCENE = "BattleScene"; 
    
    private const float TIMERPERSTEP = 0.5f;

    private void Awake()
    {
        CalaculateStapsToNextEncounter();
    }

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

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1, grassLayer);
        movingInGrass = colliders.Length != 0 && movement != Vector3.zero;

        if (movingInGrass == true)
        {
            stapTimer += Time.fixedDeltaTime;
            if (stapTimer >= TIMERPERSTEP)
            {
                stapsInGrass++;
                stapTimer = 0;

                if (stapsInGrass >= stapsToEncounter)
                {
                    SceneManager.LoadScene(BATTEN_SCENE);
                }
            }
        }
    }

    private void CalaculateStapsToNextEncounter()
    {
        stapsToEncounter = Random.Range(minStapsToEncounter, maxStapsToEncounter);
    }
}