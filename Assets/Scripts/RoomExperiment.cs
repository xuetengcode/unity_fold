using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
//using System.Threading;
using Unity.XR.CoreUtils;
//using UnityEditor.SearchService;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.PlayerLoop;

public class RoomExperiment : MonoBehaviour
{
    FadeInOut fade;
    // scene setup
    //[SerializeField] private GameObject _stand;
    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private float _timeLimit = 30f;
    public GameObject _cameraL;
    [SerializeField] private GameObject MainCamera;
    public bool save_file = true;

    private float adaptation_gain;
    private float tester = DropDownControl.playerName;
    private string tester_str;
    private string viewing;
    private string resultFileName;
    private string update_once;

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

    public bool _collideNext = false;

    public bool _expReady = false;
    private Vector3 lastTrackedPosition;
    private Transform cameraTransform;
    private int post = 0;
    private int hold = 0;
    private bool nextTriggered = false;
    void Start()
    {
        LastA = DataInput.bttnApressed;
        LastB = DataInput.bttnBpressed;
        LastX = DataInput.bttnXpressed;
        LastY = DataInput.bttnYpressed;
        // bino or mono
        if (LocalConditions[0] == 0)
        {
            // bino
            viewing = "bino";
        }
        else
        {
            // mono
            viewing = "mono";
        }

        if (tester < 10)
        {
            tester_str = $"0{tester}";
        }
        else
        {
            tester_str = $"{tester}";
        }
        fade = GetComponentInChildren<FadeInOut>();
        if (LaunchUI.SharedCounters[0] == 1)
        {
            _timeLimit = 10 * 60f;
        }
        else
        {
            _timeLimit = 2 * 60f;
        }

        if (LocalConditions[1] == 0)
        {
            adaptation_gain = 0.5f;
        }
        else if (LocalConditions[1] == 1)
        {
            adaptation_gain = 1f;
        }
        else
        {
            adaptation_gain = 2f;
        }
        Debug.Log("Adaptation gain is set as'" + adaptation_gain + "'.");

        // set initial location
        //_xrOrigin.transform.position = _stand.transform.position;
        //_xrOrigin.transform.rotation = _stand.transform.rotation;
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

        _expReady = true;

        cameraTransform = _xrOrigin.Camera.transform;
        lastTrackedPosition = cameraTransform.localPosition;

        if (save_file)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/output/");
            DateTime currentDateTime = DateTime.Now;
            //string dateString = currentDateTime.ToString("yyyyMMddHHmmss");
            string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
            // name_gain_yyyy-mm-dd_tt-tt-tt(24h)_viewing
            int round_id = LaunchUI.SharedCounters[0];
            resultFileName = Application.persistentDataPath + "/output/" + tester_str + "_" + adaptation_gain + "_" + dateString + "_" + viewing + "_room_" + round_id + "_head.csv";
            if (!File.Exists(resultFileName))
            {
                File.WriteAllText(resultFileName, "time, x, y, z, rotx, roty, rotz, hold, post \n");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;

        //timer += Time.deltaTime;
        //float seconds = timer % 10;
        _timeLimit -= Time.deltaTime;

        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();

        //Debug.Log($"time left: {_timeLimit % 20}");
        

        if (Input.GetKeyDown(KeyCode.Escape) | Ypressed > LastY)
        {
            if (!nextTriggered)
            {
                nextTriggered = true;
                Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");

                LaunchUI.SharedCounters[1] += 1;
                Debug.Log("Active Scene is '" + scene.name + "'.");
                //StartCoroutine(_ChangeScene(scene));
                // Use a coroutine to load the Scene in the background
                StartCoroutine(LoadYourAsyncScene(scene));
            }
            
        }
        else if (_timeLimit < 0)
        {
            if (!nextTriggered)
            {
                nextTriggered = true;
                Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");

                LaunchUI.SharedCounters[1] += 1;
                Debug.Log("Time out and jump to next scene");
                //SceneManager.LoadScene(scene.buildIndex + 1);
                StartCoroutine(LoadYourAsyncScene(scene));
            }
            
        }
        else// apply gain
        {
            if (_timeLimit % 20 < 0.1)
            {
                Debug.Log($"20 seconds passed, time left: {_timeLimit}");
            }
            if (adaptation_gain != 1)
            {
                // Get the current position of the VR headset
                Vector3 currentTrackedPosition = cameraTransform.localPosition;

                // Calculate the physical movement delta
                Vector3 deltaMovement = currentTrackedPosition - lastTrackedPosition;

                // Apply the gain factors separately for X and Z axes
                Vector3 gainedMovement = new Vector3(deltaMovement.x * (adaptation_gain - 1), 0, deltaMovement.z * (adaptation_gain - 1));
                //Vector3 gainedMovement = new Vector3(deltaMovement.z * gainZ, 0, -deltaMovement.x * gainX);
                // Update the XR Origin's position
                _xrOrigin.transform.position += gainedMovement;

                // Update last tracked position for the next frame
                lastTrackedPosition = currentTrackedPosition;
            }
            if (save_file)
            {
                if (_collideNext) post = 1;
                DateTime currentDateTime = DateTime.Now;
                string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss.fff");
                update_once = $"{dateString}," +
                    $"{MainCamera.transform.position.x / adaptation_gain},{MainCamera.transform.position.y},{MainCamera.transform.position.z / adaptation_gain}," +
                    $"{MainCamera.transform.eulerAngles.x},{MainCamera.transform.eulerAngles.y},{MainCamera.transform.eulerAngles.z}," +
                    $"{hold}, {post}" + "\n";
                File.AppendAllText(resultFileName, update_once);
                post = 0;
            }
            LastA = Apressed; LastB = Bpressed;
            LastX = Xpressed; LastY = Ypressed;
        }
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
    
    public void EnableHold()
    {
        hold = 1;
    }
    public void DisableHold()
    {
        hold = 0;
    }
}
