using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] Vector3 projectileOffset;
    [SerializeField] int pointValue;
    [SerializeField] GameObject explosionPrefab;
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
        GameManagerScript.GetInstance().AddPoints(pointValue);
        Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void MoveStep(Vector3 pos) { 
        transform.position = transform.position + pos;
    }

    public void Shoot() {
        Instantiate(projectile, rb.transform.position + projectileOffset, projectile.transform.rotation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Shield")) { 
            Destroy(collision.gameObject);
        }
    }

}
