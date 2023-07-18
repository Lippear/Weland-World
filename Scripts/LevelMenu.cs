using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _toMenuButton;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _currentLevelMenu;
    [SerializeField] private TMP_Text _wonOrLostString;

    private void Awake()
    {
        _pauseButton.onClick.AddListener(SetPause);
        _continueButton.onClick.AddListener(UnPause);
        _restartButton.onClick.AddListener(Restart);
        _toMenuButton.onClick.AddListener(ToMenu);
        Character.CharacterDead += SetWonOrLostMenu;
    }

    private void OnDestroy()
    {
        Character.CharacterDead -= SetWonOrLostMenu;
    }

    private void SetWonOrLostMenu(string tag)
    {
        SetPause();
        _continueButton.gameObject.SetActive(false);
        if(tag == "Player")
        {
            _wonOrLostString.text = "YOU LOST";
        }
        else
        {
            _wonOrLostString.text = "YOU WON";
        }
    }

    private void SetPause()
    {
        _pauseMenu.SetActive(true);
        _currentLevelMenu.SetActive(false);
        Time.timeScale = 0;
    }

    private void UnPause()
    {
        _pauseMenu.SetActive(false);
        _currentLevelMenu.SetActive(true);
        Time.timeScale = 1;
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    private void ToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
