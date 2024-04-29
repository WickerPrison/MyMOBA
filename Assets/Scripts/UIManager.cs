using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<AbilityButton> abilities;
    [SerializeField] UltimateButton ultimate;
    [SerializeField] AbilityButton walkButton;
    [SerializeField] Image portraitImage;
    [SerializeField] List<Image> actionPointCounters = new List<Image>();
    [SerializeField] List<Image> manaCounters = new List<Image>();
    PlayerScript currentPlayer;

    public void SetupUIforTurn(PlayerScript playerScript)
    {
        portraitImage.sprite = playerScript.portrait;
        walkButton.currentPlayer = playerScript;
        walkButton.abilityNum = 1;

        for(int i = 0; i < abilities.Count; i++)
        {
            if(i + 2 < playerScript.actionPointCosts.Count)
            {
                abilities[i].gameObject.SetActive(true);
                AbilityButton ability = abilities[i];
                ability.currentPlayer = playerScript;
                ability.abilityNum = i + 2;
                ability.SetIcon(playerScript.abilityIcons[i]);
            }
            else
            {
                abilities[i].gameObject.SetActive(false);
            }
        }

        ultimate.currentPlayer = playerScript;
        ultimate.SetIcon(playerScript.ultimateIcon);
        currentPlayer = playerScript;
    }

    private void Update()
    {
        for(int i = 0; i < currentPlayer.actionPointMax; i++)
        {
            if(currentPlayer.actionPoints > i)
            {
                actionPointCounters[i].enabled = true;
            }
            else
            {
                actionPointCounters[i].enabled = false;
            }
        }
    }

    public void UpdateMana(int mana, Color color)
    {
        for(int i = 0; i < 10; i++)
        {
            if(mana > i)
            {
                manaCounters[i].enabled = true;
                manaCounters[i].color = color;
            }
            else
            {
                manaCounters[i].enabled = false;
            }
        }
    }
}
