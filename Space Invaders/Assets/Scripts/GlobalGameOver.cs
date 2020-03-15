using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalGameOver : MonoBehaviour
{
    public Button backButton;
    public Text endGameStateText;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        Global global = GameObject.Find("Global").GetComponent<Global>();
        if (global.hasWon)
        {
            endGameStateText.text = "You won!";
        } else
        {
            endGameStateText.text = "You lost!";
        }
        scoreText.text = "Score: " + global.score.ToString();
        Destroy(global.gameObject);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    void OnBackButtonClicked()
    {
        SceneManager.LoadScene(sceneName: "MainScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
