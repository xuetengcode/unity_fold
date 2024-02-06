using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class HeadBoundary : MonoBehaviour
{
    [SerializeField] public GameObject MainCamera;
    public GameObject _bLeft;
    public GameObject _bRight;

    [SerializeField]
    public CanvasGroup _blindCanvasGroup;
    public CanvasGroup _greyCanvasGroup;
    [SerializeField] private GameObject _floor;

    private Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = MainCamera.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(cameraTransform.localPosition);
        if (cameraTransform.localPosition.x < -1)
        {
            _bLeft.SetActive(true);
            _bRight.SetActive(false);
            GetComponentInParent<ExpCondition>().parallax[0] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (cameraTransform.localPosition.x > 1)
        {
            _bLeft.SetActive(false);
            _bRight.SetActive(true);
            GetComponentInParent<ExpCondition>().parallax[1] += 1;
            //_greyCanvasGroup.alpha = 0;
        }
        else if (cameraTransform.localPosition.z>1 | cameraTransform.localPosition.z < -1)
        {
            _greyCanvasGroup.alpha = 0.7f;
        }
        else
        {
            _bLeft.SetActive(false);
            _bRight.SetActive(true);
            _greyCanvasGroup.alpha = 0;
        }
    }
}
