using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    public InputField nameInputField;
    public Button startButton;

    void Start()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    void OnStartButtonClick()
    {
        string playerName = nameInputField.text;
        PlayerData.Instance.SetPlayerName(playerName); // Store the player name
        SceneManager.LoadScene("main"); // Load the main game scene
    }
}
