using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Jobs;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl_pillar : MonoBehaviour
{
    [SerializeField] private GameObject object_cube;
    [SerializeField] private GameObject object_cylinder;

    //[SerializeField] private GameObject _targetCube;
    //[SerializeField] private GameObject _targetCylnder;
    [SerializeField] private GameObject marker_cube;
    [SerializeField] private GameObject marker_cylinder;
    [SerializeField] private GameObject marker_pentagon;
    [SerializeField] private GameObject marker_triangle;

    [SerializeField] GameObject _roomExp;

    private GameObject object_active;
    private string object_active_str;

    private float randx;
    private float randz;
    private Vector3 base_location;

    int total_area = 7;

    float[,] object_areas = { // zyx: y + 1.6
        {-1.176f, 0.712f,    0.073f, 0.073f,    -3.73f, 3.681f}, // 0 xxyyzz floor
        {-1.869f, -1.297f,    0.772f, 0.772f,    -2.926f, -1.589f}, // 1 xxyyzz
    };

    float[,] marker_areas = { // zyx: y + 1.6
        {-1.537f, -1.297f,    0.772f, 0.772f,    -1.07f, -0.561f}, // 2 xxyyzz
        {-1.869f, -1.297f,    0.772f, 0.772f,    -0.134f, 0.366f}, // 3 xxyyzz
        {-1.537f, -1.297f,    0.772f, 0.772f,    -1.07f, -0.561f}, // 2 xxyyzz
        {-1.869f, -1.297f,    0.772f, 0.772f,    -0.134f, 0.366f}, // 3 xxyyzz
        {-1.537f, -1.297f,    0.772f, 0.772f,    -1.07f, -0.561f}, // 2 xxyyzz
        {-1.869f, -1.297f,    0.772f, 0.772f,    -0.134f, 0.366f}, // 3 xxyyzz
    };

    List<int> all_marker_idexs;
    List<int> all_obj_idexs;
    private List<GameObject> all_objects;
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

    private int idxObject;
    private List<int> idx_markers;
    private int idx_cube;
    private int idx_cylinder;
    private int idx_pentagon;
    private int idx_triangle;


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

        all_objects = new List<GameObject> {object_cube, object_cylinder};

        // Range (int start, int count);
        //all_marker_idexs.AddRange(Enumerable.Range(0, 4));
        all_marker_idexs = Enumerable.Range(0,4).ToList();
        //all_obj_idexs.AddRange(Enumerable.Range(0, 2));
        all_obj_idexs = Enumerable.Range(0, 2).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;
        if (Input.GetKeyDown(KeyCode.Space) | Xpressed > LastX | _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext)
        {
            // those are the grabbable objects

            ShuffleIndexes(all_obj_idexs);
            idxObject = Random.Range(0, 1);
            object_active = all_objects[all_obj_idexs[0]];
            all_objects[all_obj_idexs[0]].SetActive(true);
            for (int i_obj = 1; i_obj < all_obj_idexs.Count; i_obj++)
            {
                all_objects[all_obj_idexs[i_obj]].SetActive(false);
            }
                

            /*
            // first find out which object this is
            object_active_str = _roomExp.GetComponent<RoomExperiment_pillar>().object_active;
            if (object_active_str == "object_cube")
            {
                idxObject = Random.Range(0, 1);
                //idxActive = 0; // Random.Range(0, total_area);
                object_active = object_cube;
                Debug.Log("Cube at Area:" + idxObject + ", " + object_areas[idxObject, 0] + ", " + object_areas[idxObject, 1]);
            }
            else if (object_active_str == "object_cylinder")
            {
                idxObject = Random.Range(0, 1); // Random.Range(0, total_area);
                object_active = object_cylinder;
                Debug.Log("Cube at Area:" + idxObject + ", " + object_areas[idxObject, 0] + ", " + object_areas[idxObject, 1]);
            }
            else
            {
                Debug.Log($"Unknow object name!! {object_active_str}");
            }
            */

            // then send it to random location
            if (object_active != null)
            {
                RandLocObject(object_active, idxObject);
                //randx = UnityEngine.Random.Range(object_areas[idxActive, 0], object_areas[idxActive, 1]);
                //randz = UnityEngine.Random.Range(object_areas[idxActive, 4], object_areas[idxActive, 5]);
                //_activeObject.transform.position = new Vector3(randx, object_areas[idxActive, 2], randz);

                _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext = false;
            }
            else
            {
                Debug.Log("No active Object selected!! ");
            }
            /*
            idxCurrMarker = Random.Range(0, 3);
            if (idxCurrMarker == 0)
            {
                _targetMarker = _markerCube;
            }
            else if (idxCurrMarker == 1)
            {
                _targetMarker = _markerCylnder;
            }
            else if (idxCurrMarker == 1)
            {
                _targetMarker = _markerCylnder;
            }
            else if (idxCurrMarker == 1)
            {
                _targetMarker = _markerCylnder;
            }
            
                // those are the collider as you call them
                idxMarker = Random.Range(0, 1);
                RandLocMarker(_targetMarker, idxMarker);
            }*/

            ShuffleIndexes(all_marker_idexs);
            idx_cube = all_marker_idexs[0];
            idx_cylinder = all_marker_idexs[1];
            idx_pentagon = all_marker_idexs[2];
            idx_triangle = all_marker_idexs[3];

            RandLocMarker(marker_cube, idx_cube);
            RandLocMarker(marker_cylinder, idx_cylinder);
            RandLocMarker(marker_pentagon, idx_pentagon);
            RandLocMarker(marker_triangle, idx_triangle);

        }
        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
    }

    public void RandLocObject(GameObject sourceObj, int idx2go)
    {
        randx = UnityEngine.Random.Range(object_areas[idx2go, 0], object_areas[idx2go, 1]);
        randz = UnityEngine.Random.Range(object_areas[idx2go, 4], object_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(randx, object_areas[idx2go, 2], randz);
    }

    public void RandLocMarker(GameObject sourceObj, int idx2go)
    {
        randx = UnityEngine.Random.Range(marker_areas[idx2go, 0], marker_areas[idx2go, 1]);
        randz = UnityEngine.Random.Range(marker_areas[idx2go, 4], marker_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(randx, marker_areas[idx2go, 2], randz);
    }

    void ShuffleIndexes(List<int> conditions)
    {
        System.Random rand = new System.Random();
        int n = conditions.Count;
        while (n > 1)
        {
            n--;
            int k = rand.Next(n + 1);
            var value = conditions[k];
            conditions[k] = conditions[n];
            conditions[n] = value;
        }
    }
}
