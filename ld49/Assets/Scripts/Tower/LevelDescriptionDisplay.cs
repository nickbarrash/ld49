using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum FADE_STATE
{
    FADEIN,
    SHOW,
    FADEOUT
}

public class LevelDescriptionDisplay : MonoBehaviour {
    public TMP_Text levelDescriptionText;

    private string activeDescription = null;
    public Queue<string> descriptionsToDisplay = new Queue<string>();

    public float fadeOutSec;
    public float fadeInSec;
    public float showForSec;

    private FADE_STATE state = FADE_STATE.FADEIN;
    private float fadeOutRemainingSec = -1f;
    private float fadeInRemainingSec = -1f;

    Color tmpColor;

    bool init = false;

    public static LevelDescriptionDisplay instance;

    private void Awake() {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }


    private void Start() {
        if (!init) {
            init = true;
            gameObject.SetActive(false);
        }
    }

    public void QueueDescription(string description) {
        descriptionsToDisplay.Enqueue(description);

        NextInQueue();
    }

    private IEnumerator FadeRoutine()
    {
        SetAlpha(1); 
        yield return new WaitForSeconds(showForSec);
        FadeOut();
    }

    public void GameOver()
    {
        descriptionsToDisplay.Clear();
        activeDescription = null;
    }

    private void FadeOut()
    {
        state = FADE_STATE.FADEOUT;
        fadeOutRemainingSec = fadeOutSec;
    }

    private void SetAlpha(float alpha) {
        tmpColor = levelDescriptionText.color;
        tmpColor.a = alpha;
        levelDescriptionText.color = tmpColor;
    }

    public void NextInQueue()
    {
        if (descriptionsToDisplay.Count == 0 || activeDescription != null)
            return;

        gameObject.SetActive(true);

        state = FADE_STATE.FADEIN;
        activeDescription = descriptionsToDisplay.Dequeue();
        levelDescriptionText.text = activeDescription;
        fadeInRemainingSec = fadeInSec;
    }

    private void Update() {
        if (descriptionsToDisplay.Count == 0 && activeDescription == null) {
            gameObject.SetActive(false);
            return;
        }

        if (state == FADE_STATE.FADEIN)
        {
            if (fadeInRemainingSec > 0)
            {
                SetAlpha(Mathf.InverseLerp(fadeInSec, 0, fadeInRemainingSec));
                fadeInRemainingSec -= Time.deltaTime;
                return;
            }
            else
            {
                state = FADE_STATE.SHOW;
                SetAlpha(1);
                StartCoroutine(FadeRoutine());
            }
            return;
        }

        if (state == FADE_STATE.FADEOUT)
        {
            if (fadeOutRemainingSec > 0)
            {
                SetAlpha(Mathf.InverseLerp(0, fadeOutSec, fadeOutRemainingSec));
                fadeOutRemainingSec -= Time.deltaTime;
                return;
            }
            else
            {
                SetAlpha(0);
                activeDescription = null;
                NextInQueue();
            }
        }
    }
}
