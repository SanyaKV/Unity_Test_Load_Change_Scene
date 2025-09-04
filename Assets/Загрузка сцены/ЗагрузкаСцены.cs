using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ЗагрузкаСцены : MonoBehaviour
{
    [SerializeField] private string LoadSceneName;
    [SerializeField] private string ReturnSceneName;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != LoadSceneName)
        {
            SceneManager.LoadScene(LoadSceneName);
        }
    }
    public void GoToGame()
    {
        SceneManager.LoadScene(ReturnSceneName);
    }
}

