using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float shootCooldown;
    [SerializeField] bool isWaitingForShoot = false;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Vector3 projectilePositionOffset;

    Rigidbody rb;


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
            rb.MovePosition(rb.transform.position + new Vector3(-moveSpeed, 0, 0) * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rb.MovePosition(rb.transform.position + new Vector3(moveSpeed, 0, 0) * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isWaitingForShoot)
        {
           StartCoroutine(ShootProjectile());
        }
    }

    IEnumerator ShootProjectile() {
        isWaitingForShoot = true;
        Instantiate(projectilePrefab, rb.transform.position + projectilePositionOffset, Quaternion.identity);
        yield return new WaitForSeconds(shootCooldown);
        isWaitingForShoot = false;
    }
}
