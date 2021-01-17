using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;
using System.Collections;

public class UIController : MonoBehaviour
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
    public static UIController Instance;
    public int enemyCount;
    public Slider mapSlider;
    public Joystick joystick;
    public GameObject powerBar;

    public void Awake()
    {
        Instance = this;
        DefaultLayout();
        AddButonListeners();
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
        EventManager.OnLevelStart.AddListener(OnLevelStart);
        EventManager.OnLevelFailed.AddListener(OpenLostPanel);
        EventManager.OnFinishLine.AddListener(OnFinishLine);
        EventManager.OnLevelSuccess.AddListener(LevelSucces);
    }
    private void OnDisable()
    {
        EventManager.OnEnemyDie.RemoveListener(UpdateTheSlider);
        EventManager.OnLevelStart.RemoveListener(OnLevelStart);
        EventManager.OnLevelFailed.RemoveListener(OpenLostPanel);
        EventManager.OnFinishLine.RemoveListener(OnFinishLine);
        EventManager.OnLevelSuccess.RemoveListener(LevelSucces);
    }
    private void OnLevelStart()
    {
        gameplayInfo.Close();
        mapSlider.maxValue = enemyCount;
    }

    private void LevelSucces() 
    {
        gamePlay.Close();
        Win.Open();
    }

    private void OnFinishLine() 
    {        
        mapSlider.gameObject.SetActive(false);
        powerBar.gameObject.SetActive(true);
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
        mapSlider.gameObject.SetActive(true);
        powerBar.gameObject.SetActive(false);
        gamePlay.Open();
        Win.Close();
        Lose.Close();
        Pause.Close();
        gameplayInfo.Open();
        ResetValues();        
    }
    private void AddButonListeners()
    {
        pauseButton.onClick.AddListener(PauseTheGame);
        nextButton.onClick.AddListener(NextLevel);
        restartButton.onClick.AddListener(FailedLevel);
        playButton.onClick.AddListener(ResumeGame);
        replayButton.onClick.AddListener(RestartLevel);
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
        DefaultLayout();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(2));        
        GameManager.Instance.StartGame();
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