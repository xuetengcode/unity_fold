using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.FilePathAttribute;
//using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
//using UnityEditor.SearchService;
using Unity.XR.CoreUtils;
using System.IO;
using System;
using System.Linq;
//using UnityEngine.SceneManagement;

// 1 - gain
public class ExpCondition_front_back : MonoBehaviour
{
    public FadeInOut fade;
    //[SerializeField] private string tester = "tx";
    private float tester = DropDownControl.playerName;
    private string tester_str;
    private string viewing;
    private float adaptation_gain;
    public float exp_gain; // pass to ApplyGain.cs
    private Vector3 lastTrackedPosition;
    private Transform cameraTransform;
    [SerializeField] private int exp_repeat = 5;

    //[SerializeField] private Vector3 _rotation;
    [SerializeField] private GameObject fold_left;
    [SerializeField] private GameObject fold_right;

    [SerializeField] private XROrigin _xrOrigin;
    //[SerializeField] private Vector3 _PlayerLocation;

    //private GameObject _stand = GameObject.Find("stand");
    //[SerializeField] private GameObject _stand;
    public GameObject _cameraL;
    [SerializeField] public CanvasGroup dark;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _appertureTop;
    [SerializeField] private GameObject _appertureBottom;

    public List<int> bumper_counter = new List<int> { 0, 0 };
    public bool save_file = false;
    [SerializeField] private GameObject MainCamera;
    public bool firstRound = true;

    private float rand_rotation;
    private float exp_distance = 1f;
    private float exp_width = 1f;
    private int exp_more = 1;
    private int exp_less = 0;

    private int curr_exp = 0;

    private List<object[]> exp_conditions = new List<object[]>();
    private List<float> all_gain = new List<float> { 2f/3.0f, 0.8f, 1f, 1.25f, 1.5f, 2f };
    private List<float> all_distance = new List<float> { 1.5f };
    private List<float> all_width = new List<float> { 1f}; //{ 1f, 1.125f, 1.25f};

    private Vector3 base_location;
    private string resultFileName;
    private string resultFileName_head;
    private string update_once;

    //public Vector3 size;
    private new MeshRenderer renderer;

    List<int> LocalConditions = LaunchUI.SharedConditions;

    private int Apressed;
    private int Bpressed;
    private int Xpressed;
    private int Ypressed;
    //public bool bttn_reset = false;
    private int LastA = 0;
    private int LastB = 0;
    private int LastX = 0;
    private int LastY = 0;

    private Material oldMaterial;
    [SerializeField] private Material Material1;
    [SerializeField] private Material Material2;
    [SerializeField] private Material Material3;
    MeshRenderer meshRendererL;
    MeshRenderer meshRendererR;

    private float fold_yy = 1.9f;
    public bool blind_on = true;
    
    private bool nextTriggered = false;
    
    // Start is called before the first frame update
    private void Start()
    {
        meshRendererL = fold_left.GetComponent<MeshRenderer>();
        meshRendererR = fold_right.GetComponent<MeshRenderer>();
        if (tester < 10)
        {
            tester_str = $"0{tester}";
        }
        else
        {
            tester_str = $"{tester}";
        }
        //oldMaterial = meshRendererL.material;
        //Material1.mainTextureScale = oldMaterial.mainTextureScale;

        // start in dark
        if (dark == null)
        {
            Debug.Log("Dark canvas not set");
            _floor.SetActive(true);
        }
        else
        {
            dark.alpha = 1;
            _floor.SetActive(false);
        }
        _appertureTop.SetActive(false);
        _appertureBottom.SetActive(false);
        fold_left.SetActive(false);
        fold_right.SetActive(false);
        fade = GetComponentInChildren<FadeInOut>();
        // bino or mono
        if (LocalConditions[0] == 0)
        {
            // bino
            _cameraL.SetActive(false);
            viewing = "bino";
        }
        else
        {
            // mono
            _cameraL.SetActive(true);
            viewing = "mono";
        }

        if (LocalConditions[1] == 0)
        {
            adaptation_gain = 0.667f;
        }
        else if (LocalConditions[1] == 1)
        {
            adaptation_gain = 1f;
        }
        else
        {
            adaptation_gain = 2f;
        }
        /*
         * Initialize output file
         */
        if (save_file)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/output/");
            DateTime currentDateTime = DateTime.Now;
            //string dateString = currentDateTime.ToString("yyyyMMddHHmmss");
            string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss");
            // name_gain_yyyy-mm-dd_tt-tt-tt(24h)_viewing
            int round_id = LaunchUI.SharedCounters[1];
            resultFileName = Application.persistentDataPath + "/output/" + tester_str + "_" + adaptation_gain + "_" + dateString + "_" + viewing + "_fold_" + round_id + ".csv";
            //if (!File.Exists(resultFileName))
            //{
            //    File.WriteAllText(resultFileName, "distance, gain, width, angle, more, less \n");
            //}

            resultFileName_head = Application.persistentDataPath + "/output/" + tester_str + "_" + adaptation_gain + "_" + dateString + "_" + viewing + "_fold_" + round_id + "_head.csv";
            //if (!File.Exists(resultFileName_head))
            //{
            //    File.WriteAllText(resultFileName_head, "time, x, y, z, rotx, roty, rotz, more, less \n");
            //}
        }

        /*
         * change the location of player
         */

        //_xrOrigin.transform.position = _PlayerLocation;
        //_xrOrigin.transform.position = _stand.transform.position;

        // get size of fold
        //renderer = GetComponentInChildren<MeshRenderer>();
        //size = renderer.bounds.size;
        //Debug.Log("fold size is '" + size + "'.");

        //   _left.transform.localScale = new Vector3(1.414f, 0, 2);

        // generate all experiment conditions
        GenCondition();

        //LastA = DataInput.bttnApressed;
        //LastB = DataInput.bttnBpressed;
        //LastX = DataInput.bttnXpressed;
        //LastY = DataInput.bttnYpressed;

        SetBlind();
        LastA = GetComponent<DataInputFold>().bttnApressed;
        LastB = GetComponent<DataInputFold>().bttnBpressed;
        LastX = GetComponent<DataInputFold>().bttnXpressed;
        LastY = GetComponent<DataInputFold>().bttnYpressed;

        //SetFold((float)exp_conditions[curr_exp][1], (float)exp_conditions[curr_exp][2]); // initial position
        // the first set of parameters are used in Start()
        //curr_exp += 1;
        cameraTransform = _xrOrigin.Camera.transform;
        lastTrackedPosition = cameraTransform.localPosition;

        exp_gain = (float)exp_conditions[curr_exp][0];
        exp_distance = (float)exp_conditions[curr_exp][1];
        exp_width = (float)exp_conditions[curr_exp][2];

        Debug.Log("Starting a new scene!!");
        Debug.Log($"curr_exp: {curr_exp}/{exp_conditions.Count}, Gain: {exp_gain}, Width: {exp_width}, Distance: {exp_distance}, Angle {rand_rotation}, Mateiral {(float)exp_conditions[curr_exp][3]}");

    }


    // Update is called once per frame
    private void Update()
    {
        //if (exp_conditions.Count == 0) GenCondition();
        Apressed = GetComponent<DataInputFold>().bttnApressed;
        Bpressed = GetComponent<DataInputFold>().bttnBpressed;
        Xpressed = GetComponent<DataInputFold>().bttnXpressed;
        Ypressed = GetComponent<DataInputFold>().bttnYpressed;

        // Apply gain
        ApplyGain(exp_distance, exp_gain);

        if (dark.alpha < 0.1)
        {
            //Debug.Log($"{blind_on}");
            //if (Input.GetKeyDown(KeyCode.Space) | Apressed > LastA | Bpressed > LastB | curr_exp == 0)
            if (Input.GetKeyDown(KeyCode.Space) | Apressed > LastA | Bpressed > LastB | firstRound)
            {
                if (curr_exp > exp_conditions.Count - 1)
                {
                    if (!nextTriggered)
                    {
                        nextTriggered = true;
                        Debug.Log("Reached limit " + curr_exp);
                        //Application.Quit();
                        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
                        LaunchUI.SharedCounters[0] += 1;

                        if (LaunchUI.SharedCounters[1] >= 4)
                        {
                            StartCoroutine(_ChangeScene(scene.buildIndex + 1));
                            //SceneManager.LoadScene(scene.buildIndex + 1);
                        }
                        else
                        {
                            Debug.Log("[Debug] here1");
                            if (fade == null) { Debug.Log("fade is Null"); }
                            StartCoroutine(_ChangeScene(scene.buildIndex - 1));
                            //SceneManager.LoadScene(scene.buildIndex - 1);
                        }
                    }
                }
                else
                {
                    Debug.Log($"Ap/A, Bp/B: {Apressed}/{LastA}, {Bpressed}/{LastB}.");

                    if (Apressed > LastA)
                    {
                        Debug.Log("Apressed");
                        exp_more = 0;
                        exp_less = 1;
                    }
                    else if (Bpressed > LastB)
                    {
                        Debug.Log("Bpressed");
                        exp_more = 1;
                        exp_less = 0;
                    }
                    else
                    {
                        exp_more = 0;
                        exp_less = 0;
                    }
                    Debug.Log($"more/less: {exp_more}/{exp_less}.");
                    

                    if (!firstRound) // first frame, we do nothing
                    {
                        if (dark != null) dark.alpha = 1;
                        SetBlind();
                        if (save_file)
                        {
                            float curr_exp_log = curr_exp - 1;
                            // distance, gain, width, angle, more, less
                            File.AppendAllText(resultFileName, exp_distance + ", " + exp_gain + ", " + exp_width + ", " + rand_rotation + ", " + exp_more + ", " + exp_less + ", " + curr_exp_log + ", " + Apressed + ", " + LastA + ", " + Bpressed + ", " + LastB + "\n");
                        }
                    }

                    // start to setup the new scene
                    GenAngle();

                    //bttn_reset = true;

                    //GetConditions
                    exp_gain = (float)exp_conditions[curr_exp][0];
                    exp_distance = (float)exp_conditions[curr_exp][1];
                    exp_width = (float)exp_conditions[curr_exp][2];
                    SetFold(exp_distance, exp_width);
                    // change angle by set value
                    //Debug.Log("random angle is '" + rand_rotation + "'.");
                    fold_left.transform.eulerAngles = new Vector3(-90, 45, rand_rotation);
                    fold_right.transform.eulerAngles = new Vector3(-90, -45, -rand_rotation);
                    //Debug.Log(firstRound);

                    Debug.Log($"==> curr_exp: {curr_exp}/{exp_conditions.Count}: Gain: {exp_gain}, Width: {exp_width}, Distance: {exp_distance}, Angle {rand_rotation}, Mateiral {(float)exp_conditions[curr_exp][3]}");


                    firstRound = false;
                    LastA = Apressed; LastB = Bpressed;
                    LastX = Xpressed; LastY = Ypressed;

                    curr_exp += 1;
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) | Xpressed > LastX)
        {
            Debug.Log("[========] X event");
            dark.alpha = 0;
            _floor.SetActive(true);
            _appertureTop.SetActive(true);
            _appertureBottom.SetActive(true);
            fold_left.SetActive(true);
            fold_right.SetActive(true);
        }
        else if (Ypressed > LastY)
        {
            Debug.Log("[========] Y event");
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            //Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");


            LaunchUI.SharedCounters[0] += 1;

            if (LaunchUI.SharedCounters[1] == 4)
            {
                StartCoroutine(_ChangeScene(scene.buildIndex + 1));
                //SceneManager.LoadScene(scene.buildIndex + 1);
            }
            else
            {
                Debug.Log("[Debug] here1");
                if (fade == null) { Debug.Log("fade is Null"); }
                StartCoroutine(_ChangeScene(scene.buildIndex - 1));
                //SceneManager.LoadScene(scene.buildIndex - 1);
            }
            //_xrOrigin.transform.position = new Vector3(0, 0, 0);
            //_xrOrigin.transform.position = _stand.transform.position;
            //_xrOrigin.transform.rotation = _stand.transform.rotation;
        }
        if (save_file)
        {
            DateTime currentDateTime = DateTime.Now;
            string dateString = currentDateTime.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss.fff");
            update_once = $"{dateString}," +
                $"{MainCamera.transform.position.x},{MainCamera.transform.position.y},{MainCamera.transform.position.z}," +
                $"{MainCamera.transform.eulerAngles.x},{MainCamera.transform.eulerAngles.y},{MainCamera.transform.eulerAngles.z}," +
                $"{exp_more}, {exp_less}" + "\n";
            File.AppendAllText(resultFileName_head, update_once);
        }

        exp_more = 0;
        exp_less = 0;

        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
        
    }

    public void ApplyGain(float local_distance, float local_gain)
    {
        if (local_gain != 1)
        {
            // Get the current position of the VR headset
            Vector3 currentTrackedPosition = cameraTransform.localPosition;

            // Calculate the physical movement delta
            //Vector3 deltaMovement = currentTrackedPosition - lastTrackedPosition;

            // Apply the gain factors separately for X and Z axes
            //Vector3 gainedMovement = new Vector3(deltaMovement.x * (1 - local_gain), 0, 0);
            //Vector3 gainedMovement = new Vector3(deltaMovement.z * gainZ, 0, -deltaMovement.x * gainX);
            // Update the XR Origin's position
            //_left.transform.position += gainedMovement;
            //_right.transform.position += gainedMovement;

            fold_left.transform.position = new Vector3(fold_left.transform.position.x, fold_left.transform.position.y, local_distance + (currentTrackedPosition.z * (1 - local_gain)));//fold_left.transform.position.z +
            fold_right.transform.position = new Vector3(fold_right.transform.position.x, fold_right.transform.position.y, local_distance + (currentTrackedPosition.z * (1 - local_gain)));//fold_right.transform.position.z +

            // Update last tracked position for the next frame
            //lastTrackedPosition = currentTrackedPosition;
        }
    }
    public void SetBlind()
    {
        bumper_counter = new List<int> { 0, 0 };
        _floor.SetActive(false);
        _appertureTop.SetActive(false);
        _appertureBottom.SetActive(false);
        fold_left.SetActive(false);
        fold_right.SetActive(false);
        //_xrOrigin.transform.position = _stand.transform.position;
    }
    public IEnumerator _ChangeScene(int nextIdx)
    {
        //fade.fadein = true;
        if (fade != null) fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(nextIdx);
    }

    void SetFold(float distance, float width = 1.414f)
    {
        float origial_width = 1.414f;
        float height = 3f;
        //base_location = _stand.transform.position;
        /*
         * fold: x -> left/right, y -> height, z -> far
         */
        fold_left.transform.position = new Vector3(0, fold_yy, distance);
        //_left.transform.Rotate(new Vector3(-90, 45, 0));

        fold_right.transform.position = new Vector3(0, fold_yy, distance);
        //_right.transform.Rotate(new Vector3(-90, -45, 0));

        // change scale

        fold_left.transform.localScale = new Vector3(width * origial_width * distance, 1e-8f, height * distance);
        fold_right.transform.localScale = fold_left.transform.localScale;

        if ((float)exp_conditions[curr_exp][3] == 0f)
        {
            meshRendererL.material = Material1;
        }
        else if ((float)exp_conditions[curr_exp][3] == 1f)
        {
            meshRendererL.material = Material2;
        }
        else
        {
            meshRendererL.material = Material3;
        }
        meshRendererR.material = meshRendererL.material;
        /*
         * top/bottom apperture: x -> left/right, y -> height, z -> far
         */
        //float distance = base_location.z - _left.transform.position.z;
    }
    public void GenAngle()
    {
        // rand_angle = math.pi * (random()*5)/180 * positive_or_negative()
        // 90 degre +- 5
        rand_rotation = UnityEngine.Random.Range(-2.5f, 2.5f);
        
    }

    public void GenCondition()
    {
        for (int i_repeat = 0; i_repeat < exp_repeat; i_repeat++)
        {
            for (int i_g = 0; i_g < all_gain.Count; i_g++)
            {
                for (int i_d = 0; i_d < all_distance.Count; i_d++)
                {
                    for (int i_w = 0; i_w < all_width.Count; i_w++)
                    {
                        exp_conditions.Add(new object[] { all_gain[i_g], all_distance[i_d], all_width[i_w], (float) i_w });
                    }
                }
            }
        }
        //Debug.Log("[log] at gencondition()");
        ShuffleExpConditions(exp_conditions);
        //PrintExpConditions(exp_conditions);
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
