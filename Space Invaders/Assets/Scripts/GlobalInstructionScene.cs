using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalInstructionScene : MonoBehaviour
{
    public Button startGameButton;

    // Start is called before the first frame update
    void Start()
    {
        startGameButton.onClick.AddListener(OnStartGameButtonClicked);
    }

    void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene(sceneName: "GameplayScene");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
