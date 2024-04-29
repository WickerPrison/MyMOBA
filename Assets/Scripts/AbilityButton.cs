using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI cooldownText;
    [SerializeField] Color greyedOut;
    [SerializeField] Color notGreyedOut;
    [SerializeField] Image greyOut;
    [SerializeField] Image abilityIcon;
    [SerializeField] GameObject counter;
    [SerializeField] TextMeshProUGUI counterText;
    public PlayerScript currentPlayer;
    CharacterAbilities abilitiesScript;
    public int abilityNum;

    private void Update()
    {
        abilitiesScript = currentPlayer.GetComponent<CharacterAbilities>();
        if (currentPlayer.actionPoints < currentPlayer.actionPointCosts[abilityNum] || currentPlayer.greyedOutAbilities.Contains(abilityNum) || abilitiesScript.ShouldBeGreyedOut(abilityNum))
        {
            greyOut.color = greyedOut;
        }
        else
        {
            greyOut.color = notGreyedOut;
        }

        if (currentPlayer.abilityCooldowns[abilityNum] > 0)
        {
            cooldownText.text = currentPlayer.abilityCooldowns[abilityNum].ToString();
            greyOut.color = greyedOut;
        }
        else
        {
            cooldownText.text = "";
        }

        int counterNum = abilitiesScript.AbilityButtonCounter(abilityNum);
        if(counterNum < 0)
        {
            counter.SetActive(false);
        }
        else
        {
            counter.SetActive(true);
            counterText.text = counterNum.ToString();
        }
    }

    public void ActivateAbility()
    {
        abilitiesScript.ActivateAbility(abilityNum);
    }

    public void SetIcon(Sprite sprite)
    {
        abilityIcon.sprite = sprite;
    }
}
