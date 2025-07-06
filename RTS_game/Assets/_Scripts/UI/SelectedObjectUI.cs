using NUnit.Framework;
using RTS;
using RTS.InputManager;
using RTS.Units;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RTS.UI
{
    public class SelectedObjectUI : MonoBehaviour
    {
        [SerializeField] private Transform singleSelect;
        [SerializeField] private Transform multiSelect;
        public void UpdateSelectionUI(List<Transform> units)
        {
            switch (units.Count)
            {
                case 1:
                    singleSelect.gameObject.SetActive(true);
                    multiSelect.gameObject.SetActive(false);
                    UpdateSingleSelectData(units[0]);
                    break;
                case > 1:
                    singleSelect.gameObject.SetActive(false);
                    multiSelect.gameObject.SetActive(true);
                    UpdateMultiSelectData(units);
                    break;
                default:
                    singleSelect.gameObject.SetActive(false);
                    multiSelect.gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateSingleSelectData(Transform unit)
        {
            Transform unitStats = singleSelect.Find("UnitStats");
            Image icon = singleSelect.Find("UnitIcon").GetComponent<Image>();
            TextMeshProUGUI unitName = singleSelect.Find("UnitName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI maxHealth = unitStats.Find("MaxHealthValue").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI armor = unitStats.Find("ArmorValue").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackDamage = unitStats.Find("AttackDamageValue").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackRange = unitStats.Find("AttackRangeValue").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI attackSpeed = unitStats.Find("AttackSpeedValue").GetComponent<TextMeshProUGUI>();
            SelectableObject selectable = unit.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                if (selectable.IsDead())
                    singleSelect.gameObject.SetActive(false);
                icon.sprite = selectable.GetBaseStats().Icon;
                unitName.text = selectable.GetObjectName();
                maxHealth.text = selectable.GetBaseStats().baseStats.health.ToString();
                armor.text = selectable.GetBaseStats().baseStats.armor.ToString();
                attackDamage.text = selectable.GetBaseStats().baseStats.attackDamage.ToString();
                attackRange.text = selectable.GetBaseStats().baseStats.attackRange.ToString();
                attackSpeed.text = selectable.GetBaseStats().baseStats.attackSpeed.ToString();
            }

        }
        private void UpdateMultiSelectData(List<Transform> units)
        {
            GameObject unitLayoutPrefab = Resources.Load<GameObject>("Prefabs/2D/UnitGridLayout");
            Debug.Log(unitLayoutPrefab.name);
            Transform unitGrid = multiSelect.Find("UnitsGrid");
            foreach (Transform child in unitGrid)
            {
                Destroy(child.gameObject);
            }
            foreach (Transform unit in units)
            {
                SelectableObject selectable = unit.GetComponent<SelectableObject>();
                if (selectable != null)
                {
                    GameObject unitLayout = Instantiate(unitLayoutPrefab, unitGrid);
                    ObjectStatDisplay statManager = unitLayout.transform.Find("HealthBackground").GetComponent<ObjectStatDisplay>();
                    statManager.SetSelectedObject(selectable);
                    Image unitIcon = unitLayout.transform.Find("unitIconBg/unitIcon").GetComponent<Image>();
                    unitIcon.sprite = selectable.GetBaseStats().Icon;
                    Image healthBar = unitLayout.transform.Find("HealthBackground/Health").GetComponent<Image>();
                    healthBar.fillAmount = selectable.GetCurrentHealth();
                }


            }
        }
    }
}

