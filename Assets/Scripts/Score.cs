using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private Stack stack;
    private float score;
    // Update is called once per frame

    private void Start()
    {
        stack = FindObjectOfType<Stack>();
        textMesh = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        score = stack.getScoreCount();
        textMesh.text = "score " + score;
    }
}