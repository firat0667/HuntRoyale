using DG.Tweening;
using Game.UI;
using UnityEngine;


namespace Game.UI
{
    public abstract class UIBase : MonoBehaviour, IUIState
    {
        [Header("Canvas")]
        [SerializeField] private CanvasGroup m_canvasGroup;

        [Header("Tween")]
        [SerializeField] private float m_fadeDuration = 0.2f;
        private Tween m_currentTween;

        protected virtual void Awake()
        {
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.interactable = false;
            m_canvasGroup.blocksRaycasts = false;
        }

        public virtual void Show()
        {
            m_currentTween?.Kill();

            gameObject.SetActive(true);
            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.interactable = true;

            m_currentTween = m_canvasGroup
                .DOFade(1f, m_fadeDuration)
                .SetEase(Ease.OutQuad);
        }

        public virtual void Hide()
        {
            m_currentTween?.Kill();

            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;

            m_currentTween = m_canvasGroup
                .DOFade(0f, m_fadeDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => gameObject.SetActive(false));
        }
        protected virtual void OnBeforeShow() { }
        protected virtual void OnAfterShow() { }
        protected virtual void OnBeforeHide() { }
        protected virtual void OnAfterHide() { }
    }
}
