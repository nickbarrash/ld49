using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public float fadeOutSec;
    public float fadeInSec;
    public float showForSec;

    public List<TMP_Text> textToFade;
    public List<Image> imagesToFade;

    private bool showing = false;
    private bool active = false;
    private float fadeOutRemainingSec = -1f;
    private float fadeInRemainingSec = -1f;

    public Fader triggerNext;

    Color tmpColor;

    public void SetAlpha(float alpha)
    {
        foreach(var text in textToFade)
        {
            tmpColor = text.color;
            tmpColor.a = alpha;
            text.color = tmpColor;
        }

        foreach(var image in imagesToFade)
        {
            tmpColor = image.color;
            tmpColor.a = alpha;
            image.color = tmpColor;
        }
    }

    public void TriggerFade()
    {
        active = true;
        showing = true;
        fadeInRemainingSec = fadeInSec;

        gameObject.SetActive(true);
    }

    private IEnumerator FadeRoutine()
    {
        SetAlpha(1); 
        yield return new WaitForSeconds(showForSec);
        FadeOut();
    }

    private void FadeOut()
    {
        fadeOutRemainingSec = fadeOutSec;
        active = false;
    }

    private void Start() {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active && fadeInRemainingSec <= 0 && fadeOutRemainingSec <= 0)
        {
            gameObject.SetActive(false);

            if (triggerNext != null)
                triggerNext.TriggerFade();

            return;
        }

        if (fadeInSec != 0 && fadeInRemainingSec > 0) {
            SetAlpha(Mathf.InverseLerp(fadeInSec, 0, fadeInRemainingSec));
            fadeInRemainingSec -= Time.deltaTime;
        } else if (showing) {
            SetAlpha(1);
            showing = false;
            StartCoroutine(FadeRoutine());
        } else if (fadeOutRemainingSec > 0) {
            SetAlpha(Mathf.InverseLerp(0, fadeOutSec, fadeOutRemainingSec));
            fadeOutRemainingSec -= Time.deltaTime;
        }
    }
}
