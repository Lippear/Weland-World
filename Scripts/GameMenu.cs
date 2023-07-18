using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _quitButton; 

    private void OnEnable()
    {
        _playButton.onClick.AddListener(LoadLevel1);
        _quitButton.onClick.AddListener(QuitGame);
    }

    private void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
