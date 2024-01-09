using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchUI : MonoBehaviour
{
    [SerializeField]
    private Button BinocularBtn;
    [SerializeField]
    private Button MonocularBtn;

    private void Awake()
    {
        BinocularBtn.onClick.AddListener(() =>
        {
            //add code
            Debug.Log("Binocular Clicked");
        });

        MonocularBtn.onClick.AddListener(() =>
        {
            //add code
        });
    }
}
