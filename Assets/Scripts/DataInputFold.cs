using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Interactions;
using Unity.XR.CoreUtils;

public class DataInputFold : MonoBehaviour
{
    [Header("INPUT ACTIONS")]
    [SerializeField]
    private InputAction btnA;
    [SerializeField]
    private InputAction btnB;
    [SerializeField]
    private InputAction btnX;
    [SerializeField]
    private InputAction btnY;

    [SerializeField]
    private InputAction menuL;

    [SerializeField] private GameObject _stand;
    [SerializeField] private GameObject _xrOrigin;

    public int bttnApressed = 0;
    public int bttnBpressed = 0;
    public int bttnXpressed = 0;
    public int bttnYpressed = 0;
    public int menuRpressed = 0;
    // Start is called before the first frame update
    void Start()
    {
        btnA.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame)
                {
                    bttnApressed += 1;
                    Debug.Log($"Button A {ctx.control} was pressed");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Button A {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };
        btnB.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame)
                {
                    bttnBpressed += 1;
                    Debug.Log($"Button B {ctx.control} was pressed");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Button B {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };

        btnX.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame)
                {
                    bttnXpressed += 1;
                    Debug.Log($"Button X {ctx.control} was pressed, {bttnXpressed}");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Button X {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };

        btnY.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame)
                {
                    bttnYpressed += 1;
                    Debug.Log($"Button Y {ctx.control} was pressed");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Button Y {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };

        menuL.performed +=
            ctx =>
            {
                var button = (ButtonControl)ctx.control;
                if (button.wasPressedThisFrame)
                {
                    menuRpressed += 1;
                    _xrOrigin.transform.position = new Vector3(_stand.transform.position.x, _xrOrigin.transform.position.y, _stand.transform.position.z);
                    Debug.Log($"Menu L {ctx.control} was pressed");
                }
                else if (button.wasReleasedThisFrame)
                    Debug.Log($"Menu L {ctx.control} was released");
                // NOTE: We may get calls here in which neither the if nor the else
                //       clause are true here. A button like the gamepad left and right
                //       triggers, for example, do not just have a binary on/off state
                //       but rather a [0..1] value range.
            };

        btnA.Enable();
        btnB.Enable();

        btnX.Enable();
        btnY.Enable();

        menuL.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        
        
        //btnA.Enable();
        //btnB.Enable();
        //resetButton.Enable();
    }


}
