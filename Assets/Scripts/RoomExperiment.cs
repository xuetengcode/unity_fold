using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
//using System.Threading;
using Unity.XR.CoreUtils;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExperiment : MonoBehaviour
{
    FadeInOut fade;
    // scene setup
    [SerializeField] private GameObject _stand;
    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private float _timeLimit = 30f;
    public GameObject _cameraL;
    // different gains
    public float adaptation_gain;

    private float timer = 0f;
    //public float exp_gain = 1f; // pass to ApplyGain.cs

    List<int> LocalConditions = LaunchUI.SharedConditions;
    private int Apressed;
    private int Bpressed;
    private int Xpressed;
    private int Ypressed;
    
    private int LastA = 0;
    private int LastB = 0;
    private int LastX = 0;
    private int LastY = 0;

    void Start()
    {
        LastA = GetComponent<DataInput>().bttnApressed;
        LastB = GetComponent<DataInput>().bttnBpressed;
        LastX = GetComponent<DataInput>().bttnXpressed;
        LastY = GetComponent<DataInput>().bttnYpressed;

        fade = GetComponentInChildren<FadeInOut>();
        if (LaunchUI.SharedCounters[0] == 1)
        {
            _timeLimit = 10* 60f;
        }
        else
        {
            _timeLimit = 2 * 60f;
        }

        if (LocalConditions[1] == 0)
        {
            adaptation_gain = 0.5f;
        }
        else
        {
            adaptation_gain = 2f;
        }
        Debug.Log("Adaptation gain is set as'" + adaptation_gain + "'.");

        // set initial location
        _xrOrigin.transform.position = _stand.transform.position;

        // bino or mono
        /* 
         * if do this way, cameraL initially has to be enabled in the scene 
         * thus, do a direct link is better
         */
        //GameObject _leftCam = GameObject.Find("CameraL"); 
        //if (_leftCam != null )
        
        if (LocalConditions[0] == 0)
        {
            // bino
            _cameraL.SetActive(false);
        }
        else
        {
            // mono
            _cameraL.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Apressed = GetComponent<DataInput>().bttnApressed;
        Bpressed = GetComponent<DataInput>().bttnBpressed;
        Xpressed = GetComponent<DataInput>().bttnXpressed;
        Ypressed = GetComponent<DataInput>().bttnYpressed;

        timer += Time.deltaTime;
        float seconds = timer % 10;

        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        
        if (Input.GetKeyDown(KeyCode.Escape) | Xpressed > LastX)
        {
            Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");

            LaunchUI.SharedCounters[1] += 1;
            Debug.Log("Active Scene is '" + scene.name + "'.");
            //StartCoroutine(_ChangeScene(scene));
            // Use a coroutine to load the Scene in the background
            StartCoroutine(LoadYourAsyncScene(scene));
        }
        if (seconds > _timeLimit)
        {
            Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");

            LaunchUI.SharedCounters[1] += 1;
            Debug.Log("Time out and jump to next scene");
            //SceneManager.LoadScene(scene.buildIndex + 1);
            StartCoroutine(LoadYourAsyncScene(scene));
        }
        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
    }

    IEnumerator _ChangeScene(UnityEngine.SceneManagement.Scene scene)
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(scene.buildIndex + 1);
    }
    IEnumerator LoadYourAsyncScene(UnityEngine.SceneManagement.Scene scene)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        fade.FadeIn();
        yield return new WaitForSeconds(1);
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.buildIndex + 1);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

}
