using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Vector3 projectileOffset;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die() { 
        Destroy(gameObject);
    }

    public void MoveStep(Vector3 pos) { 
        transform.position = pos;
    }

    public void Shoot() {
        Instantiate(projectile, rb.transform.position + projectileOffset, Quaternion.identity);
    }

}
