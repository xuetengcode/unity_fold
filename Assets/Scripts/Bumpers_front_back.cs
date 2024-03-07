using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bumpers_front_back : MonoBehaviour
{
    [SerializeField]
    public GameObject bumper_m
        ;
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

    [SerializeField] private string eventTagL = "boundaryL";
    [SerializeField] private string eventTagR = "boundaryR";
    [SerializeField] private string eventTagF = "boundaryF";
    [SerializeField] private string eventTagB = "boundaryB";

    List<int> local_parallax;
    void Start()
    {
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
    }

    void Update()
    {
        if (transform.position.z > 0.1 | transform.position.z < -0.1)
        {
            bumper_m.SetActive(true);
        }
        else
        {
            bumper_m.SetActive(false);
        }
        if (GetComponentInParent<ExpCondition_front_back>().bumper_counter[0] > 1 & GetComponentInParent<ExpCondition_front_back>().bumper_counter[1]>1) 
        {
            //blind_on
            GetComponentInParent<ExpCondition_front_back>().blind_on = false;
            
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
            GetComponentInParent<ExpCondition_front_back>().blind_on = false;
        }
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Debug.Log("Event!!!");
        if (other.tag == eventTagL)
        {
            bumper_l.SetActive(true);
            bumper_r.SetActive(false);
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[0] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagR)
        {
            bumper_l.SetActive(false);
            bumper_r.SetActive(true);
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[1] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagF | other.tag == eventTagB) 
        {
            dim.alpha = 0f;// 0.7f;
        }
        //Debug.Log(GetComponentInParent<ExpCondition>().parallax);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == eventTagL)
        {
            bumper_l.SetActive(false);
        }
        else if (other.tag == eventTagR)
        {
            bumper_r.SetActive(false);
        }
        else if (other.tag == eventTagF | other.tag == eventTagB)
        {
            dim.alpha = 0;
        }
    }
}
