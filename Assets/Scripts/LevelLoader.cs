using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    FadeInOut fade;
    [SerializeField] GameObject _roomExp;
    [SerializeField] private AudioClip _correct;
    [SerializeField] private AudioClip _wrong;
    // Start is called before the first frame update
    //bool local = GetComponentInParent<RoomExperiments>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "toFold")
        {
            Debug.Log($"Trigger {other.tag}, {transform.name}");
            _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
        }
        else if (other.tag =="cylinder")
        {
            
            if (transform.name == "Object_Cylinder")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
                Debug.Log($"Correct!! Trigger {other.tag}, {transform.name}");
                _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
                _roomExp.GetComponent<RoomExperiment>()._activeObject = transform.name;
            }
            else
            {
                SoundFXManager.Instance.PlaySoundFXClip(_wrong, transform, 1f);
                Debug.Log($"Wrong!! Trigger {other.tag}, {transform.name}");
                _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
                //_roomExp.GetComponent<RoomExperiment>()._collideNext = false;

                _roomExp.GetComponent<RoomExperiment>()._activeObject = transform.name;
            }
            
        }
        else if (other.tag == "cube")
        {
            if (transform.name == "Object_Cube")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
                Debug.Log($"Correct!! Trigger {other.tag}, {transform.name}");
                _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
                _roomExp.GetComponent<RoomExperiment>()._activeObject = transform.name;
            }
            else
            {
                SoundFXManager.Instance.PlaySoundFXClip(_wrong, transform, 1f);
                Debug.Log($"Wrong!! Trigger {other.tag}, {transform.name}");
                _roomExp.GetComponent<RoomExperiment>()._collideNext = true;
                //_roomExp.GetComponent<RoomExperiment>()._collideNext = false;

                _roomExp.GetComponent<RoomExperiment>()._activeObject = transform.name;
            }
        }
    }
    
}
