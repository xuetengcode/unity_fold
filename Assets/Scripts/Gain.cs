using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Gain : MonoBehaviour
{
    public float gainX = 1.5f; // The translational gain factor for X axis
    public float gainZ = 1.5f; // The translational gain factor for Z axis
    private Vector3 lastTrackedPosition;
    private XROrigin xrOrigin;
    private Transform cameraTransform;



    // Start is called before the first frame update
    void Start()
    {
        xrOrigin = GetComponent<XROrigin>();
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
        // Get the current position of the VR headset
        Vector3 currentTrackedPosition = cameraTransform.localPosition;

        // Calculate the physical movement delta
        Vector3 deltaMovement = currentTrackedPosition - lastTrackedPosition;

        // Apply the gain factors separately for X and Z axes
        Vector3 gainedMovement = new Vector3(deltaMovement.x * (gainX-1), 0, deltaMovement.z * (gainZ-1));
        //Vector3 gainedMovement = new Vector3(deltaMovement.z * gainZ, 0, -deltaMovement.x * gainX);
        // Update the XR Origin's position
        xrOrigin.transform.position += gainedMovement;

        // Update last tracked position for the next frame
        lastTrackedPosition = currentTrackedPosition;
    }
}
