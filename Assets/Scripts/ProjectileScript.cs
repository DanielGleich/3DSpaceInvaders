using UnityEngine;

public enum DIR { UP, DOWN };
public class ProjectileScript : MonoBehaviour
{
    [SerializeField] DIR dir;
    Vector3 moveDir;
    [SerializeField] float speed;
    Rigidbody rb;
    bool hasCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (dir == DIR.UP)
        {
            moveDir = new Vector3(0, 1, 0);
        }
        else
        {
            moveDir = new Vector3(0, -1, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        rb.transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasCollided) // Make sure that Projectile only destroys a single shield block
        {
            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Border"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Shield")) {
            hasCollided = true;
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
        else if (dir == DIR.UP && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            other.gameObject.GetComponent<EnemyScript>().Die();
            Destroy(gameObject);
        }
        else if (dir == DIR.DOWN && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hasCollided = true;
            GameManagerScript.GetInstance().HitPlayer();
            Destroy(gameObject);
        }
    }
}
