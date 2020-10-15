using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour
{
    public GameObject StartUI;
    public GameObject button;
    public void Startgame()
    {
        button.SetActive(false);
        StartUI.SetActive(true);
        Invoke("startInvoke", 1f);
    }

    void startInvoke()
    {
        SceneManager.LoadScene("StackUp");
    }
}