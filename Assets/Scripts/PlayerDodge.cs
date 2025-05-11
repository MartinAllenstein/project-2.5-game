using System.Collections;
using UnityEngine;

public class PlayerDodge : MonoBehaviour
{
    public float dodgeSpeed = 10f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;

    private bool isDodging = false;
    private float dodgeTimer;
    private Vector3 lastMoveDirection;
    void Update()
    {
        dodgeTimer -= Time.deltaTime;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = new Vector3(h, 0, v).normalized;

        if (inputDirection.magnitude > 0f)
        {
            lastMoveDirection = inputDirection;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && dodgeTimer <= 0f && !isDodging)
        {
            StartCoroutine(Dodge());
        }
    }

    IEnumerator Dodge()
    {
        isDodging = true;
        dodgeTimer = dodgeCooldown;

        float elapsed = 0f;
        while (elapsed < dodgeDuration)
        {
            transform.position += lastMoveDirection * dodgeSpeed * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDodging = false;
    }
}
