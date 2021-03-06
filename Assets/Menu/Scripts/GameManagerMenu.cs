﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class that manages the main menu functions.
/// </summary>
public class GameManagerMenu : MonoBehaviour
{
    #region Variables
    public static GameManagerMenu managerMenu;

    [Header("Panels")]
    [SerializeField] GameObject[] panels = null;

    [Header("Games")]
    [SerializeField] GameObject[] games = null;
    int activeGame = 0;

    [Header("Volume")]
    int volume;
    [SerializeField] Text volumeText = null;
    [SerializeField] GameObject volumeLeftArrow = null;
    [SerializeField] GameObject volumeRightArrow = null;

    [Header("Region")]
    [SerializeField] Text regionText = null;
    [SerializeField] GameObject[] regions = null;
    int activeRegionButton = 0;
    #endregion

    private void Awake()
    {
        managerMenu = this;
    }

    private void Start()
    {
        CheckVolume();

        UpdateRegionButton();

        if (PlayerPrefs.GetInt("FirstTime") == 0)
        {
            OpenPanel(panels[5]);
        }
    }

    /// <summary>
    /// Function called to load a new game (scene).
    /// </summary>
    /// <param name="buildIndex">Number of the scene to be loaded.</param>
    public void LoadGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Function called to load a random game.
    /// </summary>
    public void LoadRandomGame()
    {
        SceneManager.LoadScene(Random.Range(1, SceneManager.sceneCountInBuildSettings));
    }

    /// <summary>
    /// Function used to navigate between the main menu panels.
    /// </summary>
    /// <param name="panel">The panel to open.</param>
    public void OpenPanel(GameObject panel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        
        panel.SetActive(true);
    }

    /// <summary>
    /// Function that changes the active game.
    /// </summary>
    public void ArrowGames(bool leftArrow)
    {
        if (leftArrow)
        {
            activeGame -= 1;

            if (activeGame < 0)
            {
                activeGame = games.Length - 1;
            }
        }

        else
        {
            activeGame += 1;

            if (activeGame >= games.Length)
            {
                activeGame = 0;
            }
        }

        for (int i = 0; i < games.Length; i++)
        {
            games[i].SetActive(false);
        }

        games[activeGame].SetActive(true);
    }

    /// <summary>
    /// Close the game completely.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Function called to check the volume at the start of the game.
    /// </summary>
    void CheckVolume()
    {
        LoadOptions();

        AudioListener.volume = (volume / 100f);

        if (volume > 0)
        {
            volumeText.text = volume.ToString() + "%";

            if (volume >= 100)
            {
                volumeRightArrow.SetActive(false);
            }
        }

        else
        {
            volumeLeftArrow.SetActive(false);
            volumeText.text = "OFF";
        }
    }

    /// <summary>
    /// Function called to modify the volume of the game.
    /// </summary>
    /// <param name="leftArrow">True if we are lowering the volume.</param>
    public void VolumeManager(bool leftArrow)
    {
        if (leftArrow)
        {
            volume -= 5;
            AudioListener.volume = (volume / 100f);
            volumeRightArrow.SetActive(true);

            if (volume > 0)
            {
                volumeText.text = volume.ToString() + "%";
            }

            else
            {
                volumeLeftArrow.SetActive(false);
                volumeText.text = "OFF";
            }
        }

        else
        {
            volume += 5;
            AudioListener.volume = (volume / 100f);
            volumeLeftArrow.SetActive(true);
            volumeText.text = volume.ToString() + "%";


            if (volume >= 100)
            {
                volumeRightArrow.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Function that loads the options from the PlayerPrefs.
    /// </summary>
    void LoadOptions()
    {
        int soundVolumeLoaded = PlayerPrefs.GetInt("GameVolume", 100);
        volume = soundVolumeLoaded;
    }

    /// <summary>
    /// Function that saves the options in the PlayerPrefs.
    /// </summary>
    public void SaveOptions()
    {
        PlayerPrefs.SetInt("GameVolume", volume);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Function that allows changing the language from the options menu.
    /// </summary>
    /// <param name="newLanguage">The code of the language that we want to activate.</param>
    public void ChangeLanguage(string newLanguage)
    {
        MultilanguageManager.multilanguageManager.ChangeLanguage(newLanguage);
    }

    /// <summary>
    /// Function to delete all saved high scores.
    /// </summary>
    public void ClearHighScores()
    {
        PlayerPrefs.SetInt("HighScore1", 0);
        PlayerPrefs.SetInt("HighScore2-1", 0);
        PlayerPrefs.SetInt("HighScore2-2", 0);
        PlayerPrefs.SetInt("HighScore3", 0);
        PlayerPrefs.SetInt("HighScore4", 0);
        PlayerPrefs.SetInt("HighScore5", 0);
        PlayerPrefs.SetInt("HighScore6", 0);
        PlayerPrefs.SetInt("HighScore7", 0);
        PlayerPrefs.SetInt("HighScore8", 0);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Function called to update the server region in the PlayerPrefs.
    /// </summary>
    /// <param name="token">The region code.</param>
    public void UpdateRegion(string token)
    {
        PlayerPrefs.SetString("ActiveRegion", token);
        PlayerPrefs.Save();

        UpdateRegionButton();
    }

    /// <summary>
    /// Function that updates the text of the active region accordingly.
    /// </summary>
    void UpdateRegionButton()
    {
        string activeRegion = PlayerPrefs.GetString("ActiveRegion", "eu");

        switch (activeRegion)
        {
            case "asia":
                regionText.text = "Asia";
                break;
            case "au":
                regionText.text = "Australia";
                break;
            case "cae":
                regionText.text = "Canada East";
                break;
            case "eu":
                regionText.text = "Europe";
                break;
            case "in":
                regionText.text = "India";
                break;
            case "jp":
                regionText.text = "Japan";
                break;
            case "rue":
                regionText.text = "Russia East";
                break;
            case "ru":
                regionText.text = "Russia West";
                break;
            case "za":
                regionText.text = "South Africa";
                break;
            case "sa":
                regionText.text = "South America";
                break;
            case "kr":
                regionText.text = "South Korea";
                break;
            case "us":
                regionText.text = "USA East";
                break;
            case "usw":
                regionText.text = "USA West";
                break;
        }
    }

    /// <summary>
    /// Function to scroll through the menu of available regions.
    /// </summary>
    /// <param name="leftArrow">True if we press the left arrow button, false if we press the right.</param>
    public void ArrowRegions(bool leftArrow)
    {
        if (leftArrow)
        {
            activeRegionButton -= 1;

            if (activeRegionButton < 0)
            {
                activeRegionButton = regions.Length - 1;
            }
        }

        else
        {
            activeRegionButton += 1;

            if (activeRegionButton >= regions.Length)
            {
                activeRegionButton = 0;
            }
        }

        for (int i = 0; i < regions.Length; i++)
        {
            regions[i].SetActive(false);
        }

        regions[activeRegionButton].SetActive(true);
    }
}
