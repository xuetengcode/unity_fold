using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fpsdisplay : MonoBehaviour
{
    public int FPS {  get; private set; }
    public TextMeshPro displayCurrent;
    // Update is called once per frame
    void Update()
    {
        float current = (int)(1f / Time.deltaTime);
        if (Time.frameCount % 50 == 0)
        {
            displayCurrent.text = current.ToString() + " FPS";
        }
    }
}
