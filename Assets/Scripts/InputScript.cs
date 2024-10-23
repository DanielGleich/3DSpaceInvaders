using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float maxVel;
    Vector3 velRef;
    Vector3 velStandByRef;
    [SerializeField] float shootCooldown;
    [SerializeField] bool isWaitingForShoot = false;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Vector3 projectilePositionOffset;

    Rigidbody rb;
    [SerializeField] Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
            if (rb.velocity.x < -maxVel)
            {
                rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(-maxVel, rb.velocity.y, 0), ref velRef, .1f);
                animator.SetBool("MovingLeft", true);
                animator.SetBool("MovingRight", false);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
            if (rb.velocity.x > maxVel)
            {
                rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(maxVel, rb.velocity.y, 0), ref velRef, .1f);
                animator.SetBool("MovingLeft", false);
                animator.SetBool("MovingRight", true);
            }
        }
        else { 
                rb.velocity = Vector3.SmoothDamp(rb.velocity, new Vector3(0, rb.velocity.y, 0), ref velStandByRef, .1f);
            animator.SetBool("MovingLeft", false);
            animator.SetBool("MovingRight", false);
        }

        
        
        

        ScreenWrapping();

        if (Input.GetKeyDown(KeyCode.Space) && !isWaitingForShoot)
        {
           StartCoroutine(ShootProjectile());
        }
    }

    void ScreenWrapping() {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        float rightSideScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        float leftSideScreen = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;

        if (screenPos.x <= 0 && Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector2(rightSideScreen, rb.transform.position.y);
        }

        if (screenPos.x >= Screen.width && Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector2(leftSideScreen, rb.transform.position.y);
        }
    }

    IEnumerator ShootProjectile() {
        isWaitingForShoot = true;
        Instantiate(projectilePrefab, rb.transform.position + projectilePositionOffset, Quaternion.identity);
        yield return new WaitForSeconds(shootCooldown);
        isWaitingForShoot = false;
    }
}
