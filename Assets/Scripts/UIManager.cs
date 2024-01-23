using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    // scene setup
    [SerializeField] private GameObject _stand;
    [SerializeField] private XROrigin _xrOrigin;
    void Start()
    {
        _xrOrigin.transform.position = _stand.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            if (scene.buildIndex + 1 < 4 )
            {
                SceneManager.LoadScene(scene.buildIndex + 1);
            }
            
        }
    }
}
