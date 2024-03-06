using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorControl : MonoBehaviour
{
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Button buttonFirst;
    [SerializeField] private Button buttonSecond;
    [SerializeField] private Button buttonThird;

    public void ChangeColorFirst()
    {
        Change2Color(buttonFirst, selectedColor);
        Change2Color(buttonSecond, defaultColor);
        if (buttonThird != null) Change2Color(buttonThird, defaultColor);
    }
    public void ChangeColorSecond()
    {
        Change2Color(buttonSecond, selectedColor);
        Change2Color(buttonFirst, defaultColor);
        if (buttonThird != null) Change2Color(buttonThird, defaultColor);
    }
    public void ChangeColorThird()
    {
        Change2Color(buttonThird, selectedColor);
        Change2Color(buttonFirst, defaultColor);
        Change2Color(buttonSecond, defaultColor);
    }
    public void Change2Color(Button button, Color tarColor)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = tarColor;
        cb.highlightedColor = tarColor;
        cb.pressedColor = tarColor;
        cb.selectedColor = tarColor;
        cb.disabledColor = tarColor;
        button.colors = cb;
    }
}
