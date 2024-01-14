using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using Unity.XR.CoreUtils;
using System.IO;
using System;
using System.Linq;

public class ExpCondition : MonoBehaviour
{
    [SerializeField] private string tester = "tx";
    [SerializeField] private string viewing = "bino";
    [SerializeField] private float exp_gain = 1f;
    [SerializeField] private int exp_repeat = 2;

    //[SerializeField] private Vector3 _rotation;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;

    [SerializeField] private XROrigin _xrOrigin;
    //[SerializeField] private Vector3 _PlayerLocation;

    //private GameObject _stand = GameObject.Find("stand");
    [SerializeField] private GameObject _stand;
    private float rand_rotation;
    private float exp_distance = 1f;
    private float exp_width = 1f;
    private int exp_more = 1;
    private int exp_less = 0;

    private List<object[]> exp_conditions = new List<object[]>();
    private List<float> all_gain = new List<float> { 0.5f, 1f, 1.5f, 1.75f, 2f };
    private List<float> all_distance = new List<float> { 0.1f, 0.2f, 0.3f };

    private Vector3 base_location;
    private string resultFileName;

    public Vector3 size;
    private new MeshRenderer renderer;

    // Start is called before the first frame update
    private void Start()
    {
        /*
         * Initialize output file
         */
        Directory.CreateDirectory(Application.dataPath + "/output/");
        DateTime currentDateTime = DateTime.Now;
        //string dateString = currentDateTime.ToString("yyyyMMddHHmmss");
        string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
        // name_gain_yyyy-mm-dd_tt-tt-tt(24h)_viewing
        
        resultFileName = Application.dataPath + "/output/" + tester + "_" + exp_gain + "_" + dateString + "_" + viewing + "_test.csv";
        if (!File.Exists(resultFileName))
        {
            File.WriteAllText(resultFileName, "distance, gain, width, angle, more, less \n");
        }
        /*
         * change the location of player
         */
        base_location = _stand.transform.position;
        //_xrOrigin.transform.position = _PlayerLocation;
        _xrOrigin.transform.position = _stand.transform.position;

        /*
         * fold: x -> left/right, y -> height, z -> far
         */
        _left.transform.position = new Vector3(base_location.x, 3f, 5f);
        _left.transform.Rotate(new Vector3(-90, 45, 0));

        _right.transform.position = new Vector3(base_location.x, 3f, 5f);
        _right.transform.Rotate(new Vector3(-90, -45, 0));

        /*
         * top/bottom apperture: x -> left/right, y -> height, z -> far
         */
        float distance = base_location.z - _left.transform.position.z;

        // get size of fold
        renderer = GetComponentInChildren<MeshRenderer>();
        size = renderer.bounds.size;
        Debug.Log("fold size is '" + size + "'.");

        _left.transform.localScale = new Vector3(1.414f, 0, 2);

        // generate all experiment conditions
        GenCondition();
    }


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenAngle();
            
            /*
            // change angle by rotation
            Debug.Log("random rotation is '" + rand_rotation + "'.");
            _left.transform.Rotate(rand_rotation);
            _right.transform.Rotate(-rand_rotation);
            */

            // change angle by set value
            Debug.Log("random angle is '" + rand_rotation + "'.");
            _left.transform.eulerAngles = new Vector3 (-90, 45, rand_rotation);
            _right.transform.eulerAngles = new Vector3(-90, -45, -rand_rotation);

            // distance, gain, width, angle, more, less
            File.AppendAllText(resultFileName, exp_distance + ", " + exp_gain + ", " + exp_width + ", " + rand_rotation + ", " + exp_more + ", " + exp_less + "\n");
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("[Log] Exp done. Saving results to '" + rand_rotation + "'.");

        }
        
    }
    public void GenAngle()
    {
        // rand_angle = math.pi * (random()*5)/180 * positive_or_negative()
        // 90 degre +- 5
        rand_rotation = UnityEngine.Random.Range(-2.5f, 2.5f);
        
    }

    public void GenCondition()
    {
        for (int i_g = 0; i_g < all_gain.Count; i_g++)
        {
            for (int i_d = 0; i_d < all_distance.Count; i_d++)
            {
                exp_conditions.Add(new object[] { all_gain[i_g], all_distance[i_d] });
            }
        }
        //Debug.Log("[log] at gencondition()");
        ShuffleExpConditions(exp_conditions);
        PrintExpConditions(exp_conditions);
    }
    void PrintExpConditions(List<object[]> conditions)
    {
        //Debug.Log("[log] at print");
        foreach (var condition in conditions)
        {
            Debug.Log($"Gain: {condition[0]}, Width: {condition[1]}");
        }
    }
    void ShuffleExpConditions(List<object[]> conditions)
    {
        System.Random rand = new System.Random();
        int n = conditions.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            var value = conditions[k];
            conditions[k] = conditions[n];
            conditions[n] = value;
        }
    }
}
