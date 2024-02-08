//using System.Collections;
//using System.Collections.Generic;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ApplyGain : MonoBehaviour
{
    //public GameObject otherGameObject;

    private Vector3 lastTrackedPosition;
    private XROrigin xrOrigin;
    private Transform cameraTransform;

    private float curr_gain;
    List<int> LocalConditions = LaunchUI.SharedConditions;
    // Start is called before the first frame update
    void Start()
    {
        if (LocalConditions[1] == 0)
        {
            curr_gain = 0.5f;
        }
        else
        {
            curr_gain = 2f;
        }
        //curr_gain = GetComponent<RoomExperiment>().adaptation_gain;
        xrOrigin = GetComponentInChildren<XROrigin>();
        if (xrOrigin == null)
        {
            Debug.LogError("TranslationalGain script requires XROrigin component");
            enabled = false;
            return;
        }

        cameraTransform = xrOrigin.Camera.transform;
        lastTrackedPosition = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
        //Debug.Log($"Applying Gain: {curr_gain}");
        if (curr_gain != 1)
        {
            // Get the current position of the VR headset
            Vector3 currentTrackedPosition = cameraTransform.localPosition;

            // Calculate the physical movement delta
            Vector3 deltaMovement = currentTrackedPosition - lastTrackedPosition;

            // Apply the gain factors separately for X and Z axes
            Vector3 gainedMovement = new Vector3(deltaMovement.x * (curr_gain - 1), 0, deltaMovement.z * (curr_gain - 1));
            //Vector3 gainedMovement = new Vector3(deltaMovement.z * gainZ, 0, -deltaMovement.x * gainX);
            // Update the XR Origin's position
            xrOrigin.transform.position += gainedMovement;

            // Update last tracked position for the next frame
            lastTrackedPosition = currentTrackedPosition;
        }
        
    }
}
