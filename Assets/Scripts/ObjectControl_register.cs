using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl_register : MonoBehaviour
{
    [SerializeField] private GameObject _square;
    [SerializeField] private GameObject _bucket;
    [SerializeField] GameObject _roomExp;

    // Start is called before the first frame update
    List<int> LocalConditions = LaunchUI.SharedConditions;
    /*
     * 
     * */
    void Start()
    {
        Debug.Log("[!!!] This is to test if the distance in VR matches real world.");
        

        /*
        base_location = _square.transform.position;

        Vector3 size = _square.GetComponent<Collider>().bounds.size;
        Debug.Log(size);
        randx = UnityEngine.Random.Range(1.216f, 1.761f);
        randz = UnityEngine.Random.Range(1.216f, 1.761f);

        _square.transform.position = new Vector3(randx, base_location.y, randz);
        */
        //_square.transform.position = new Vector3(0, 0, 0);
        //_bucket.transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
