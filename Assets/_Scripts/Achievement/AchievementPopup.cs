using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    [SerializeField] BaseAlphaController m_alphaController;
    [SerializeField] TextMeshProUGUI m_achievementNotifierText;
    [SerializeField] Image m_achievementNotifierIcon;

    [SerializeField] AudioGroupDetails m_sfx;

    [SerializeField] float m_timeBeforeFade = 2.5f, m_fadeDuration = 0.4f;

    private void Start() => m_alphaController.SetAlpha(0);

    public void Display(string _text, Sprite _icon)
    {
        m_achievementNotifierText.text = _text;
        m_achievementNotifierIcon.sprite = _icon;

        m_alphaController.SetAlpha(1);

        transform.localScale = Vector3.one * 0.8f;
        transform.DOScale(Vector3.one, 0.3f);

        AudioManager.Instance.PlaySoundAtDistance(m_sfx, 0);

        TimerManager.RunAfterTime(() =>
        {
            TimerManager.RunUntilTime(FadeOut, m_fadeDuration);
        }, m_timeBeforeFade);
    }

    public void FadeOut(float _counter, float _duration)
    {
        m_alphaController.SetAlpha(1 - (_counter / _duration));
        //if (_counter >= _duration) transform.localScale = Vector3.zero; // - Hides object
    }
}
