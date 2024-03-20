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
    [SerializeField] private GameObject object_pentagon; 
    [SerializeField] private GameObject object_triangle;
    
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

    float[,] marker_areas = {
        {0.468f, 0.468f,   1.4f, 1.4f,    2f, 2f}, // 0 xxyyzz floor
        {0.468f, 0.468f,   1.4f, 1.4f,    0.7f, 0.7f}, // 0 xxyyzz floor
        {0.468f, 0.468f,   1.4f, 1.4f,    -0.7f, -0.7f}, // 0 xxyyzz floor
        {0.468f, 0.468f,   1.4f, 1.4f,    -2f, -2f}, // 0 xxyyzz floor
    };

    float[,] object_areas = {
        {-0.5f, -0.5f,    1.0205f, 1.0005f,    2f, 2f}, // 2 xxyyzz
        {-0.5f, -0.5f,    1.0252f, 1.0152f,    0.7f, 0.7f}, // 2 xxyyzz
        {-0.5f, -0.5f,    1.025f, 1.015f,    -0.7f, -0.7f}, // 2 xxyyzz
        {-0.5f, -0.5f,    1.0278f, 1.0078f,    -2f, -2f}, // 2 xxyyzz
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

    private int total_objects = 4;
    private bool firstRound = true;
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

        all_objects = new List<GameObject> {object_cube, object_cylinder, object_pentagon, object_triangle };

        // Range (int start, int count);
        //all_marker_idexs.AddRange(Enumerable.Range(0, 4));
        all_marker_idexs = Enumerable.Range(0,4).ToList();
        //all_obj_idexs.AddRange(Enumerable.Range(0, 2));
        all_obj_idexs = Enumerable.Range(0, total_objects).ToList();
    }

    // Update is called once per frame
    void Update()
    {
        Apressed = DataInput.bttnApressed;
        Bpressed = DataInput.bttnBpressed;
        Xpressed = DataInput.bttnXpressed;
        Ypressed = DataInput.bttnYpressed;
        if (Input.GetKeyDown(KeyCode.Space) | Xpressed > LastX | _roomExp.GetComponent<RoomExperiment_pillar>()._collideNext | firstRound)
        {
            firstRound = false;
            // those are the grabbable objects

            ShuffleIndexes(all_obj_idexs); // this is to get which object to show
            idxObject = Random.Range(0, total_objects); // this is where the object goes
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
            RandLocMarkerCylinder(marker_cylinder, idx_cylinder);
            RandLocMarker(marker_pentagon, idx_pentagon);
            RandLocMarker(marker_triangle, idx_triangle);

            // pass the info back for output
            GetComponent<RoomExperiment_pillar>().object_active_id = idxObject;
            GetComponent<RoomExperiment_pillar>().object_active_location = all_obj_idexs[0];
            GetComponent<RoomExperiment_pillar>().all_marker_idexs = all_marker_idexs;

        }
        LastA = Apressed; LastB = Bpressed;
        LastX = Xpressed; LastY = Ypressed;
    }

    public void RandLocObject(GameObject sourceObj, int idx2go)
    {
        //randx = UnityEngine.Random.Range(object_areas[idx2go, 0], object_areas[idx2go, 1]);
        //randz = UnityEngine.Random.Range(object_areas[idx2go, 4], object_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(object_areas[idx2go, 0], object_areas[idx2go, 2], object_areas[idx2go, 4]);
    }

    public void RandLocMarker(GameObject sourceObj, int idx2go)
    {
        //randx = UnityEngine.Random.Range(marker_areas[idx2go, 0], marker_areas[idx2go, 1]);
        //randz = UnityEngine.Random.Range(marker_areas[idx2go, 4], marker_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(marker_areas[idx2go, 0], marker_areas[idx2go, 2], marker_areas[idx2go, 4]);
    }
    public void RandLocMarkerCylinder(GameObject sourceObj, int idx2go)
    {
        //randx = UnityEngine.Random.Range(marker_areas[idx2go, 0], marker_areas[idx2go, 1]);
        //randz = UnityEngine.Random.Range(marker_areas[idx2go, 4], marker_areas[idx2go, 5]);
        sourceObj.transform.position = new Vector3(0.464f, marker_areas[idx2go, 2], marker_areas[idx2go, 4]);
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
