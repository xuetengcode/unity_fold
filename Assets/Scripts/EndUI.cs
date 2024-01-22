using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Button ViewingBBttn;
    [SerializeField]
    private Button ViewingMBttn;

    [SerializeField]
    private Button ConfirmBttn;

    public static List<int> SharedConditions = new List<int> { 0, 1};
    private void Awake()
    {
        ViewingBBttn.onClick.AddListener(() =>
        {
            Debug.Log("Bino button clicked.");
            SharedConditions[0] = 0;
        });

        ViewingMBttn.onClick.AddListener(() =>
        {
            Debug.Log("Mono button clicked.");
            SharedConditions[0] = 1;
        });

        

        // support buttons
        ConfirmBttn.onClick.AddListener(() =>
        {
            Debug.Log("Confirm button clicked.");
            UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
            Debug.Log("Active Scene is '" + scene.name + "'.");
            SceneManager.LoadScene(scene.buildIndex + 1);
        });
    }
}
