using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ЗагрузкаСцены : MonoBehaviour
{
    [SerializeField] private Animator componentAnimator;

    [SerializeField] private TextMeshProUGUI TextПрогресаЗагрузки;
    [SerializeField] private Image ПрогресБарЗагрузки;
    [SerializeField] private string MainSceneName;

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
        {//Для сцены в которую загрузились
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
            load_progress_cur = Mathf.Lerp(load_progress_cur, progress, Time.deltaTime * 1);

            int progress_proc = Mathf.RoundToInt(load_progress_cur * 100);

            TextПрогресаЗагрузки.text = progress_proc + "%";
            ПрогресБарЗагрузки.fillAmount = load_progress_cur;
            if (progress_proc >= 100)
            {
                Debug.Log("ЗагрузкаСцены = Ок");
                asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;
                ИгратьАнимациюПроявленияПриВыводеСцены = true;
            }
            Debug.Log("ЗагрузкаСцены = "+ progress + "%"+" /"+ load_progress_cur);
        }
        else
        {
            TextПрогресаЗагрузки.text = "0%";
            ПрогресБарЗагрузки.fillAmount = 0f;
            load_progress_cur = 0f;
        }
    }
    public  void ПереключитьСценуАcинхронноДомой() => ПереключитьСценуАcинхронно(MainSceneName);
    public static void ПереключитьСценуАcинхронно(string sceneName)
    {
        Instance.componentAnimator.SetTrigger(name: "ЗагрузкаСцены_Начало");//Установить тригер запуска анимации затемнения экрана
        ПереключаемаяСцена = sceneName;

        Instance.asyncOperation_ПрогрессЗагрузки =  SceneManager.LoadSceneAsync(sceneName);
        Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться
        
    }
    //Вызывается когда закончилась анимация появления окна загрузки
    public void АнимацияНачалаЗагрузкиКончилась()
    {
        //asyncOperation_ПрогрессЗагрузки = SceneManager.LoadSceneAsync(ПереключаемаяСцена);
        //asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться

        //Debug.Log("ЗагрузкаСцены = " + asyncOperation_ПрогрессЗагрузки.progress + "%");

        //Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;//Разрешить переключение сцены
    }
}


