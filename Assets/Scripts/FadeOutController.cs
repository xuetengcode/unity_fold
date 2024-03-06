using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutController : MonoBehaviour
{
    FadeInOut fade;
    // Start is called before the first frame update
    void Start()
    {
        fade = GetComponent<FadeInOut>();
        fade.FadeOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
