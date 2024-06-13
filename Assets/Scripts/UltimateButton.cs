using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UltimateButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] Color greyedOut;
    [SerializeField] Color notGreyedOut;
    [SerializeField] Image greyOut;
    [SerializeField] Image abilityIcon;
    [SerializeField] GameObject counter;
    [SerializeField] TextMeshProUGUI counterText;
    AbilityTooltip abilityTooltip;
    public PlayerScript currentPlayer;
    CharacterAbilities abilitiesScript;

    private void Start()
    {
        abilityTooltip = GetComponentInChildren<AbilityTooltip>();
    }

    private void Update()
    {
        abilitiesScript = currentPlayer.GetComponent<CharacterAbilities>();
        if (abilitiesScript.CanUseUltimate())
        {
            greyOut.color = notGreyedOut;
        }
        else
        {
            greyOut.color = greyedOut;
        }

        if (currentPlayer.ultimateCD > 0)
        {
            cooldownText.text = currentPlayer.ultimateCD.ToString();
            greyOut.color = greyedOut;
        }
        else
        {
            cooldownText.text = "";
        }

        int counterNum = abilitiesScript.AbilityButtonCounter(10);
        if (counterNum < 0)
        {
            counter.SetActive(false);
        }
        else
        {
            counter.SetActive(true);
            counterText.text = counterNum.ToString();
        }
    }

    public void ActivateUltimate()
    {
        abilitiesScript.ActivateUltimate();
    }

    public void SetIcon(Sprite sprite)
    {
        abilityIcon.sprite = sprite;
    }

    public void SetupTooltip(AbilityData abilityData)
    {
        abilityTooltip.SetupTooltip(abilityData);
    }
}
