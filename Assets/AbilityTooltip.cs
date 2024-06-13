using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] TextMeshProUGUI APcost;
    [SerializeField] TextMeshProUGUI cooldown;
    [SerializeField] TextMeshProUGUI description;
    Image image;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void SetupTooltip(AbilityData abilityData)
    {
        abilityName.text = abilityData.name;
        APcost.text = "AP Cost: " + abilityData.APcost;
        cooldown.text = "Cooldown: " + abilityData.cooldown;
        description.text = abilityData.description;
    }

    public void ShowTooltip()
    {
        image.enabled = true;
        abilityName.gameObject.SetActive(true);
        APcost.gameObject.SetActive(true);
        cooldown.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        image.enabled = false;
        abilityName.gameObject.SetActive(false);
        APcost.gameObject.SetActive(false);
        cooldown.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
    }
}
