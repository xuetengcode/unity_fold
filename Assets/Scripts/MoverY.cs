using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoverY : MonoBehaviour
{
    public float xSpeed = 0;
    public float ySpeed = -0.004f;
    public float zSpeed = 0;
    public string eventTag = "none";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(xSpeed, ySpeed, zSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == eventTag)
        {
            //SceneManager.LoadScene(1);
            Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            Debug.Log("Updating shared counter '" + LaunchUI.SharedCounters[0] + ", " + +LaunchUI.SharedCounters[1] + "'.");

            LaunchUI.SharedCounters[1] += 1;

            SceneManager.LoadScene(scene.buildIndex + 1);
        }

    }
}
