using UnityEngine;
using UnityEngine.UI;

namespace RTS.Objects
{
    public class ObjectStatDisplay : MonoBehaviour
    {
        [SerializeField] private SelectableObject selectable;
        private float currentHealth;
        [SerializeField] private Image healthBar;
        private string parentName;

        void Start()
        {
            parentName = transform.parent.name;
        }
        private void Update()
        {
            if (selectable == null) return;
            if (healthBar.transform.parent)
                PositionHealth();
            UpdateStats();
            if (parentName == "UnitGridLayout(Clone)" && currentHealth <= 0)
            {
                Destroy(transform.parent.gameObject);
            }
            UpdateHealthBar();
        }

        private void UpdateStats()
        {
            if (selectable != null)
            {
                currentHealth = selectable.GetCurrentHealth();
            }
        }

        public void UpdateHealthBar()
        {
            healthBar.fillAmount = currentHealth / selectable.GetBaseStats().baseStats.health;
        }

        public void PositionHealth()
        {
            healthBar.transform.parent.rotation = Quaternion.identity;
        }
        public void SetSelectedObject(SelectableObject _selectable)
        {
            selectable = _selectable;
        }

    }

}
