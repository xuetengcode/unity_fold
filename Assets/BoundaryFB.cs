using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryFB : MonoBehaviour
{
    [SerializeField] private GameObject _SphereCenter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > 0.1 | transform.position.x < -0.1)
        {
            _SphereCenter.SetActive(true);
        }
        else
        {
            _SphereCenter.SetActive(false);
        }
    }
}
