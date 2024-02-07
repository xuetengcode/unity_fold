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

    float[,] object_areas = { // xxyyzz
        {-1.631f, -1.12f,    -0.8f, -0.8f,    -3.442f, -2.074f}, // 0 xxyyzz
        {-1.631f, -1.12f,    -0.8f, -0.8f,    -1.625f, -0.328f}, // 1 xxyyzz
        {-0.608f, -0.307f,   -0.262f, -0.262f,    3.916f, 4.469f}, // 2 xxyyzz
        {1.287f, 1.815f,    -0.76f, -0.76f,    0.9f, 1.869f}, // 3 xxyyzz
        {1.287f, 1.564f,    -0.82f, -0.82f,    -0.328f, 0.567f}, // 4 xxyyzz
        {1.287f, 1.815f,    -0.893f, -0.893f,    -2.208f, -1.14f}, // 5 xxyyzz
        {-0.841f, 1f,    -1.377f, -1.377f,    -4.2f, 3.18f}, // 6 floor

    };

    float[,] bucket_areas = { // xxyyzz
        {-1.631f, -1.12f,    -0.8f, -0.8f,    -3.442f, -2.074f}, // 0 xxyyzz
        {-1.631f, -1.12f,    -0.8f, -0.8f,    -1.625f, -0.328f}, // 1 xxyyzz
        {-0.608f, -0.307f,   -0.262f, -0.262f,    3.916f, 4.469f}, // 2 xxyyzz
        {1.287f, 1.815f,    -0.76f, -0.76f,    0.9f, 1.869f}, // 3 xxyyzz
        {1.287f, 1.564f,    -0.82f, -0.82f,    -0.328f, 0.567f}, // 4 xxyyzz
        {1.287f, 1.815f,    -0.893f, -0.893f,    -2.208f, -1.14f}, // 5 xxyyzz
        {-0.841f, 1f,    -1.377f, -1.377f,    -4.2f, 3.18f}, // 6 floor

    };
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
        Apressed = GetComponent<DataInput>().bttnApressed;
        Bpressed = GetComponent<DataInput>().bttnBpressed;
        Xpressed = GetComponent<DataInput>().bttnXpressed;
        Ypressed = GetComponent<DataInput>().bttnYpressed;
        base_location = _square.transform.position;

        Vector3 size = _square.GetComponent<Collider>().bounds.size;
        Debug.Log(size);
        randx = UnityEngine.Random.Range(1.216f, 1.761f);
        randz = UnityEngine.Random.Range(1.216f, 1.761f);

        _square.transform.position = new Vector3(randx, base_location.y, randz);

    }

    // Update is called once per frame
    void Update()
    {
        Apressed = GetComponent<DataInput>().bttnApressed;
        Bpressed = GetComponent<DataInput>().bttnBpressed;
        Xpressed = GetComponent<DataInput>().bttnXpressed;
        Ypressed = GetComponent<DataInput>().bttnYpressed;
        if (Input.GetKeyDown(KeyCode.Space) | Apressed > LastA | Bpressed > LastB | _roomExp.GetComponent<RoomExperiment>()._collideNext)
        {

            idxCube = Random.Range(3, 7);

            Debug.Log("Cube at Area:" + idxCube + ", " + object_areas[idxCube, 0] + ", " + object_areas[idxCube, 1]);

            randx = UnityEngine.Random.Range(object_areas[idxCube, 0], object_areas[idxCube, 1]);
            randz = UnityEngine.Random.Range(object_areas[idxCube,4], object_areas[idxCube, 5]);

            _square.transform.position = new Vector3(randx, object_areas[idxCube, 2], randz);

            while (true)
            {
                idxBucket = Random.Range(3, 7);
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
