using CoreScripts.ObjectPool;
using UnityEngine;
using Combat;

namespace CoreScripts.ObjectPool
{
    public class DamagePopupPool : ComponentPool<DamagePopup>
    {
        public static DamagePopupPool Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Spawn(int damage, Vector3 pos,bool isheal)
        {
            var popup = Retrieve();

            Vector3 offsetPos = pos + Vector3.up * 2f;

            popup.Show(damage, offsetPos,isheal);
        }
    }
}