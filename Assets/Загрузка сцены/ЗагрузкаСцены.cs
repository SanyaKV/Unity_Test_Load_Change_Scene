
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ЗагрузкаСцены : MonoBehaviour
{
    /*
     * Идея в следующем:
     * Когда начинается смена сцены:
     * 1 запускается анимация затемнения "ЗагрузкаСцены_Начало" ф-я ПереключитьСценуАcинхронно
     * 2 в конце анимации вызываеися ф-я АнимацияНачалаЗагрузкиКончилась
     * 3 Открывается окно с продолжением загрузки
     * 4 после окончания управление передается новому окну asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;
     *  и устанавливается static ИгратьАнимациюПроявленияПриВыводеСцены = true; переменная статическая и доступна во всех экзкмпляраз класса
     * 5 Новая сцена стартует, если стоит ИгратьАнимациюПроявленияПриВыводеСцены то играет анимацию проявления         
     */
    [SerializeField] private Animator componentAnimator;

    [SerializeField] private TextMeshProUGUI TextПрогресаЗагрузки;
    [SerializeField] private Image ПрогресБарЗагрузки;
    [SerializeField] private GameObject Фон;
    [SerializeField] private GameObject БлокЗагрузки;
    [SerializeField] private string MainSceneName;

    private AsyncOperation asyncOperation_ПрогрессЗагрузки;

    public static ЗагрузкаСцены Instance;//СинглТон (Singleton)
    public static bool ИгратьАнимациюПроявленияПриВыводеСцены = false;
    private static string ПереключаемаяСцена;

    private void Start()
    {
        Time.timeScale = 0;
        componentAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        Instance = this;
        if (componentAnimator == null)
            componentAnimator = GetComponent<Animator>();
        if (ИгратьАнимациюПроявленияПриВыводеСцены)
        {//Для сцены в которую загрузились
            ИгратьАнимациюПроявленияПриВыводеСцены = false;
            Instance.componentAnimator.SetTrigger(name: "ЗагрузкаСцены_Конец");
        }
        else
        {
            Фон.SetActive(false);
            БлокЗагрузки.SetActive(false);
        }
    }
    public static void ПереключитьСценуДомой() => ПереключитьСцену(Instance.MainSceneName);
    //public IEnumerator Fade()
    //{
    //    Time.timeScale
    //    yield break;
    //}

    //public void ПереключитьСценуАcинхронно(string sceneName)
    public static void ПереключитьСцену(string sceneName)
    {
        Instance.componentAnimator.SetTrigger(name: "ЗагрузкаСцены_Начало");//Установить тригер запуска анимации затемнения экрана
        Instance.componentAnimator.playbackTime = 0;
        ПереключаемаяСцена = sceneName;

        //Instance.asyncOperation_ПрогрессЗагрузки = SceneManager.LoadSceneAsync(sceneName);
        //Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться        
    }
    //Вызывается когда закончилась анимация появления окна загрузки
    public void АнимацияНачалаЗагрузкиКончилась()
    {
        asyncOperation_ПрогрессЗагрузки = SceneManager.LoadSceneAsync(ПереключаемаяСцена);
        asyncOperation_ПрогрессЗагрузки.allowSceneActivation = false; //После загрузки сцены сразу на неё не переключаться

        //Debug.Log("ЗагрузкаСцены = " + asyncOperation_ПрогрессЗагрузки.progress + "%");
        //Instance.asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;//Разрешить переключение сцены
    }
    private float load_progress_view = 0;
    private void Update()
    {
        if (asyncOperation_ПрогрессЗагрузки != null)
        {
            float progress = asyncOperation_ПрогрессЗагрузки.progress;
            if (asyncOperation_ПрогрессЗагрузки.isDone || progress >= 0.9f)
                progress = 1;
            load_progress_view = Mathf.Lerp(load_progress_view, progress, Time.unscaledDeltaTime * 1);

            int progress_proc = Mathf.RoundToInt(load_progress_view * 100);

            TextПрогресаЗагрузки.text = progress_proc + "%";
            ПрогресБарЗагрузки.fillAmount = load_progress_view;
            if (progress_proc >= 100)
            {
                //Debug.Log("ЗагрузкаСцены = Ок");
                asyncOperation_ПрогрессЗагрузки.allowSceneActivation = true;
                ИгратьАнимациюПроявленияПриВыводеСцены = true;
            }
            //Debug.Log("ЗагрузкаСцены = "+ progress + "%"+" / "+ load_progress_cur);
        }
        else
        {
            TextПрогресаЗагрузки.text = "0%";
            ПрогресБарЗагрузки.fillAmount = 0f;
            load_progress_view = 0f;
        }
    }
}


