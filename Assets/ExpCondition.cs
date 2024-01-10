using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;
using Unity.XR.CoreUtils;

public class ExpCondition : MonoBehaviour
{
    //[SerializeField] private Vector3 _rotation;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;

    [SerializeField] private XROrigin _xrOrigin;
    //[SerializeField] private Vector3 _PlayerLocation;

    //private GameObject _stand = GameObject.Find("stand");
    [SerializeField] private GameObject _stand;
    Vector3 rand_rotation;
    Vector3 base_location;

    public Vector3 size;
    private new MeshRenderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        
        /*
         * change the location of player
         */
        base_location = _stand.transform.position;
        //_xrOrigin.transform.position = _PlayerLocation;
        _xrOrigin.transform.position = _stand.transform.position;

        /*
         * fold: x -> left/right, y -> height, z -> far
         */
        _left.transform.position = new Vector3(base_location.x, 3f, 5f);
        _left.transform.Rotate(new Vector3(-90, 45, 0));

        _right.transform.position = new Vector3(base_location.x, 3f, 5f);
        _right.transform.Rotate(new Vector3(-90, -45, 0));

        /*
         * top/bottom apperture: x -> left/right, y -> height, z -> far
         */
        float distance = base_location.z - _left.transform.position.z;

        // get size of fold
        renderer = GetComponentInChildren<MeshRenderer>();
        size = renderer.bounds.size;
        Debug.Log("fold size is '" + size + "'.");

        _left.transform.localScale = new Vector3(1.414f, 0, 2);
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenCondition();
            Debug.Log("random rotation is '" + rand_rotation + "'.");
            _left.transform.Rotate(rand_rotation);
            _right.transform.Rotate(-rand_rotation);
        }
        
    }
    public void GenCondition()
    {
        rand_rotation = new Vector3(0, 0, Random.Range(-5f, 5.0f));
    }
}
