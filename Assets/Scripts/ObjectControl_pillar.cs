using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl_pillar : MonoBehaviour
{
    [SerializeField] private GameObject _cube;
    [SerializeField] private GameObject _cylinder;

    [SerializeField] private GameObject _targetCube;
    [SerializeField] private GameObject _targetCylnder;
    

    [SerializeField] GameObject _roomExp;

    private GameObject _activeObject;
    private string currObj;

    private float randx;
    private float randz;
    private Vector3 base_location;

    int total_area = 7;
    
    float[,] object_areas = { // zyx: y + 1.6
        {-1.176f, 0.712f,    0.073f, 0.073f,    -3.73f, 3.681f}, // 0 xxyyzz floor
        {-1.869f, -1.297f,    0.772f, 0.772f,    -2.926f, -1.589f}, // 1 xxyyzz
        {-1.537f, -1.297f,    0.772f, 0.772f,    -1.07f, -0.561f}, // 2 xxyyzz
        {-1.869f, -1.297f,    0.772f, 0.772f,    -0.134f, 0.366f}, // 3 xxyyzz !!!
        {-0.752f, -0.415f,    1.362f, 1.362f,    3.907f, 4.417f}, // 4 xxyyzz
        { 0.899f, 1.537f,     0.861f, 0.861f,    1.167f, 2.106f}, // 5 xxyyzz
        { 0.902f, 1.524f,     0.77f, 0.77f,    -0.383f, 1.026f}, // 6 xxyyzz
        { 0.902f, 1.524f,     0.77f, 0.77f, -1.845f, -0.839f}
    };
    /*
     * markers:
        * 
        * 
        * 
        * 
     * objects:
        * 
        * 
        * 
        * 
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

    private int idxActive;

    // Start is called before the first frame update

    /*
     * 
     * */
    void Start()
    {
        Debug.Log("[!!!] This is pillar");
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;
        
        
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
            // those are the grabbable objects

            // first find out which object this is
            currObj = _roomExp.GetComponent<RoomExperiment>()._activeObject;
            if (currObj == "Object_Cube")
            {
                idxActive = 3; // Random.Range(0, total_area);
                _activeObject = _cube;
                Debug.Log("Cube at Area:" + idxActive + ", " + object_areas[idxActive, 0] + ", " + object_areas[idxActive, 1]);
            }
            else if (currObj == "Object_Cylinder")
            {
                idxActive = 1; // Random.Range(0, total_area);
                _activeObject = _cylinder;
                Debug.Log("Cube at Area:" + idxActive + ", " + object_areas[idxActive, 0] + ", " + object_areas[idxActive, 1]);
            }
            else
            {
                Debug.Log($"Unknow object name!! {currObj}");
            }

            
            // then send it to random location
            if (_activeObject != null)
            {
                RandLoc(_activeObject, idxActive);
                //randx = UnityEngine.Random.Range(object_areas[idxActive, 0], object_areas[idxActive, 1]);
                //randz = UnityEngine.Random.Range(object_areas[idxActive, 4], object_areas[idxActive, 5]);
                //_activeObject.transform.position = new Vector3(randx, object_areas[idxActive, 2], randz);

                _roomExp.GetComponent<RoomExperiment>()._collideNext = false;
            }
            else
            {
                Debug.Log("No active Object selected!! ");
            }

            // those are the collider as you call them
            RandLoc(_targetCube, 5);
            RandLoc(_targetCylnder, 6);

        }
        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
    }

    public void RandLoc(GameObject sourceObj, int idx2go)
    {
        randx = UnityEngine.Random.Range(object_areas[idx2go, 0], object_areas[idx2go, 1]);
        randz = UnityEngine.Random.Range(object_areas[idx2go, 4], object_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(randx, object_areas[idx2go, 2], randz);
    }
}
