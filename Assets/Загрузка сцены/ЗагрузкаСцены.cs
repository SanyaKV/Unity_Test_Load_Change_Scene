using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ЗагрузкаСцены : MonoBehaviour
{
    [SerializeField] private string LoadSceneName;
    [SerializeField] private string MenuSceneName;
    [SerializeField] private Animator componentAnimator;
    #region Сист
    #region СинглТон
    public static ЗагрузкаСцены Instance;//СинглТон (Singleton)

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    #endregion //СинглТон
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != LoadSceneName)
        {
            SceneManager.LoadScene(LoadSceneName);
        }
    }
    private void Start()
    {
        if (componentAnimator == null)
            componentAnimator = GetComponent<Animator>();
    }
    #endregion //Сист
    private AsyncOperation asyncOperation_ПрогрессЗагрузки;
    public static void ПереключитьСценуАлинхронно(string sceneName)
    {
        Instance.componentAnimator.SetTrigger(name: "SceneLoad_Open");
        Instance.asyncOperation_ПрогрессЗагрузки =  SceneManager.LoadSceneAsync(sceneName);
        Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false;//После загрузки сцены сразу на ней не переключаться
        //12:00
    }
    public void GoToGame()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
}

