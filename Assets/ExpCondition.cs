using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.SceneManagement;
using UnityEditor.SearchService;

public class ExpCondition : MonoBehaviour
{
    [SerializeField] private Vector3 _rotation;
    [SerializeField] private GameObject _left;
    [SerializeField] private GameObject _right;

    Vector3 rand_rotation;
    // Start is called before the first frame update
    void Start()
    {
        
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
