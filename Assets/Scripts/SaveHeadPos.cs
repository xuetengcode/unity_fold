using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SaveHeadPos : MonoBehaviour
{
    [SerializeField] public GameObject MainCamera;
    //[SerializeField] private string _csvPost = "room";
    private string resultFileName;
    private float tester = DropDownControl.playerName;
    private string viewing;
    private float adaptation_gain;
    List<int> LocalConditions = LaunchUI.SharedConditions;
    private string update_once;
    // Start is called before the first frame update
    void Start()
    {
        if (LocalConditions[1] == 0)
        {
            adaptation_gain = 0.667f;
        }
        else
        {
            adaptation_gain = 2f;
        }
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        /*
         * Initialize output file
         */
        Directory.CreateDirectory(Application.dataPath + "/output/");
        DateTime currentDateTime = DateTime.Now;
        //string dateString = currentDateTime.ToString("yyyyMMddHHmmss");
        string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
        // name_gain_yyyy-mm-dd_tt-tt-tt(24h)_viewing

        //adaptation_gain = GetComponent<RoomExperiment>().adaptation_gain;

        if (LocalConditions[1] == 0)
        {
            adaptation_gain = 0.667f;
        }
        else
        {
            adaptation_gain = 2f;
        }

        resultFileName = Application.dataPath + "/output/" + tester + "_" + adaptation_gain + "_" + dateString + "_" + viewing + "_"+ scene.name + "_log.csv";
        if (!File.Exists(resultFileName))
        {
            File.WriteAllText(resultFileName, "x, y, z, rotx, roty, rotz \n");
        }
        Debug.Log(resultFileName);
        //_xrOrigin.transform.position
    }

    // Update is called once per frame
    void Update()
    {
        update_once = $"{MainCamera.transform.position.x},{MainCamera.transform.position.y},{MainCamera.transform.position.z}"+
            $"{MainCamera.transform.eulerAngles.x},{MainCamera.transform.eulerAngles.y},{MainCamera.transform.eulerAngles.z}," +
            "\n";
        File.AppendAllText(resultFileName, update_once);
    }
}
