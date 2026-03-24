using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    public GameObject mainMenuPanel;

    [Header("Settings UI")]
    public GameObject settingsPanel;
    public Slider volumeSlider;

    void Start()
    {
        volumeSlider.value = AudioManager.Instance.masterVolume;

        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // Make sure we start on the main menu
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnPlayPressed()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnSettingsPressed()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnSettingsBackPressed()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void OnVolumeChanged(float value)
    {
        AudioManager.Instance.masterVolume = value;
        AudioManager.Instance.musicSource.volume = value;
        AudioListener.volume = value;
    }
}