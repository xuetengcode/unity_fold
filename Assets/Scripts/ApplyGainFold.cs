using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ApplyGainFold : MonoBehaviour
{
    //public GameObject otherGameObject;
    [SerializeField] private GameObject _leftCollider;
    [SerializeField] private GameObject _rightCollider;
    [SerializeField] private GameObject _stand;
    private Vector3 lastTrackedPosition;
    private XROrigin xrOrigin;
    private Transform cameraTransform;

    private float curr_gain;
    // Start is called before the first frame update
    void Start()
    {


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
        curr_gain = GetComponent<ExpCondition>().exp_gain;
        //Debug.Log($"Applying Gain: {curr_gain}");

        // Get the current position of the VR headset
        Vector3 currentTrackedPosition = cameraTransform.localPosition;

        // Calculate the physical movement delta
        Vector3 deltaMovement = currentTrackedPosition - lastTrackedPosition;

        // Apply the gain factors separately for X and Z axes
        //Vector3 gainedMovement = new Vector3(deltaMovement.x * (curr_gain - 1), 0, deltaMovement.z * (curr_gain - 1));
        Vector3 gainedMovement = new Vector3(deltaMovement.x * (curr_gain - 1), 0, 0);


        //Vector3 gainedMovement = new Vector3(deltaMovement.z * gainZ, 0, -deltaMovement.x * gainX);
        // Update the XR Origin's position
        xrOrigin.transform.position += gainedMovement;

        // Update last tracked position for the next frame
        lastTrackedPosition = currentTrackedPosition;
        
        _leftCollider.transform.position += new Vector3(deltaMovement.x * (curr_gain - 1), 0, 0);
        _rightCollider.transform.position += new Vector3(deltaMovement.x * (curr_gain - 1), 0, 0);
        _stand.transform.position += new Vector3(deltaMovement.x * (curr_gain - 1), 0, 0);
    }
}
