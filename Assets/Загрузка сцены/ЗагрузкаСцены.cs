using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ЗагрузкаСцены : MonoBehaviour
{
    [SerializeField] private Animator componentAnimator;

    [SerializeField] private TextMeshProUGUI TextПрогресаЗагрузки;
    [SerializeField] private Image ПрогресБарЗагрузки;

    private AsyncOperation asyncOperation_ПрогрессЗагрузки;

    public static ЗагрузкаСцены Instance;//СинглТон (Singleton)
    public static bool ИгратьАнимациюПроявленияПриВыводеСцены = false;
    private static string ПереключаемаяСцена;

    private void Start()
    {
        Instance = this;
        if (componentAnimator == null)
            componentAnimator = GetComponent<Animator>();
        if (ИгратьАнимациюПроявленияПриВыводеСцены)
        {
            ИгратьАнимациюПроявленияПриВыводеСцены = false;
            Instance.componentAnimator.SetTrigger(name: "ЗагрузкаСцены_Конец");
        }
    }
    private float load_progress_cur = 0;
    private void Update()
    {
        if (asyncOperation_ПрогрессЗагрузки != null)
        {
            float progress = asyncOperation_ПрогрессЗагрузки.progress;
            if (asyncOperation_ПрогрессЗагрузки.isDone || progress >= 0.9f)
                progress = 1;
            load_progress_cur = Mathf.Lerp(load_progress_cur, progress, Time.deltaTime * 3);

            TextПрогресаЗагрузки.text = Mathf.RoundToInt(load_progress_cur * 100) + "%";
            ПрогресБарЗагрузки.fillAmount = load_progress_cur;
            if (load_progress_cur >= 0.9f)
            {
                asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;
                ИгратьАнимациюПроявленияПриВыводеСцены = true;
            }
            Debug.Log("ЗагрузкаСцены = "+ progress + "%");
        }
        else
        {
            TextПрогресаЗагрузки.text = "0%";
            ПрогресБарЗагрузки.fillAmount = 0f;
            load_progress_cur = 0f;
        }
    }
    public static void ПереключитьСценуАлинхронно(string sceneName)
    {
        Instance.componentAnimator.SetTrigger(name: "ЗагрузкаСцены_Начало");//Установить тригер запуска анимации затемнения экрана
        ПереключаемаяСцена = sceneName;

        //Instance.asyncOperation_ПрогрессЗагрузки =  SceneManager.LoadSceneAsync(sceneName);
        //Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться
        
    }
    //Вызывается когда закончилась анимация появления окна загрузки
    public void OnAnimatorOver()
    {
        asyncOperation_ПрогрессЗагрузки = SceneManager.LoadSceneAsync(ПереключаемаяСцена);
        Debug.Log("ЗагрузкаСцены = " + asyncOperation_ПрогрессЗагрузки.progress + "%");
        asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться

        //Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;//Разрешить переключение сцены
    }

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);
    public void LoadScene(int sceneIndex) => SceneManager.LoadScene(sceneIndex);
}


