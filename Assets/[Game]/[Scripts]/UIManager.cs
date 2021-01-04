using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    public CanvasGroup Win;
    public CanvasGroup Lose;
    public CanvasGroup Pause;
    public CanvasGroup gameplayInfo;

    public void Awake()
    {
        DefaultLayout();
        AddButonListeners();
    }
    public void PauseTheGame()
    {
        Time.timeScale = 0;
        gamePlay.Close();
        Pause.Open();

    }
    public void NextLevel()
    {
        //
    }
    public void RestartLevel() 
    {
        SceneManager.LoadScene( SceneManager.GetActiveScene().buildIndex );
        Time.timeScale = 1;
    }
    public void ResumeGame()
    {
        Time.timeScale=1;
        Pause.Close();
        gamePlay.Open();
    }

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
        restartButton.onClick.AddListener(RestartLevel);
        playButton.onClick.AddListener(ResumeGame);
        replayButton.onClick.AddListener(RestartLevel);
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