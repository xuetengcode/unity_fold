//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpersStaircase : MonoBehaviour
{
    [SerializeField]
    public GameObject bumper_m;
    public GameObject bumper_l;
    public GameObject bumper_r;

    [SerializeField]
    public CanvasGroup dark;
    public CanvasGroup dim;
    [SerializeField] private GameObject fold_left;
    [SerializeField] private GameObject fold_right;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _appertureTop;
    [SerializeField] private GameObject _appertureBottom;
    /*
    [SerializeField] private string eventTagL = "boundaryL";
    [SerializeField] private string eventTagR = "boundaryR";
    [SerializeField] private string eventTagF = "boundaryF";
    [SerializeField] private string eventTagB = "boundaryB";
    */
    private bool still_counting = true;
    List<int> local_parallax;
    void Start()
    {
        bumper_l.SetActive(false);
        bumper_r.SetActive(false);
    }

    void Update()
    {
        if (transform.position.x > 0.2 | transform.position.x < -0.2)
        { dim.alpha = 0.5f; }
        else { dim.alpha = 0f; }

        if (transform.position.z > 0.1 & still_counting)//(transform.position.z > 0.1 | transform.position.z < -0.1)
        {
            GetComponentInParent<ExpConditionStaircase>().bumper_counter[0] += 1;
            bumper_m.SetActive(true);
            Debug.Log($"Bumper_counter_0: {GetComponentInParent<ExpConditionStaircase>().bumper_counter[0]}");
            still_counting = false;
        }
        else if (transform.position.z < -0.1 & still_counting)//(transform.position.z > 0.1 | transform.position.z < -0.1)
        {
            GetComponentInParent<ExpConditionStaircase>().bumper_counter[1] += 1;
            bumper_m.SetActive(true);
            Debug.Log($"Bumper_counter_1: {GetComponentInParent<ExpConditionStaircase>().bumper_counter[1]}");
            still_counting = false;
        }
        else if (transform.position.z >= -0.1 & transform.position.z <= 0.1)
        {
            bumper_m.SetActive(false);
            still_counting = true;
        }
        if (GetComponentInParent<ExpConditionStaircase>().bumper_counter[0] > 1 & GetComponentInParent<ExpConditionStaircase>().bumper_counter[1] > 1)
        {
            //blind_on
            GetComponentInParent<ExpConditionStaircase>().blind_on = false;

            //Debug.Log($"clear blind {GetComponentInParent<ExpCondition>().parallax[0]}, {GetComponentInParent<ExpCondition>().parallax[1]}");
            dark.alpha = 0;
            _floor.SetActive(true);
            _appertureTop.SetActive(true);
            _appertureBottom.SetActive(true);
            fold_left.SetActive(true);
            fold_right.SetActive(true);


        }
        else
        {
            GetComponentInParent<ExpConditionStaircase>().blind_on = false;
        }
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
    }
}
