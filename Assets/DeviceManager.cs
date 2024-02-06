using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

//using System.IO;
using System.Linq;
//using System;
//using UnityEditor.SceneManagement;

public class DeviceManager : MonoBehaviour
{
    [SerializeField]
    private XRNode xrNode = XRNode.RightHand;
    private List<InputDevice> devices = new List<InputDevice>();
    private InputDevice device;

    bool LastStateA = false;
    bool LastStateB = false;

    private bool firstinterval = false;
    private bool secondinterval = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!device.isValid)
        {
            GetDevice();
        }

        bool primaryButton = false;

        bool secondaryButton = false;

        device.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButton);

        if (primaryButton != LastStateA)
        {
            if (primaryButton == true)
            {
                //button was pressed this frame
            }
            else if (primaryButton == false)
            {
                ButtonPressed();
                firstinterval = true;
                secondinterval = false;
            }
            //Set last known state of button
            LastStateA = primaryButton;
        }

        device.TryGetFeatureValue(CommonUsages.secondaryButton, out secondaryButton);

        if (secondaryButton != LastStateB)
        {
            if (secondaryButton == true)
            {
                //button was pressed this frame
            }
            else if (secondaryButton == false)
            {
                ButtonPressed();
                firstinterval = false;
                secondinterval = true;
            }
            //Set last known state of button
            LastStateB = secondaryButton;
        }

        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 Adjust);
    }


    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable()
    {
        if (!device.isValid)
        {
            GetDevice();
        }
    }
    void ButtonPressed()
    {

        Debug.Log("Button pressed");
    }
}
