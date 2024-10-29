using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{

    #region Singleton
    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    #endregion

    static GameManagerScript instance;

    private void OnDestroy()
    {
        instance = null;
    }


    [SerializeField] static int playerTries = 3;
    [SerializeField] float pauseTime;
    [SerializeField] bool isPaused = false;
    [SerializeField] List<Image> uiHearts = new List<Image>();
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject gameOverlay;
    [SerializeField] TextMeshProUGUI gameOverScoreText;
    [SerializeField] TextMeshProUGUI gameplayScoreText;
    [SerializeField] int playerScore = 0;

    public static GameManagerScript GetInstance() {  return instance; }

    static int GetPlayerTries() { 
        return playerTries;
    }

    static void SetPlayerTries(int playerTries)
    {
        GameManagerScript.playerTries = playerTries;
    }

    public void AddPoints(int points) { playerScore += points; }

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
        gameOverScoreText.text = "Score: " + playerScore;
        gameOverScreen.SetActive(true);
        gameOverlay.SetActive(false);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    } 

    private void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < uiHearts.Count; i++)
        {
            uiHearts[i].gameObject.SetActive(i < playerTries);
        }
        gameplayScoreText.text = playerScore.ToString();
    }
}
