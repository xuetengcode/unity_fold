using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public GameObject myXRRig;
    public float xLocation = 1.94f;
    public float yLocation = -0.003f;
    public float zLocation = 4.55f;
    // Start is called before the first frame update
    void Start()
    {
        //GoToLocation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLocation() { 
        myXRRig.transform.position = new Vector3(xLocation, yLocation, zLocation);
    }
}
