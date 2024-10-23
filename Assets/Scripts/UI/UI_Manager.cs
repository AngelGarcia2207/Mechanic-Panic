using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingLivesAmount;
    [SerializeField] private TMP_Text scoreAmount;
    [SerializeField] private float scoreScaleMultiplier = 2f;
    [SerializeField] private float scoreEffectDuration = 0.2f;
    private Vector3 originalScoreScale;

    public static UI_Manager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        remainingLivesAmount.text = GameManager.Instance.GetRemainingLives().ToString();
        originalScoreScale = scoreAmount.transform.localScale;
    }

    public void UpdateRemainingLivesText(int remainingLives)
    {
        remainingLivesAmount.text = remainingLives.ToString();
    }

    public void UpdateScoreText(int score)
    {
        scoreAmount.text = score.ToString();
        StartCoroutine(ScaleScoreText());
    }

    private IEnumerator ScaleScoreText()
    {
        float elapsed = 0f;
        while (elapsed < scoreEffectDuration)
        {
            float t = elapsed / scoreEffectDuration;
            scoreAmount.transform.localScale = Vector3.Lerp(originalScoreScale, originalScoreScale * scoreScaleMultiplier, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0f;
        while (elapsed < scoreEffectDuration)
        {
            float t = elapsed / scoreEffectDuration;
            scoreAmount.transform.localScale = Vector3.Lerp(originalScoreScale * scoreScaleMultiplier, originalScoreScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        scoreAmount.transform.localScale = originalScoreScale;
    }
}
