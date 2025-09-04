using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ЗагрузкаСцены : MonoBehaviour
{
    [SerializeField] private string LoadSceneName;
    [SerializeField] private string MenuSceneName;
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
    #endregion //Сист
    public void GoToGame()
    {
        SceneManager.LoadScene(MenuSceneName);
    }
    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
}

