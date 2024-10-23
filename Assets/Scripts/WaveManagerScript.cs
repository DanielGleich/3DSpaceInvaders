using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum EnemyMoveDir { RIGHT, LEFT, DOWN };

public class WaveManagerScript : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Dictionary<Vector2, GameObject> enemyList = new Dictionary<Vector2, GameObject>();
    [SerializeField] Vector2 waveAnchor;
    [SerializeField] Vector2 waveSize;
    [SerializeField] Vector2 enemyOffset;
    [SerializeField] float stepCooldown;
    [SerializeField] float stepRange;
    EnemyMoveDir moveDir = EnemyMoveDir.RIGHT;

    [SerializeField] float enemyShootCooldown;
    bool isShootCooldown = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyList.Count == 0) { 
            NextWave();
        }

        if (!isShootCooldown)
            StartCoroutine(LetEnemyShoot());
    }

    void NextWave() {
        for (int x = 0; x < waveSize.x; x++)
        {
            for (int y = 0; y < waveSize.y; y++)
            {
                Vector3 spawnPoint = waveAnchor;
                spawnPoint.x += ((enemyPrefab.transform.localScale.x + enemyOffset.x) * x);
                spawnPoint.y -= ((enemyPrefab.transform.localScale.y + enemyOffset.y) * y);
                enemyList.Add(new Vector2(x,y), Instantiate(enemyPrefab, spawnPoint, Quaternion.identity));
            }
        }
    }

    IEnumerator LetEnemyShoot()
    {
        isShootCooldown = true;
        yield return new WaitForSeconds(enemyShootCooldown);

        List<KeyValuePair<Vector2, GameObject>> lowestEnemies = GetLowestEnemies();
        if (lowestEnemies.Count > 0) {
            int randomInt = ((int)Random.Range(0, lowestEnemies.Count));
            lowestEnemies[randomInt].Value.GetComponent<EnemyScript>().Shoot();
            Debug.Log(lowestEnemies.Count);
        }
        isShootCooldown = false;

    }

    List<KeyValuePair<Vector2, GameObject>> GetLowestEnemies()
    {
        List<KeyValuePair<Vector2, GameObject>> lowestEnemies = new List<KeyValuePair<Vector2, GameObject>>();
        int lowestRow = -1;
        foreach (KeyValuePair<Vector2, GameObject> enemy in enemyList)
        {
            if (enemy.Key.y > lowestRow)
            {
                lowestEnemies.Clear();
                lowestEnemies.Add(enemy);
                lowestRow = (int)enemy.Key.y;
            }
            else if (enemy.Key.y == lowestRow)
            {
                lowestEnemies.Add(enemy);
            }
        }
        return lowestEnemies;
    }

    List<KeyValuePair<Vector2, GameObject>> GetMostRightEnemies()
    {
        List<KeyValuePair<Vector2, GameObject>> rightEnemies = new List<KeyValuePair<Vector2, GameObject>>();
        int highestRow = -1;
        foreach (KeyValuePair<Vector2, GameObject> enemy in enemyList)
        {
            if (enemy.Key.y > highestRow)
            {
                rightEnemies.Clear();
                rightEnemies.Add(enemy);
                highestRow = (int)enemy.Key.x;
            }
            else if (enemy.Key.x == highestRow)
            {
                rightEnemies.Add(enemy);
            }
        }
        return rightEnemies;
    }

    bool isNextStepPossible() {

        switch (moveDir) {
            case EnemyMoveDir.RIGHT:

            break;

            case EnemyMoveDir.LEFT:
            break;

            case EnemyMoveDir.DOWN:

            break;
        }

        return true;
    }

    IEnumerator NextEnemyStep()
    {

        yield return new WaitForSeconds(stepCooldown);
    }
}
