using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalMainScene : MonoBehaviour
{
    public Button enterButton;

    // Start is called before the first frame update
    void Start()
    {
        enterButton.onClick.AddListener(OnEnterButtonClicked);
    }

    void OnEnterButtonClicked()
    {
        SceneManager.LoadScene(sceneName: "InstructionScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
