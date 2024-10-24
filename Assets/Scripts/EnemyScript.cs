using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Vector3 projectileOffset;
    public Vector2 id;
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
        WaveManagerScript.enemyList.Remove(id);
        Destroy(gameObject);
    }

    public void MoveStep(Vector3 pos) { 
        transform.position = transform.position + pos;
    }

    public void Shoot() {
        Instantiate(projectile, rb.transform.position + projectileOffset, Quaternion.identity);
    }

}
