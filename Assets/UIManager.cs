using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
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
