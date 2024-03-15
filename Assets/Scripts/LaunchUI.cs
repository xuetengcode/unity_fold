using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Button ViewingBBttn;
    [SerializeField]
    private Button ViewingMBttn;

    [SerializeField]
    private Button Adaptation1Bttn;
    [SerializeField]
    private Button Adaptation2Bttn;
    [SerializeField]
    private Button Adaptation3Bttn;

    [SerializeField]
    private Button ConfirmBttn;
    [SerializeField] private AudioClip _click;

    private string playerName = "";
    public static List<int> SharedConditions = new List<int> { 0, 1};
    public static List<int> SharedCounters = new List<int> { 0, 0 }; // number of visits to room and fold
    public static int curr_exp = 0; 

    public static List<float> all_distance = new List<float> { 1.5f };
    public static List<float> all_width = new List<float> { 1f }; //{ 1f, 1.125f, 1.25f};
    public static List<float> gains_both = new List<float>();
    public static List<float> gains_expansive = new List<float> { 2f };
    public static List<float> gains_compressive = new List<float> { 0.667f };
    public static int all_reverse = 0;

    public FadeInOut fade;

    private void Awake()
    {
        ViewingBBttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            Debug.Log("Bino button clicked.");
            SharedConditions[0] = 0;
        });

        ViewingMBttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            Debug.Log("Mono button clicked.");
            SharedConditions[0] = 1;
        });

        Adaptation1Bttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            Debug.Log("Adaptation 1 button clicked.");
            SharedConditions[1] = 0;
        });

        Adaptation2Bttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            Debug.Log("Adaptation 2 button clicked.");
            SharedConditions[1] = 1;
        });
        Adaptation3Bttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            Debug.Log("Adaptation 2 button clicked.");
            SharedConditions[1] = 2;
        });

        // support buttons
        ConfirmBttn.onClick.AddListener(() =>
        {
            SoundFXManager.Instance.PlaySoundFXClip(_click, transform, 1f);
            SharedCounters[0] += 1;
            Debug.Log("Confirm button clicked.");
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            //SceneManager.LoadScene(scene.buildIndex + 1);
            StartCoroutine(_ChangeScene(scene.buildIndex + 1));
        });
    }

    public void ReadStringInput(string s)
    {
        playerName = s;
        Debug.Log(playerName);
    }

    private void Start()
    {
        fade = GetComponentInChildren<FadeInOut>();
    }
    public IEnumerator _ChangeScene(int nextIdx)
    {
        //fade.fadein = true;
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextIdx);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
