using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoundaryLight : MonoBehaviour
{
    public GameObject _bLeft;
    public GameObject _bRight;
    
    public string eventTagL = "boundaryL";
    public string eventTagR = "boundaryR";
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Event!!!");
        if (other.tag == eventTagL)
        {
            _bLeft.SetActive(true);
            _bRight.SetActive(false);
        }
        else if (other.tag == eventTagR)
        {
            _bLeft.SetActive(false);
            _bRight.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _bLeft.SetActive(false);
        _bRight.SetActive(false);
    }
}
