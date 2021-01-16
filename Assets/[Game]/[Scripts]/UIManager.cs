using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button pauseButton;
    public Button nextButton;
    public Button restartButton;
    public Button playButton;
    public Button replayButton;
    [Header("Panels")]
    public CanvasGroup gamePlay;
    public CanvasGroup Win;  // Açılması İçin Ayarlanmadı Karakter Bitişe Vardığında ---> UIManager.Won.Invoke(); ' u çağırınız
    public CanvasGroup Lose; // Açılması İçin Ayarlanmadı Karakter Öldüğü Zaman      ---> UIManager.Lost.Invoke(); ' u çağırınız
    public CanvasGroup Pause;
    public CanvasGroup gameplayInfo;
    public static UIManager instance;
    public int enemyCount;
    public Slider mapSlider;
    public Joystick joystick;

    public void Awake()
    {
        instance = this;
        DefaultLayout();
        AddButonListeners();
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneLoadStatement.isLoaded);
        GunCameraController.Instance.joystick = joystick;
        mapSlider.maxValue = enemyCount;
    }
    public void Assignments()
    {
        GunCameraController.Instance.joystick = joystick;
        mapSlider.maxValue = enemyCount;
    }
    public void ResetValues()
    {
        enemyCount = 0;
        mapSlider.maxValue = 0;
        mapSlider.value = 0;
    }
    public void OnEnable()
    {
        EventManager.OnEnemyDie.AddListener(UpdateTheSlider);
        EventManager.OnTapStart.AddListener(CloseTheHelpMenu);
        EventManager.OnLevelFailed.AddListener(OpenLostPanel);
    }
    private void CloseTheHelpMenu()
    {
         gameplayInfo.Close(); EventManager.OnLevelStart.Invoke();
    }
    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(UpdateTheSlider);
        EventManager.OnTapStart.RemoveListener(CloseTheHelpMenu);
        EventManager.OnLevelFailed.RemoveListener(OpenLostPanel);


    }
    public void UpdateTheSlider()
    {
        mapSlider.value++;
    }
    // Related Panels Pause-Resume
    public void PauseTheGame()
    {
        Time.timeScale = 0;
        gamePlay.Close();
        Pause.Open();
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Pause.Close();
        gamePlay.Open();
    }
    // Half Related Panels NextLevel-RestartLevel-Level Failed
    public void NextLevel()
    {
        int i = PlayerPrefs.GetInt("Level");
        PlayerPrefs.SetInt("Level", i + 1);
        ReloadScene();
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        ReloadScene();

    }
    public void FailedLevel()
    {
        Time.timeScale = 1;
        ReloadScene();
    }
    // Methods
    private void DefaultLayout()
    {
        Time.timeScale = 1;
        gamePlay.Open();
        Win.Close();
        Lose.Close();
        Pause.Close();
        gameplayInfo.Open();
        EventManager.OnTapStart.AddListener(() => { gameplayInfo.Close(); });
    }
    private void AddButonListeners()
    {
        pauseButton.onClick.AddListener(PauseTheGame);
        nextButton.onClick.AddListener(NextLevel);
        restartButton.onClick.AddListener(FailedLevel);
        playButton.onClick.AddListener(ResumeGame);
        replayButton.onClick.AddListener(RestartLevel);
    }
    public void OpenWonPanel()
    {
        gamePlay.Close();
        Win.Open();
    }
    public void OpenLostPanel()
    {
        gamePlay.Close();
        Lose.Open();
    }
    public void ReloadScene()
    {
        StartCoroutine(LevelButtonCo());
    }
    IEnumerator LevelButtonCo()
    {
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(2));
        DefaultLayout();
    }
}
public static class CanvasProp
{
    public static void Close(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    public static void Open(this CanvasGroup canvasGroup)
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}