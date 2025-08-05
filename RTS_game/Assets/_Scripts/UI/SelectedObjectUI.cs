using RTS.Ability;
using RTS.Objects;
using RTS.Objects.Units;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace RTS.UI
{
    public class SelectedObjectUI : MonoBehaviour
    {
        [SerializeField] private Transform singleSelect;
        [SerializeField] private Transform multiSelect;
        [SerializeField] private Transform abilityGrid;
        //[SerializeField] Image icon;
        //[SerializeField] TextMeshProUGUI unitName;
        //[SerializeField] TextMeshProUGUI maxHealth;
        //[SerializeField] TextMeshProUGUI armor;
        //[SerializeField] TextMeshProUGUI attackDamage;
        //[SerializeField] TextMeshProUGUI attackRange;
        //[SerializeField] TextMeshProUGUI attackSpeed;
        //[SerializeField] TextMeshProUGUI moveSpeedValue;
        //[SerializeField] TextMeshProUGUI moveSpeedLabel;

        //private void Start()
        //{

        //}

        private void Update()
        {
            if (singleSelect.gameObject.activeInHierarchy)
            {

            }
        }

        public void UpdateSelectionUI(List<Transform> units)
        {
            switch (units.Count)
            {
                case 1:
                    singleSelect.gameObject.SetActive(true);
                    abilityGrid.gameObject.SetActive(true);
                    multiSelect.gameObject.SetActive(false);
                    UpdateSingleSelectData(units[0]);
                    UpdateAbilityGrid(units[0]);
                    break;
                case > 1:
                    singleSelect.gameObject.SetActive(false);
                    abilityGrid.gameObject.SetActive(false);
                    multiSelect.gameObject.SetActive(true);
                    UpdateMultiSelectData(units);
                    break;
                default:
                    singleSelect.gameObject.SetActive(false);
                    abilityGrid.gameObject.SetActive(false);
                    multiSelect.gameObject.SetActive(false);
                    break;
            }
        }

        private void UpdateAbilityGrid(Transform unit)
        {
            foreach (Transform child in abilityGrid)
            {
                Destroy(child.gameObject);
            }

            SelectableObject selectable = unit.GetComponent<SelectableObject>();
            if (selectable)
            {
                foreach (AbilityHolder abilityHolder in selectable.GetAbilityHolders())
                {
                    GameObject buttonPrefab = Instantiate(abilityHolder.GetButtonPrefab(), abilityGrid);
                    Button button = buttonPrefab.GetComponent<Button>();
                    if (button != null)
                    {
                        AbilityButtonUI buttonUI = button.transform.Find("AbilityButtonComponents").GetComponent<AbilityButtonUI>();
                        abilityHolder.SetButtonUIComponent(buttonUI);
                        button.onClick.AddListener(() =>
                        {
                            if (abilityHolder.GetAbilityState() == AbilityStateType.Ready)
                            {
                                Debug.Log(unit.name);
                                abilityHolder.ActivateAbility(); // Pozovi metodu aktivacije
                            }
                        });
                    }
                }
            }
            Debug.Log("ability grid updated");

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
            TextMeshProUGUI moveSpeedValue = unitStats.Find("MoveSpeedValue").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI moveSpeedLabel = unitStats.Find("MoveSpeedLbl").GetComponent<TextMeshProUGUI>();
            ObjectStatDisplay statManager = singleSelect.Find("HealthBackground").GetComponent<ObjectStatDisplay>();
            Image healthBar = singleSelect.Find("HealthBackground/Health").GetComponent<Image>();
            SelectableObject selectable = unit.GetComponent<SelectableObject>();
            if (selectable != null)
            {
                if (selectable.IsDead())
                    singleSelect.gameObject.SetActive(false);
                statManager.SetSelectedObject(selectable);
                icon.sprite = selectable.GetBaseStats().Icon;
                healthBar.fillAmount = selectable.GetCurrentHealth();
                unitName.text = selectable.GetObjectName();
                maxHealth.text = selectable.GetBaseStats().baseStats.health.ToString();
                armor.text = selectable.GetBaseStats().baseStats.armor.ToString();
                attackDamage.text = selectable.GetBaseStats().baseStats.attackDamage.ToString();
                attackRange.text = selectable.GetBaseStats().baseStats.attackRange.ToString();
                attackSpeed.text = selectable.GetBaseStats().baseStats.attackSpeed.ToString();
                Unit unitObject = selectable as Unit;
                if (unitObject != null)
                {
                    moveSpeedLabel.gameObject.SetActive(true);
                    moveSpeedValue.gameObject.SetActive(true);
                    moveSpeedValue.text = unitObject.unitStats.GetMoveSpeed().ToString();
                }
                else
                {
                    moveSpeedLabel.gameObject.SetActive(false);
                    moveSpeedValue.gameObject.SetActive(false);
                    moveSpeedValue.text = "0";
                }
            }

        }
        private void UpdateMultiSelectData(List<Transform> units)
        {
            GameObject unitLayoutPrefab = Resources.Load<GameObject>("Prefabs/2D/UnitGridLayout");
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

