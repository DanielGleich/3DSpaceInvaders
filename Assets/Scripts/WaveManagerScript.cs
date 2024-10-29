using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyMoveDir { RIGHT, LEFT, DOWN };

public class WaveManagerScript : MonoBehaviour
{
    [SerializeField] bool debugPlayField;
    [SerializeField] bool debugSteps;
    [SerializeField] Material debugMat;
    [SerializeField] Rect playField;
    [SerializeField] GameObject enemyPrefab;
    public static Dictionary<Vector2, GameObject> enemyList = new Dictionary<Vector2, GameObject>();
    [SerializeField] Vector2 waveAnchor;
    [SerializeField] Vector2 waveSize;
    [SerializeField] Vector2 enemyOffset;
    [SerializeField] float stepCooldown;
    [SerializeField] float stepRange;

    [SerializeField] float gameOverHeight;

    GameManagerScript gameManager;
    bool isStepinProgress = false;

    EnemyMoveDir currentMoveDir = EnemyMoveDir.RIGHT;
    EnemyMoveDir LastHorizontalMoveDir;

    [SerializeField] float enemyShootCooldown;
    bool isShootCooldown = false;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManagerScript.GetInstance();
    }

    void DebugPlayField() {
        Debug.DrawLine(new Vector3(playField.x, playField.y), new Vector3(playField.x+playField.width, playField.y), Color.red);
        Debug.DrawLine(new Vector3(playField.x + playField.width, playField.y), new Vector3(playField.x + playField.width, playField.y-playField.height), Color.red);
        Debug.DrawLine(new Vector3(playField.x + playField.width, playField.y - playField.height), new Vector3(playField.x, playField.y - playField.height), Color.red);
        Debug.DrawLine(new Vector3(playField.x, playField.y - playField.height), new Vector3(playField.x, playField.y), Color.red);
    }

    void DebugSteps() { 
        foreach (KeyValuePair<Vector2, GameObject> enemy in enemyList) {
            Debug.DrawLine(enemy.Value.transform.position, enemy.Value.transform.position - new Vector3(stepRange, 0, 0));
            Debug.DrawLine(enemy.Value.transform.position, enemy.Value.transform.position + new Vector3(stepRange, 0, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (debugPlayField) DebugPlayField();
        if (debugSteps) DebugSteps();

        if (enemyList.Count == 0) { 
            NextWave();
        }

        if (!isStepinProgress && !gameManager.GetIsPaused())
            StartCoroutine(NextEnemyStep());

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
                GameObject enemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
                enemy.GetComponent<EnemyScript>().id = new Vector2(x, y);
                enemyList.Add(new Vector2(x,y), enemy);
            }
        }
    }

    IEnumerator LetEnemyShoot()
    {
        isShootCooldown = true;
        yield return new WaitForSeconds(enemyShootCooldown);

        List<KeyValuePair<Vector2, GameObject>> lowestEnemies = GetLowestEnemies();
        if (lowestEnemies.Count > 0 && !gameManager.GetIsPaused()) {
            int randomInt = ((int)Random.Range(0, lowestEnemies.Count));
            lowestEnemies[randomInt].Value.GetComponent<EnemyScript>().Shoot();
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
            if (enemy.Key.x > highestRow)
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

    List<KeyValuePair<Vector2, GameObject>> GetMostLeftEnemies()
    {
        List<KeyValuePair<Vector2, GameObject>> leftEnemies = new List<KeyValuePair<Vector2, GameObject>>();
        int lowestRow = (int)waveSize.x+1;
        foreach (KeyValuePair<Vector2, GameObject> enemy in enemyList)
        {
            if (enemy.Key.x < lowestRow)
            {
                leftEnemies.Clear();
                leftEnemies.Add(enemy);
                lowestRow = (int)enemy.Key.x;
            }
            else if (enemy.Key.x == lowestRow)
            {
                leftEnemies.Add(enemy);
            }
        }
        return leftEnemies;
    }

    void SetMoveDirection()
    {

        switch (currentMoveDir)
        {
            case EnemyMoveDir.RIGHT:
                List<KeyValuePair<Vector2, GameObject>> rightEnemies = GetMostRightEnemies();
                if (rightEnemies.Count > 0)
                {
                    if (rightEnemies[0].Value.gameObject.transform.position.x+stepRange >= (playField.x + playField.width))
                    {
                        LastHorizontalMoveDir = EnemyMoveDir.RIGHT;
                        currentMoveDir = EnemyMoveDir.DOWN;
                    }
                }
                else
                    Debug.LogError("Es gibt keine Gegner auf der rechten Seite");
                break;

            case EnemyMoveDir.LEFT:
                List<KeyValuePair<Vector2, GameObject>> leftEnemies = GetMostLeftEnemies();
                if (leftEnemies.Count > 0)
                {
                    if (leftEnemies[0].Value.gameObject.transform.position.x-stepRange <= playField.x) {
                        LastHorizontalMoveDir = EnemyMoveDir.LEFT;
                        currentMoveDir = EnemyMoveDir.DOWN;
                    }
                }
                else
                    Debug.LogError("Es gibt keine Gegner auf der rechten Seite");
                break;

            case EnemyMoveDir.DOWN:
                currentMoveDir = (LastHorizontalMoveDir == EnemyMoveDir.RIGHT) ? EnemyMoveDir.LEFT : EnemyMoveDir.RIGHT;
            break;
        }
    }

    IEnumerator NextEnemyStep()
    {
        isStepinProgress = true;
        SetMoveDirection();
        Vector3 step = Vector3.zero;

        switch (currentMoveDir)
        {
            case EnemyMoveDir.LEFT:
                step.x = -stepRange;
                break;

            case EnemyMoveDir.RIGHT:
                step.x = stepRange;
                break;

            case EnemyMoveDir.DOWN:
                step.y = -stepRange;
                break;
        }

        for (int y = (int)waveSize.y - 1; y >= 0; y--)
        {
            while (gameManager.GetIsPaused()) {
                yield return new WaitForSeconds(.1f);
            }
            yield return new WaitForSeconds(stepCooldown);
            for (int x = 0; x < waveSize.x; x++)
            {
                if (enemyList.ContainsKey(new Vector2(x, y)))
                {
                    enemyList[new Vector2(x, y)].GetComponent<EnemyScript>().MoveStep(step);
                    if (enemyList[new Vector2(x, y)].transform.position.y <= gameOverHeight)
                    {
                        gameManager.GameOver();
                        break;
                    }
                }
            }
        }
        isStepinProgress = false;
    }

    private void OnDestroy()
    {
        enemyList.Clear();
        StopAllCoroutines();
    }
}
