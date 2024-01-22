using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExperiment : MonoBehaviour
{
    // Start is called before the first frame update
    
    // scene setup
    [SerializeField] private GameObject _stand;
    [SerializeField] private XROrigin _xrOrigin;

    public GameObject _cameraL;
    // different gains
    public float adaptation_gain = 1f;
    //public float exp_gain = 1f; // pass to ApplyGain.cs

    List<int> LocalConditions = LaunchUI.SharedConditions;
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            SceneManager.LoadScene(scene.buildIndex + 1);
        }
    }
}
