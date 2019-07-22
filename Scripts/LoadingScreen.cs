using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    // Make sure the loading screen shows for at least 1 second:
    //private const float MIN_TIME_TO_SHOW = 1f;
    // The reference to the current loading operation running in the background:
    //private AsyncOperation currentLoadingOperation;
    // A flag to tell whether a scene is being loaded or not:
    //private bool isLoading;
    // The rect transform of the bar fill game object:
    //[SerializeField] private RectTransform barFillRectTransform;
    // Initialize as the initial local scale of the bar fill game object. Used to cache the Y-value (just in case):
    //private Vector3 barFillLocalScale;
    // The text that shows how much has been loaded:
    //[SerializeField] private Text percentLoadedText;
    // The elapsed time since the new scene started loading:
    //private float timeElapsed;

    float counter = 0; // delay before launching the loading. If we don't use it, the scene begins to load before loadingScreen appears.

    public float Counter { get => counter; set => counter = value; }

    string sceneToLoad;

    private void Awake()
    {
        // Singleton logic:
        if (Instance == null)
        {
            Instance = this;
            // Don't destroy the loading screen while switching scenes:
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Save the bar fill's initial local scale:
        //barFillLocalScale = barFillRectTransform.localScale;
        Hide();
    }

    private void Update()
    {
        Counter += Time.fixedDeltaTime;
        if (counter > 0.1f)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Hide();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        counter = 0f;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //private void Update()
    //{
    //    if (isLoading)
    //    {
    //        // Get the progress and update the UI. Goes from 0 (start) to 1 (end):
    //        //SetProgress(currentLoadingOperation.progress);
    //        // If the loading is complete, hide the loading screen:
    //        if (currentLoadingOperation.isDone)
    //        {
    //            Hide();
    //        }

    //        //else
    //        //{
    //        //    timeElapsed += Time.deltaTime;
    //        //    if (timeElapsed >= MIN_TIME_TO_SHOW)
    //        //    {
    //        //        // The loading screen has been showing for the minimum time required.
    //        //        // Allow the loading operation to formally finish:
    //        //        currentLoadingOperation.allowSceneActivation = true;
    //        //    }
    //        //}
    //    }
    //}
    // Updates the UI based on the progress:
    //private void SetProgress(float progress)
    //{
    //    // Update the fill's scale based on how far the game has loaded:
    //    barFillLocalScale.x = progress;
    //    // Update the rect transform:
    //    barFillRectTransform.localScale = barFillLocalScale;
    //    // Set the percent loaded text:
    //    percentLoadedText.text = Mathf.CeilToInt(progress * 100).ToString() + "%";
    //}
    // Call this to show the loading screen.
    // We can determine the loading's progress when needed from the AsyncOperation param:
    public void Show(string sceneName)
    {
        // Enable the loading screen:
        gameObject.SetActive(true);
        sceneToLoad = sceneName;
        // Store the reference:
        //currentLoadingOperation = loadingOperation;
        // Stop the loading operation from finishing, even if it technically did:
        //currentLoadingOperation.allowSceneActivation = false;
        // Reset the UI:
        //SetProgress(0f);
        // Reset the time elapsed:
        //timeElapsed = 0f;
        //isLoading = true;
    }
    // Call this to hide it:
    public void Hide()
    {
        // Disable the loading screen:
        gameObject.SetActive(false);
        //currentLoadingOperation = null;
        //isLoading = false;
    }

}
