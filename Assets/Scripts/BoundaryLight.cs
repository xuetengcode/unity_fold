using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryLight : MonoBehaviour
{
    public GameObject _bLeft;
    public GameObject _bRight;

    [SerializeField]
    public CanvasGroup _blindCanvasGroup;
    public CanvasGroup _greyCanvasGroup;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;
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
        if (GetComponentInParent<ExpCondition>().parallax[0] > 2 & GetComponentInParent<ExpCondition>().parallax[1]>2) 
        {
            _blindCanvasGroup.alpha = 0;
            _floor.SetActive(true);
            _appertureTop.SetActive(true);
            _appertureBottom.SetActive(true);
            _left.SetActive(true);
            _right.SetActive(true);
        }
        //local_parallax = GetComponentInParent<ExpCondition>().parallax;
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.tag);
        //Debug.Log("Event!!!");
        if (other.tag == eventTagL)
        {
            _bLeft.SetActive(true);
            _bRight.SetActive(false);
            GetComponentInParent<ExpCondition>().parallax[0] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagR)
        {
            _bLeft.SetActive(false);
            _bRight.SetActive(true);
            GetComponentInParent<ExpCondition>().parallax[1] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (other.tag == eventTagF | other.tag == eventTagB) 
        {
            _greyCanvasGroup.alpha = 0.7f;
        }
        //Debug.Log(GetComponentInParent<ExpCondition>().parallax);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == eventTagL)
        {
            _bLeft.SetActive(false);
        }
        else if (other.tag == eventTagR)
        {
            _bRight.SetActive(false);
        }
        else if (other.tag == eventTagF | other.tag == eventTagB)
        {
            _greyCanvasGroup.alpha = 0;
        }
    }
}
