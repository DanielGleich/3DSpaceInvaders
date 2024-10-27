using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    static GameManagerScript instance;

    [SerializeField] static int playerTries = 3;
    [SerializeField] float pauseTime;
    [SerializeField] bool isPaused = false;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] int playerScore = 0;

    public static GameManagerScript GetInstance() {  return instance; }

    static int GetPlayerTries() { 
        return playerTries;
    }

    static void SetPlayerTries(int playerTries)
    {
        GameManagerScript.playerTries = playerTries;
    }

    public bool GetIsPaused() { return isPaused; }
    public void HitPlayer() {
        playerTries--;
        if (playerTries > 0)
            StartCoroutine(PauseAfterHit());
        else
            GameOver();
    }

    IEnumerator PauseAfterHit() { 
        isPaused = true;
        yield return new WaitForSeconds(pauseTime);
        isPaused = false;
    }

    public void GameOver() {
        scoreText.text = "Score: " + playerScore;
        gameOverScreen.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    } 
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
