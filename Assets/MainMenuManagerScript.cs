using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public void LoadGame() {
        SceneManager.LoadScene(1);
    }
}
