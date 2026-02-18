using DG.Tweening;
using TMPro;
using UnityEngine;
using Firat0667.WesternRoyaleLib.Behavior.GameSystem;

namespace Combat
{
    public class DamagePopup : MonoBehaviour, IPool<DamagePopup>
    {
        [SerializeField] private TextMeshPro m_text;

        [Header("Animate")]
        [SerializeField] private float m_moveUp = 1.5f;
        [SerializeField] private float m_duration = 0.8f;
        [SerializeField] private float m_jumpPower = 0.45f;

        private ComponentPool<DamagePopup> m_pool;
        private Tween m_moveTween;
        private Tween m_fadeTween;
        private Transform m_camera;

        private void OnEnable()
        {
            if (m_camera == null)
                m_camera = Camera.main.transform;
        }

        private void LateUpdate()
        {
            if (m_camera == null) return;

            transform.forward = m_camera.forward;
        }

        public void Init(ComponentPool<DamagePopup> pool)
        {
            m_pool = pool;
        }

        public void Show(int damage, Vector3 position,bool isheal)
        {
            KillTweens();

            transform.position = position;
            transform.localScale = Vector3.one;

            if(isheal)
                m_text.color = Color.green;
             else
                m_text.color = Color.white;

            m_text.text = damage.ToString();
            m_text.alpha = 1f;

            Animate();
        }

        private void Animate()
        {
            float randomX = Random.Range(-0.5f, 0.5f);

            Vector3 targetPos = transform.position + new Vector3(randomX, m_moveUp, 0);

            transform.localScale = Vector3.zero;

            transform
                .DOScale(1.3f, 0.12f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                    transform.DOScale(1f, 0.08f));
            m_moveTween = transform
                .DOJump(targetPos, m_jumpPower, 1, m_duration)
                .SetEase(Ease.OutCubic);

            m_fadeTween = m_text
                .DOFade(0f, m_duration)
                .SetEase(Ease.InQuad)
                .OnComplete(ReturnToPool);
        }

        private void ReturnToPool()
        {
            KillTweens();
            m_pool.Return(this);
        }

        private void KillTweens()
        {
            m_moveTween?.Kill();
            m_fadeTween?.Kill();
        }
    }
}