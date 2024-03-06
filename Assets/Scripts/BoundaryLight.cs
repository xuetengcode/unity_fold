using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryLight : MonoBehaviour
{
    public GameObject _bLeft;
    public GameObject _bRight;

    [SerializeField]
    public CanvasGroup _blindCanvasGroup;
    public CanvasGroup _greyCanvasGroup;
    [SerializeField] private XROrigin _xrOrigin;
    [SerializeField] private ExpCondition _expCondition;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;
    [SerializeField] private GameObject _floor;
    [SerializeField] private GameObject _appertureTop;
    [SerializeField] private GameObject _appertureBottom;

    [SerializeField] private string eventTagL = "boundaryL";
    [SerializeField] private string eventTagR = "boundaryR";
    [SerializeField] private string eventTagF = "boundaryF";
    [SerializeField] private string eventTagB = "boundaryB";

    private bool flagL = false;
    private bool flagR = false;
    private bool flagFB = false;
    private float boundF = 0.25f;
    private float boundB = -0.33f;
    //private bool flagB = false;
    private float curr_gain;
    List<int> local_parallax;
    void Start()
    {
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
        
    }

    void Update()
    {
        curr_gain = _expCondition.GetComponent<ExpCondition>().exp_gain;
        if (GetComponentInParent<ExpCondition>().parallax[0] > 2 & GetComponentInParent<ExpCondition>().parallax[1]>2) 
        {
            if (GetComponentInParent<ExpCondition>().firstRound)
            {
                GetComponentInParent<ExpCondition>().parallax = new List<int> { 0, 0 };
                GetComponentInParent<ExpCondition>().firstRound = false;
            }
            else
            {
                //Debug.Log($"clear blind {GetComponentInParent<ExpCondition>().parallax[0]}, {GetComponentInParent<ExpCondition>().parallax[1]}");
                _blindCanvasGroup.alpha = 0;
                _floor.SetActive(true);
                _appertureTop.SetActive(true);
                _appertureBottom.SetActive(true);
                _left.SetActive(true);
                _right.SetActive(true);
            }
            
        }

        if (_xrOrigin.transform.position.z < (boundF* curr_gain) & _xrOrigin.transform.position.z > (boundB* curr_gain))
        {
            flagFB = false;
        }
        else
        {
            flagFB = false;
        }
        //Debug.Log($"R/L/FB: {flagR}, {flagL}, {flagFB}");
        if (flagR) 
        {
            _bLeft.SetActive(false);
            _bRight.SetActive(true);
            GetComponentInParent<ExpCondition>().parallax[1] += 1;
            flagR = false;
        }
        else
        {
            _bRight.SetActive(false);
        }

        if (flagL)
        {
            _bLeft.SetActive(true);
            _bRight.SetActive(false);
            GetComponentInParent<ExpCondition>().parallax[0] += 1;
            flagL = false;
        }
        else
        {
            _bLeft.SetActive(false);
        }

        if (flagFB)
        {
            _greyCanvasGroup.alpha = 0.7f;
        }
        else
        {
            _greyCanvasGroup.alpha = 0f;
        }
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Debug.Log("Event!!!");
        if (other.tag == eventTagL)
        {
            flagL = true;
            flagR = false;
            //_bLeft.SetActive(true);
            //_bRight.SetActive(false);
            //GetComponentInParent<ExpCondition>().parallax[0] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagR)
        {
            flagR = true;
            flagL = false;
            //_bLeft.SetActive(false);
            //_bRight.SetActive(true);
            //GetComponentInParent<ExpCondition>().parallax[1] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagF | other.tag == eventTagB) 
        {
            flagFB = true;
            //_greyCanvasGroup.alpha = 0.7f;
        }
        //Debug.Log(GetComponentInParent<ExpCondition>().parallax);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == eventTagL)
        {
            flagL = false;
            //_bLeft.SetActive(false);
        }
        else if (other.tag == eventTagR)
        {
            flagR = false;
            //_bRight.SetActive(false);
        }
        else if (other.tag == eventTagF | other.tag == eventTagB)
        {
            flagFB = false;
            //_greyCanvasGroup.alpha = 0;
        }
    }
}
