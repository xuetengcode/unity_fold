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
    private Button ConfirmBttn;
    [SerializeField] private AudioClip _click;

    private string playerName = "";
    public static List<int> SharedConditions = new List<int> { 0, 1};
    public static List<int> SharedCounters = new List<int> { 0, 0 }; // number of visits to room and fold

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
