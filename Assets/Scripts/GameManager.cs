using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject restartLevelUI;
    public void restartGame()
    {
        restartLevelUI.SetActive(true);
        Invoke("restart", 1f);
    }

    void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
