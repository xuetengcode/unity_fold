using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LaunchUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Button ViewingBBttn;
    [SerializeField]
    private Button ViewingMBttn;

    [SerializeField]
    private Button Adaptation1Bttn;
    [SerializeField]
    private Button Adaptation2Bttn;

    [SerializeField]
    private Button ConfirmBttn;

    private string playerName = "";
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

        Adaptation1Bttn.onClick.AddListener(() =>
        {
            Debug.Log("Adaptation 1 button clicked.");
            SharedConditions[1] = 0;
        });

        Adaptation2Bttn.onClick.AddListener(() =>
        {
            Debug.Log("Adaptation 2 button clicked.");
            SharedConditions[1] = 1;
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

    public void ReadStringInput(string s)
    {
        playerName = s;
        Debug.Log(playerName);
    }
}
