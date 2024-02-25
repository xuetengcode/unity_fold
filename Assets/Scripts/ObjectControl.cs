using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    [SerializeField] private GameObject _square;
    [SerializeField] private GameObject _bucket;
    [SerializeField] GameObject _roomExp;

    private float randx;
    private float randz;
    private Vector3 base_location;

    int total_area = 2;
    float[,] object_areas = { // xxyyzz 
        {-0.6f, 0.6f,    0.08f, 0.08f,    0.09f, 1.845f}, // 0 xxyyzz floor
        {-0.6f, 0.6f,    0.08f, 0.08f,    -1.845f, -0.26f}, // 0 xxyyzz floor
    };
    float[,] bucket_areas = { // xxyyzz 
        {-0.93f, 0.25f,    0.08f, 0.08f,    -0.31f, 1.78f}, // 0 xxyyzz floor
        {-0.93f, 0.25f,    0.08f, 0.08f,    -2.67f, -0.59f}, // 0 xxyyzz floor
    };
    /*
    float[,] object_areas = { // xxyyzz 
        {-1.176f, 0.712f,    -1.627f, -1.619f,    -3.73f, 3.681f}, // 0 xxyyzz floor
        {-1.869f, -1.297f,    -0.928f, -0.928f,    -2.926f, -1.589f}, // 1 xxyyzz
        {-1.537f, -1.297f,    -0.928f, -0.928f,    -1.07f, -0.561f}, // 2 xxyyzz
        {-1.869f, -1.297f,    -0.928f, -0.928f,    -0.134f, 0.366f}, // 3 xxyyzz
        {-0.752f, -0.415f,   -0.338f, -0.338f,    3.907f, 4.417f}, // 4 xxyyzz
        { 0.899f, 1.537f,    -0.839f, -0.839f,    1.167f, 2.106f}, // 5 xxyyzz
        { 0.902f, 1.524f,    -0.93f, -0.93f,    -0.383f, 1.026f}, // 6 xxyyzz
        { 0.902f, 1.524f,    -0.93f, -0.93f, -1.845f, -0.839f}
    };

    float[,] bucket_areas = { // xxyyzz 
        {-1.365f, 0.307f,    -1.627f, -1.619f,    -4.132f, 3.376f}, // 0 xxyyzz floor
        {-2.186f, -1.659f,    -0.91f, -0.9f,    -3.127f, -1.897f}, // 1 xxyyzz
        {-1.778f, -1.659f,    -0.988f, -0.988f,    -1.288f, -0.887f}, // 2 xxyyzz
        {-2.186f, -1.659f,    -0.988f, -0.988f,    -0.361f, 0.041f}, // 3 xxyyzz
        {-1.008f, -0.761f,   -0.308f, -0.307f,    3.714f, 4.129f}, // 4 xxyyzz
        { 0.622f, 1.207f,    -0.821f, -0.821f,    0.929f, 1.775f}, // 5 xxyyzz
        { 0.647f, 1.207f,    -0.907f, -0.907f,    -0.601f, 0.69f}, // 6 xxyyzz
        { 0.622f, 1.207f,    -0.9867f, -0.985f, -2.123f, -1.185f}
    };
    */

    private int Apressed;
    private int Bpressed;
    private int Xpressed;
    private int Ypressed;
    //public bool bttn_reset = false;
    private int LastA = 0;
    private int LastB = 0;
    private int LastX = 0;
    private int LastY = 0;

    private int idxCube;
    private int idxBucket;

    // Start is called before the first frame update

    /*
     * 
     * */
    void Start()
    {
        Debug.Log("[!!!] This is Room floor at 000");
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;
        
        /*
        base_location = _square.transform.position;

        Vector3 size = _square.GetComponent<Collider>().bounds.size;
        Debug.Log(size);
        randx = UnityEngine.Random.Range(1.216f, 1.761f);
        randz = UnityEngine.Random.Range(1.216f, 1.761f);

        _square.transform.position = new Vector3(randx, base_location.y, randz);
        */
    }

    // Update is called once per frame
    void Update()
    {
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;
        if (Input.GetKeyDown(KeyCode.Space) | Xpressed > LastX | _roomExp.GetComponent<RoomExperiment>()._collideNext)
        {

            idxCube = Random.Range(0, total_area);

            Debug.Log("Cube at Area:" + idxCube + ", " + object_areas[idxCube, 0] + ", " + object_areas[idxCube, 1]);

            randx = UnityEngine.Random.Range(object_areas[idxCube, 0], object_areas[idxCube, 1]);
            randz = UnityEngine.Random.Range(object_areas[idxCube, 4], object_areas[idxCube, 5]);

            _square.transform.position = new Vector3(randx, object_areas[idxCube, 2], randz);

            while (true)
            {
                idxBucket = Random.Range(0, total_area);
                if (idxBucket != idxCube) break;
            }
            

            Debug.Log("Bucket at Area:" + idxBucket + ", " + bucket_areas[idxBucket, 0] + ", " + bucket_areas[idxBucket, 1]);

            randx = UnityEngine.Random.Range(bucket_areas[idxBucket, 0], bucket_areas[idxBucket, 1]);
            randz = UnityEngine.Random.Range(bucket_areas[idxBucket, 4], bucket_areas[idxBucket, 5]);
            _bucket.transform.position = new Vector3(randx, bucket_areas[idxBucket, 2], randz);
            _roomExp.GetComponent<RoomExperiment>()._collideNext = false;

        }
        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
    }
}
