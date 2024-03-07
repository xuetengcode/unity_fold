using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bumpers_front_back : MonoBehaviour
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
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
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
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[0] += 1;
            bumper_m.SetActive(true);
            Debug.Log($"Bumper_counter_0: {GetComponentInParent<ExpCondition_front_back>().bumper_counter[0]}");
            still_counting = false;
        }
        else if (transform.position.z < -0.1 & still_counting)//(transform.position.z > 0.1 | transform.position.z < -0.1)
        {
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[1] += 1;
            bumper_m.SetActive(true);
            Debug.Log($"Bumper_counter_1: {GetComponentInParent<ExpCondition_front_back>().bumper_counter[1]}");
            still_counting = false;
        }
        else if (transform.position.z >= -0.1 & transform.position.z <= 0.1)
        {
            bumper_m.SetActive(false);
            still_counting = true;
        }
        if (GetComponentInParent<ExpCondition_front_back>().bumper_counter[0] > 1 & GetComponentInParent<ExpCondition_front_back>().bumper_counter[1] > 1)
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
}
    /*
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Debug.Log("Event!!!");
        if (other.tag == eventTagF)
        {
            //bumper_l.SetActive(true);
            //bumper_r.SetActive(false);
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[0] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagB)
        {
            //bumper_l.SetActive(false);
            //bumper_r.SetActive(true);
            GetComponentInParent<ExpCondition_front_back>().bumper_counter[1] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagL | other.tag == eventTagR) 
        {
            dim.alpha = 0.7f;
        }
        //Debug.Log(GetComponentInParent<ExpCondition>().parallax);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == eventTagF)
        {
            bumper_m.SetActive(false);
        }
        else if (other.tag == eventTagB)
        {
            bumper_m.SetActive(false);
        }
        else if (other.tag == eventTagL | other.tag == eventTagR)
        {
            dim.alpha = 0;
        }
    }
*/