using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;

public class ExpConditionStaircase : MonoBehaviour
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
    public bool save_file = true;
    [SerializeField] private GameObject MainCamera;
    public bool firstRound = true;
    private bool firstExpansive = true;
    private bool firstCompressive = true;

    private float rand_rotation;
    private float exp_distance = 1f;
    private float exp_width = 1f;
    private int exp_more = 1;
    private int exp_less = 0;

    private int curr_exp = 0;

    private List<object[]> exp_conditions = new List<object[]>();
    //private List<float> all_gain = new List<float> { 2f / 3.0f, 0.8f, 1f, 1.25f, 1.5f, 2f };
    //private List<float> all_distance = new List<float> { 1.5f };
    //private List<float> all_width = new List<float> { 1f }; //{ 1f, 1.125f, 1.25f};

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

    private List<float> stair_gains = new List<float>();
    private float current_direction = -1f;
    private int correct_expansive_B = 0;
    private int wrong_expansive_A = 0;
    private int correct_compressive_A = 0;
    private int wrong_compressive_B = 0;
    private int thr_correct = 2;
    private int thr_reversal = 11;//each sequence, accumulating across top-ups
    private int thr_trials = 20; //half for each sequence and each session, not accumulating across top-ups
    private int thr_rounds = 10; // top up rounds

    // reversal variables. Check carefully!!
    private int reversal_expansive = 0;
    private int reversal_compressive = 0;
    //private int reversal_expansive_num;
    //private int reversal_compressive_num;
    private int pre_reversal_expansive = -1;
    private int pre_reversal_compressive = -1;

    private float gain_next;
    private float initial_expansive = 2.1f;
    private float initial_compressive = 0.567f;
    private float step_initial = 0.5f;
    private float step_unchanged;
    
    private float gain_current_expansive;
    private float gain_current_compressive;

    private List<float> step_sizes_expansive = new List<float> { 0.5f, 0.25f, 0.125f, 0.07f, 0.05f, 0.03f};
    private List<float> step_sizes_compressive = new List<float> { 0.5f, 0.25f, 0.125f, 0.07f, 0.05f, 0.03f};
    private float current_step_expansive;
    private float current_step_compressive;
    private int expansive_compressive;

    private int curr_expansive = 0;
    private int curr_compressive = 0;
    bool reversal_satisfied = false;

    private int pre_expansive_compressive;
    private string debug_expansive_compressive;
    private string debug_expansive_compressive_last;

    private bool satisfied_expansive;
    private bool satisfied_compressive;
    // Start is called before the first frame update
    private void Start()
    {
        
        // =========> gain step
        if (LaunchUI.reversal_expansive_num < step_sizes_expansive.Count)
        {
            current_step_expansive = step_sizes_expansive[LaunchUI.reversal_expansive_num];
        }
        else
        {
            current_step_expansive = step_sizes_expansive.Last();
        }
        //current_step_expansive = step_sizes_expansive[LaunchUI.reversal_expansive_num];
        if (LaunchUI.reversal_compressive_num < step_sizes_compressive.Count)
        {
            current_step_compressive = step_sizes_compressive[LaunchUI.reversal_compressive_num];
        }
        else
        {
            current_step_compressive = step_sizes_compressive.Last();
        }
        //current_step_compressive = step_sizes_compressive[LaunchUI.reversal_compressive_num];

        // =======> gain
        if (LaunchUI.gains_expansive.Count > 1)
        {
            gain_current_expansive = LaunchUI.gains_expansive.Last();
        }
        else
        {
            gain_current_expansive = 2f + UnityEngine.Random.Range(-0.1f, 0.1f);
            LaunchUI.gains_expansive.Add(gain_current_expansive);
        }
        
        if (LaunchUI.gains_compressive.Count > 1)
        {
            gain_current_compressive = LaunchUI.gains_compressive.Last();
        }
        else
        {
            gain_current_compressive = 0.667f + UnityEngine.Random.Range(-0.01f, 0.01f);
            LaunchUI.gains_compressive.Add(gain_current_compressive);
        }
        Debug.Log($"Randomize initial gain value: {gain_current_compressive}, {gain_current_expansive}, size {LaunchUI.gains_compressive.Count}");

        //-------------------- for limiting gain----------------------------------------------------------------------------
        if (LaunchUI.initial_expansive < 2)
        {
            LaunchUI.initial_expansive = initial_expansive; // gain_current_expansive;
        }
        if (LaunchUI.initial_compressive < 0.667)
        {
            LaunchUI.initial_compressive = initial_compressive; //gain_current_compressive;
        }

        step_unchanged = step_initial;
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
            // expansive_compressive, curr_expansive, curr_compressive, distance, gain, width, angle, more, less, curr_exp_log, Apressed, LastA, Bpressed, LastB,
            // correct_expansive_B, wrong_expansive_A, correct_compressive_A, wrong_compressive_B, reversal_expansive, reversal_compressive
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

        //LastA = DataInput.bttnApressed;
        //LastB = DataInput.bttnBpressed;
        //LastX = DataInput.bttnXpressed;
        //LastY = DataInput.bttnYpressed;

        SetBlind();
        LastA = GetComponent<DataInputFold>().bttnApressed;
        LastB = GetComponent<DataInputFold>().bttnBpressed;
        LastX = GetComponent<DataInputFold>().bttnXpressed;
        LastY = GetComponent<DataInputFold>().bttnYpressed;

        // the first set of parameters are used in Start()
        //curr_exp += 1;
        cameraTransform = _xrOrigin.Camera.transform;
        lastTrackedPosition = cameraTransform.localPosition;

        Debug.Log("Starting a new scene!!");

        //Debug.Log("Contents of the list: " + ListToString(LaunchUI.gains_both));
        //Debug.Log(LaunchUI.stair_gains);
    }


    // Update is called once per frame
    private void Update()
    {
        //if (exp_conditions.Count == 0) GenCondition();
        Apressed = GetComponent<DataInputFold>().bttnApressed;
        Bpressed = GetComponent<DataInputFold>().bttnBpressed;
        Xpressed = GetComponent<DataInputFold>().bttnXpressed;
        Ypressed = GetComponent<DataInputFold>().bttnYpressed;

        satisfied_expansive = LaunchUI.reversal_expansive_num >= thr_reversal;
        satisfied_compressive = LaunchUI.reversal_compressive_num >= thr_reversal;
        reversal_satisfied = (satisfied_expansive & satisfied_compressive);
        if (dark.alpha < 0.1)
        {
            //Debug.Log($"{blind_on}");
            //if (Input.GetKeyDown(KeyCode.Space) | Apressed > LastA | Bpressed > LastB | curr_exp == 0)
            if (Input.GetKeyDown(KeyCode.Space) | Apressed > LastA | Bpressed > LastB | firstRound)
            {
                //if (num_reversal > thr_reversal | curr_exp > thr_trials)
                if ((curr_compressive + curr_expansive) > thr_trials | reversal_satisfied)
                {
                    
                    if (!nextTriggered | reversal_satisfied)
                    {
                        nextTriggered = true;
                        Debug.Log("Reached limit " + curr_exp);
                        //Application.Quit();
                        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
                        LaunchUI.SharedCounters[0] += 1;

                        if (LaunchUI.SharedCounters[1] >= thr_rounds | reversal_satisfied)
                        {
                            StartCoroutine(_ChangeScene(scene.buildIndex + 1));
                            //SceneManager.LoadScene(scene.buildIndex + 1);
                        }
                        else
                        {
                            Debug.Log("[Debug] here1");
                            if (fade == null) { Debug.Log("fade is Null"); }
                            save_status();
                            LaunchUI.restore = true;
                            StartCoroutine(_ChangeScene(scene.buildIndex - 1));
                            //SceneManager.LoadScene(scene.buildIndex - 1);
                        }
                    }
                }
                else
                {
                    Debug.Log($"Ap/A: {Apressed}/{LastA}, Bp/B: {Bpressed}/{LastB}. {firstExpansive}, {firstCompressive}");

                    if (Apressed > LastA)
                    {
                        //Debug.Log("Apressed");
                        exp_more = 0;
                        exp_less = 1;
                    }
                    else if (Bpressed > LastB)
                    {
                        //Debug.Log("Bpressed");
                        exp_more = 1;
                        exp_less = 0;
                    }
                    else
                    {
                        exp_more = 0;
                        exp_less = 0;
                    }
                    //Debug.Log($"more/less: {exp_more}/{exp_less}.");


                    if (!firstRound) // first frame, we do nothing
                    {
                        if (dark != null) dark.alpha = 1;
                        SetBlind();
                        if (save_file)
                        {
                            //float curr_exp_log = curr_exp;
                            // expansive_compressive, curr_expansive, curr_compressive, distance, gain, width, angle, more, less, curr_exp_log, Apressed, LastA, Bpressed, LastB,
                            // correct_expansive_B, wrong_expansive_A, correct_compressive_A, wrong_compressive_B, reversal_expansive, reversal_compressive
                            
                            // first half
                            
                            File.AppendAllText(resultFileName, 
                                expansive_compressive + ", " + curr_expansive + ", " + curr_compressive + ", " 
                                + exp_distance + ", " + exp_gain + ", " + exp_width + ", " + rand_rotation + ", " + exp_more + ", " + exp_less + ", " 
                                + curr_exp + ", " + Apressed + ", " + LastA + ", " + Bpressed + ", " + LastB + "," 
                                );
                            //+ correct_expansive_B + ", " + wrong_expansive_A + ", " 
                            //+ correct_compressive_A + ", " + wrong_compressive_B + ", "
                            //+ LaunchUI.reversal_expansive_num + ", " + LaunchUI.reversal_compressive_num 
                            //+"," + current_step_expansive + ", " + current_step_compressive
                        }
                    }

                    // start to setup the new scene
                    GenAngle();

                    //bttn_reset = true;

                    //GetConditions
                    
                    // handling previous data
                    if (expansive_compressive == 0)
                    {
                        debug_expansive_compressive_last = "expansive";
                        if (firstExpansive)
                        {
                            gain_next = gain_current_expansive;
                            firstExpansive = false;
                        }
                        else
                        {
                            StairCase_expansive(gain_current_expansive, Apressed > LastA, Bpressed > LastB);
                            gain_current_expansive = gain_next;
                        }
                    }
                    else
                    {
                        debug_expansive_compressive_last = "compressive";
                        if (firstCompressive)
                        {
                            gain_next = gain_current_compressive;
                            firstCompressive = false;
                        }
                        else
                        {
                            StairCase_compressive(gain_current_compressive, Apressed > LastA, Bpressed > LastB);
                            gain_current_compressive = gain_next;
                        }
                    }

                    if (!firstRound) // first frame, we do nothing
                    // this is for second half of the data
                    {
                        if (dark != null) dark.alpha = 1;
                        SetBlind();
                        if (save_file)
                        {
                            float curr_exp_log = curr_exp - 1;
                            // expansive_compressive, curr_expansive, curr_compressive, distance, gain, width, angle, more, less, curr_exp_log, Apressed, LastA, Bpressed, LastB,
                            // correct_expansive_B, wrong_expansive_A, correct_compressive_A, wrong_compressive_B, reversal_expansive, reversal_compressive

                            // first half

                            File.AppendAllText(resultFileName,
                                LaunchUI.reversal_expansive_num + ", " + LaunchUI.reversal_compressive_num
                                + "," + current_step_expansive + ", " + current_step_compressive
                                + "\n");
                        }
                    }

                    // randomly choose this round's experiment type and gain
                    if (curr_expansive == thr_trials / 2)
                    {
                        expansive_compressive = 1;
                    }
                    else if (curr_compressive == thr_trials / 2)
                    {
                        expansive_compressive = 0;
                    }
                    else
                    {
                        expansive_compressive = UnityEngine.Random.Range(0, 2);
                    }

                    // if expansive satisfied only show compressive, and vice versa
                    if (satisfied_compressive)
                    {
                        expansive_compressive = 0;
                    }
                    else if (satisfied_expansive)
                    {
                        expansive_compressive = 1;
                    }
                    // get this round's gain
                    if (expansive_compressive == 0)
                    {
                        debug_expansive_compressive = "expansive";
                        curr_expansive++;
                        gain_next = gain_current_expansive;
                    }
                    else
                    {
                        debug_expansive_compressive = "compressive";
                        curr_compressive++;
                        gain_next = gain_current_compressive;
                        firstCompressive = false;
                    }


                    Debug.Log($"{debug_expansive_compressive}, compressive step {current_step_compressive}");
                    //SingleStairCase(gain_initial, Apressed > LastA, Bpressed > LastB);
                    //limit_gain();
                    LaunchUI.gains_both.Add(gain_next);

                    exp_gain = gain_next; //(float)exp_conditions[curr_exp][0];
                    exp_distance = LaunchUI.all_distance[UnityEngine.Random.Range(0, LaunchUI.all_distance.Count)]; //(float)exp_conditions[curr_exp][1];
                    exp_width = LaunchUI.all_width[UnityEngine.Random.Range(0, LaunchUI.all_width.Count)]; //(float)exp_conditions[curr_exp][2];
                    SetFold(exp_distance, exp_width);
                    // change angle by set value
                    //Debug.Log("random angle is '" + rand_rotation + "'.");
                    fold_left.transform.eulerAngles = new Vector3(-90, 45, rand_rotation);
                    fold_right.transform.eulerAngles = new Vector3(-90, -45, -rand_rotation);
                    //Debug.Log(firstRound);

                    Debug.Log($"==> curr_exp: {curr_exp}: prev_type: {debug_expansive_compressive_last}, " +
                        $"type: {debug_expansive_compressive}, Gain: {exp_gain}, Width: {exp_width}, " +
                        $"Distance: {exp_distance}, Angle {rand_rotation}, Mateiral ");


                    firstRound = false;
                    LastA = Apressed; LastB = Bpressed;
                    LastX = Xpressed; LastY = Ypressed;

                    if (LaunchUI.restore)
                    {
                        //LaunchUI.saved_status = new List<float> {
                        //pre_reversal_compressive, reversal_compressive,
                        //pre_reversal_expansive, reversal_expansive,
                        //correct_expansive_B, wrong_expansive_A,
                        //correct_compressive_A, wrong_compressive_B, current_step_expansive, current_step_compressive};
                        pre_reversal_compressive = (int)LaunchUI.saved_status[0];
                        reversal_compressive = (int)LaunchUI.saved_status[1];

                        pre_reversal_expansive = (int)LaunchUI.saved_status[2];
                        reversal_expansive = (int)LaunchUI.saved_status[3];

                        correct_expansive_B = (int)LaunchUI.saved_status[4]; ;
                        wrong_expansive_A = (int)LaunchUI.saved_status[5];
                        correct_compressive_A = (int)LaunchUI.saved_status[6];
                        wrong_compressive_B = (int)LaunchUI.saved_status[7];

                        current_step_expansive = LaunchUI.saved_status[8];
                        current_step_compressive = LaunchUI.saved_status[9];

                        curr_exp = LaunchUI.curr_exp;

                        LaunchUI.restore = false;
                    }
                    else
                    {
                        curr_exp += 1;
                        LaunchUI.curr_exp = curr_exp;
                    }
                    
                }

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) | Xpressed > LastX)
        {
            Debug.Log("Show scene");
            dark.alpha = 0;
            _floor.SetActive(true);
            _appertureTop.SetActive(true);
            _appertureBottom.SetActive(true);
            fold_left.SetActive(true);
            fold_right.SetActive(true);
        }
        else if (Ypressed > LastY)
        {
            Debug.Log("Switch scene");
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            //Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");


            LaunchUI.SharedCounters[0] += 1;

            if (LaunchUI.SharedCounters[1] >= 4 | reversal_satisfied)
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

        // Apply gain
        ApplyGain(exp_distance, exp_gain);

    }

    public void ApplyGain(float local_distance, float local_gain)
    {
        if (local_gain != 1)
        {
            // Get the current position of the VR headset then apply gain
            Vector3 currentTrackedPosition = cameraTransform.localPosition;
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
        //Debug.Log($"distance {distance}, width {width}.");
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

        if (width == 1f)
        {
            meshRendererL.material = Material1;
        }
        else if (width == 1.125f)
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
    void StairCase_expansive(float gain_curr_expansive, bool inputA, bool inputB) // high: 2 down 1 up
    {
        // 2
        if (LaunchUI.reversal_expansive_num < step_sizes_expansive.Count)
        {
            current_step_expansive = step_sizes_expansive[LaunchUI.reversal_expansive_num];
        }
        else
        {
            current_step_expansive = step_sizes_expansive.Last();
        }
        current_direction = -1;
        if (inputB)
        {
            // got correct answer
            correct_expansive_B++;
            wrong_expansive_A = 0;
        }
        else
        {
            // got wrong answer
            correct_expansive_B = 0;
            wrong_expansive_A++;
        }

        if (correct_expansive_B >= thr_correct)
        {
            gain_next = gain_curr_expansive + current_direction * current_step_expansive;
            correct_expansive_B = 0;
            wrong_expansive_A = 0;
            Debug.Log($"[Staircase] expansive 2B and updating gain to {gain_curr_expansive}");
            reversal_expansive = 0;
        }
        else if (inputA)
        {
            gain_next = gain_curr_expansive - current_direction * current_step_expansive;
            correct_expansive_B = 0;
            wrong_expansive_A = 0;
            //step_initial = step_unchanged / num_reversal;
            Debug.Log($"[Staircase] expansive A and updating gain to {gain_curr_expansive}");
            reversal_expansive = 1;
        }
        LaunchUI.gains_expansive.Add(gain_next);

        if (pre_reversal_expansive < 0)
        {
            pre_reversal_expansive = reversal_expansive;
        }
        if (pre_reversal_expansive != reversal_expansive)
        {
            //reversal_expansive_num++;
            LaunchUI.reversal_expansive_num++;
            pre_reversal_expansive = reversal_expansive;
        }
        
    }

    void StairCase_compressive(float gain_curr_compressive, bool inputA, bool inputB) // low: 2 up 1 down
    {
        // 0.667
        if (LaunchUI.reversal_compressive_num < step_sizes_compressive.Count)
        {
            current_step_compressive = step_sizes_compressive[LaunchUI.reversal_compressive_num];
        }
        else
        {
            current_step_compressive = step_sizes_compressive.Last();
        }
        current_direction = 1;
        if (inputA)
        {
            // got correct answer
            correct_compressive_A++;
            wrong_compressive_B = 0;
            current_direction = 1;
        }
        else
        {
            // got wrong answer
            correct_compressive_A = 0;
            wrong_compressive_B++;
        }
        Debug.Log($"======> Compressive {correct_compressive_A}");
        if (correct_compressive_A >= thr_correct)
        {
            gain_next = gain_curr_compressive + current_direction * current_step_compressive;
            correct_compressive_A = 0;
            wrong_compressive_B = 0;
            Debug.Log($"[Staircase] compressive 2A and updating gain {gain_curr_compressive} to {gain_next}");
            reversal_compressive = 0;
        }
        else if (inputB)
        {
            gain_next = gain_curr_compressive - current_direction * current_step_compressive;
            correct_compressive_A = 0;
            wrong_compressive_B = 0;
            //step_initial = step_unchanged / num_reversal;
            Debug.Log($"[Staircase] compressive B and updating gain {gain_curr_compressive} to {gain_next}");
            reversal_compressive = 1;
        }
        LaunchUI.gains_compressive.Add(gain_next);


        if (pre_reversal_compressive < 0)
        {
            pre_reversal_compressive = reversal_compressive;
        }
        if (pre_reversal_compressive != reversal_compressive)
        {
            //reversal_compressive_num++;
            LaunchUI.reversal_compressive_num++;
            pre_reversal_compressive = reversal_compressive;
        }
    }

    
    private void limit_gain()
    {
        if (gain_next > LaunchUI.initial_expansive)
        {
            Debug.Log($"[limiting] {gain_next} {LaunchUI.initial_expansive}");
            gain_next = LaunchUI.initial_expansive;
        }
        else if (gain_next < LaunchUI.initial_compressive)
        {
            Debug.Log($"[limiting] {gain_next} {LaunchUI.initial_compressive}");
            gain_next = LaunchUI.initial_compressive;
        }
    }

    // Helper method to convert list contents into a string
    string ListToString(List<float> list)
    {
        return string.Join(", ", list);
    }
    private void save_status()
    {
        LaunchUI.saved_status = new List<float> { 
            pre_reversal_compressive, reversal_compressive,
            pre_reversal_expansive, reversal_expansive, 
            correct_expansive_B, wrong_expansive_A, 
            correct_compressive_A, wrong_compressive_B, current_step_expansive, current_step_compressive 
        };
    }
}
