using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    FadeInOut fade;
    [SerializeField] GameObject _roomExp;
    // Start is called before the first frame update
    //bool local = GetComponentInParent<RoomExperiments>();
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger {other.tag}");
        if (other.tag == "toFold")
        {
            _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
        }
    }
    
}
