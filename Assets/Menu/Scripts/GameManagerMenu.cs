using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManagerMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] GameObject[] panels = null;

    [Header("Options")]
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] Slider soundSlider = null;
    float soundVolume;
    
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        LoadOptions();
    }

    private void Update()
    {
        soundVolume = soundSlider.value;

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(soundVolume) * 20);
    }

    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadRandomGame()
    {
        SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings));
    }

    public void OpenPanel(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        
        panel.SetActive(true);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void LoadOptions()
    {
        if (PlayerPrefs.HasKey("GameVolume"))
        {
            float soundVolumeLoaded = PlayerPrefs.GetFloat("GameVolume");
            soundSlider.value = soundVolumeLoaded;
        }
    }

    public void SaveOptions()
    {
        PlayerPrefs.SetFloat("GameVolume", soundVolume);
        PlayerPrefs.Save();
    }

    public void ClearHighScores()
    {
        PlayerPrefs.SetInt("HighScore1", 0);
        PlayerPrefs.SetInt("HighScore2-1", 0);
        PlayerPrefs.SetInt("HighScore2-2", 0);
        PlayerPrefs.SetInt("HighScore3", 0);
        PlayerPrefs.Save();
    }
}
