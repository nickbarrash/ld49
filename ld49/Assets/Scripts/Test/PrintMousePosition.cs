using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrintMousePosition : MonoBehaviour
{
    public TMP_Text mouseText;

    private void Update() {
        mouseText.text = ((Vector2)InputUtility.instance.MouseToWorldZeroed()).ToString();
    }
}
