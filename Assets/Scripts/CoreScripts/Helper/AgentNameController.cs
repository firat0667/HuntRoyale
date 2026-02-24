using TMPro;
using UnityEngine;

public class AgentNameController : MonoBehaviour
{
    [SerializeField] private TextMeshPro m_nameText;

    private void Start()
    {
        if (m_nameText == null) return;

        var entity = GetComponentInParent<BaseEntity>();
        if (entity != null)
        {
            m_nameText.text = entity.gameObject.name;
        }
    }
}
