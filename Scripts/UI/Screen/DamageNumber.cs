using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class DamageNumber : MonoBehaviour
{
    #region Font Variables
    public const float LIFETIME = 0.8f;
    public const int BIGDAMAGELIMIT = 10;
    public const int MAXDAMAGELIMIT = 20;
    [SerializeField] int minFontSize = 12;
    [SerializeField] int maxFontSize = 36;
    #endregion
    #region FountainVariables
    [Header("Fountain")]
    [SerializeField] float MAX_ARC_PERCENTAGE = 0.5f;
    [SerializeField] float GRAVITY = -100f;
    [SerializeField] float FORCE = 100;
    [SerializeField] float INTENSITY = 2f;
    #endregion
    #region RiseVariables
    const float LERP_SIZE_TIME_PERCENTAGE = 0.3f;
    Vector2 localPositionOffset = new Vector2(0, 60);
    #endregion
    [SerializeField] public AnimationType animationType = AnimationType.Fountain;
    Vector2 velocity;
    public Color color;
    [SerializeField] TMP_Text text;
    RectTransform rect;
    public enum AnimationType
    {
        Fountain,
        Rise,
    }
    void Awake()
    {
        text = this.GetComponent<TMP_Text>();
        rect = this.GetComponent<RectTransform>();
        color = text.color;
        Hide();
    }
    public void Display(float damage)
    {
        velocity = GetLaunchAngle() * FORCE;
        rect.localPosition = Vector3.zero;
        color.a = 1;
        text.text = "" + damage;
        HandleAnimationType(damage);
    }
    public void Hide()
    {
        color.a = 0;
        text.color = color;
    }
    public void HandleAnimationType(float damage)
    {
        if (damage < BIGDAMAGELIMIT)
        {
            StartCoroutine(AnimateFountain(damage));
            return;
        }
        StartCoroutine(AnimateRise(damage));
    }
    IEnumerator AnimateFountain(float damage)
    {
        float timer = 0;
        text.fontSize = GetFontSize(damage);
        while (timer < LIFETIME)
        {
            rect.localPosition += (Vector3)velocity * Time.deltaTime * INTENSITY;
            velocity.y += GRAVITY * Time.deltaTime * INTENSITY;
            timer += Time.deltaTime;
            color.a = 1 - (timer / LIFETIME);
            text.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    private int GetFontSize(float damage)
    {
        damage = Mathf.Abs(Mathf.Clamp(damage, -MAXDAMAGELIMIT, MAXDAMAGELIMIT));
        int fontSizeDelta = maxFontSize - minFontSize;
        int fontSize = Mathf.RoundToInt(minFontSize + (fontSizeDelta * damage / MAXDAMAGELIMIT));
        return fontSize;
    }
    IEnumerator AnimateRise(float damage)
    {
        float timer = 0;
        float sizeTime = LIFETIME * LERP_SIZE_TIME_PERCENTAGE;
        float positionTime = LIFETIME - sizeTime;
        float targetFontSize = GetFontSize(damage);
        float fontDelta = targetFontSize - minFontSize;
        while (timer < sizeTime)
        {
            color.a = (EasingFunction.EaseOutCirc(0, 1, timer / sizeTime));
            text.color = color;
            text.fontSize = minFontSize + (fontDelta * EasingFunction.EaseOutCirc(0, 1, timer / sizeTime));
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        timer = 0;
        while (timer < positionTime)
        {

            rect.localPosition = localPositionOffset * EasingFunction.EaseInExpo(0, 1, timer / positionTime);
            color.a = 1 - EasingFunction.EaseInExpo(0, 1, timer / positionTime);
            text.color = color;
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Hide();
    }
    private Vector2 GetLaunchAngle()
    {
        float maxRad = (Mathf.PI / 2) * MAX_ARC_PERCENTAGE;
        float rad = Random.Range(-maxRad, maxRad);
        Vector2 launchAngle = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
        return launchAngle;
    }
    [ContextMenu("Animate")]
    void Test()
    {
        Display(12);
    }
}
