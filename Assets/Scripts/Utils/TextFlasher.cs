using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextFlasher : MonoBehaviour
{
    public TMP_Text text; 

    private void OnEnable()
    {
        text.enabled = true;
        StartCoroutine(FlashText());
    }

    public IEnumerator FlashText()
    {
        while (true)
        {
            text.enabled = !text.enabled;
            yield return new WaitForSeconds(.5f);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
