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
            _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext = true;
        }
        else if (other.tag == "cube")
        {
            if (transform.name == "object_cube")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
                Debug.Log($"Correct!! Trigger {other.tag}, {transform.name}");

                _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext = true;
                _roomExp.GetComponent<RoomExperiment_pillar>().object_active = transform.name;
            }
            else
            {
                SoundFXManager.Instance.PlaySoundFXClip(_wrong, transform, 1f);
                Debug.Log($"Wrong!! Trigger {other.tag}, {transform.name}");
                
                _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext = true;
                //_roomExp.GetComponent<RoomExperiment>()._collideNext = false;
                _roomExp.GetComponent<RoomExperiment_pillar>().object_active = transform.name;
            }
        }
        else if (other.tag == "cylinder" |  other.tag == "triangle" | other.tag == "pentagon")
        {
            if (other.tag == "cylinder" & transform.name == "object_cylinder")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
            }
            else if (other.tag == "triangle" & transform.name == "object_triangle")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
            }
            else if (other.tag == "pentagon" & transform.name == "object_pentagon")
            {
                SoundFXManager.Instance.PlaySoundFXClip(_correct, transform, 1f);
            }
            else
            {
                SoundFXManager.Instance.PlaySoundFXClip(_wrong, transform, 1f);
                Debug.Log($"Wrong!! Trigger: {other.tag}, object: {transform.name}");
            }

            _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext = true;
            _roomExp.GetComponent<RoomExperiment_pillar>().object_active = transform.name;
        }
        _roomExp.GetComponent<RoomExperiment_pillar>().marker_tag = other.tag;
    }
    
}
