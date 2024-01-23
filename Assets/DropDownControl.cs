using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownControl : MonoBehaviour
{
    public TMPro.TMP_Dropdown TenDigit;
    public TMPro.TMP_Dropdown OneDigit;
    public static float playerName = 0f;
    public void playerNameSelector() {
        playerName = 0;
        playerName += 10 * TenDigit.value;
        playerName += OneDigit.value;

        Debug.Log("Player Name is set to '" + playerName + "'.");
    }
}
