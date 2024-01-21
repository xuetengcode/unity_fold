using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class RoomExperiment : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public float adaptation_gain = 1f;
    //public float exp_gain = 1f; // pass to ApplyGain.cs
    // scene setup
    [SerializeField] private GameObject _stand;
    [SerializeField] private XROrigin _xrOrigin;
    void Start()
    {
        _xrOrigin.transform.position = _stand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
