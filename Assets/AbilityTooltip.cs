using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.U2D.Animation;
using System.Linq;

public class AbilityTooltip : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] TextMeshProUGUI APcost;
    [SerializeField] TextMeshProUGUI manaCost;
    [SerializeField] TextMeshProUGUI cooldown;
    [SerializeField] TextMeshProUGUI range;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextAsset additionsJson;
    [SerializeField] Image image;
    [SerializeField] GameObject additionsObject;
    [SerializeField] List<AdditionUI> additionUIs;
    PlayerScript playerScript;
    string descriptionText;
    Additions additions;
    AbilityData data;

    Dictionary<string, Func<string, string>> variableDict = new Dictionary<string, Func<string, string>>();

    private void Start()
    {
        additions = JsonUtility.FromJson<Additions>(additionsJson.text);
        SetUpTooltipVariables();
    }

    public void SetupTooltip(AbilityData abilityData, PlayerScript currentPlayer)
    {
        data = abilityData;

        playerScript = currentPlayer;
        abilityName.text = abilityData.name;
        APcost.text = "AP Cost: " + abilityData.APcost;
        if (abilityData.manaCost != 0)
        {
            manaCost.text = abilityData.manaName + " Cost: " + abilityData.manaCost.ToString();
        }
        else manaCost.text = "";

        cooldown.text = "Cooldown: " + abilityData.cooldown;

        if (abilityData.range != 0)
        {
            range.text = "Range: " + abilityData.range;
        }
        else range.text = "";

        descriptionText = abilityData.description;
        if(abilityData.variables != null)
        {
            foreach(string variable in abilityData.variables)
            {
                descriptionText = variableDict[variable](descriptionText) ;
            }
        }
        description.text = descriptionText;


        if(abilityData.additions == null)
        {
            foreach(AdditionUI additionUI in additionUIs)
            {
                additionUI.Hide();
            }
        }
        else
        {
            for(int i = 0; i < additionUIs.Count; i++)
            {
                if(i >= abilityData.additions.Count())
                {
                    additionUIs[i].Hide();
                }
                else
                {
                    Addition addition = additions.GetAddition(abilityData.additions[i]);
                    additionUIs[i].Setup(addition);
                }
            }
        }
    }

    public void ShowTooltip()
    {
        abilityName.gameObject.SetActive(true);
        APcost.gameObject.SetActive(true);
        manaCost.gameObject.SetActive(true);
        cooldown.gameObject.SetActive(true);
        range.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        additionsObject.SetActive(true);
    }

    public void HideTooltip()
    {
        abilityName.gameObject.SetActive(false);
        APcost.gameObject.SetActive(false);
        manaCost.gameObject.SetActive(false);
        cooldown.gameObject.SetActive(false);
        range.gameObject.SetActive(false);  
        description.gameObject.SetActive(false);
        additionsObject.SetActive(false);
    }

    void SetUpTooltipVariables()
    {
        variableDict.Add("_moveSpeed", (input) => {
            int move = playerScript.moveSpeed + playerScript.moveSpeedModifier;
            return input.Replace("_moveSpeed", move.ToString());
        });
        variableDict.Add("_damage", (input) =>
        {
            return input.Replace("_damage", data.damage.ToString());
        });
        variableDict.Add("_healing", (input) =>
        {
            return input.Replace("_healing", data.healing.ToString());
        });
        variableDict.Add("_duration", (input) =>
        {
            return input.Replace("_duration", data.duration.ToString());
        });
        variableDict.Add("_range", (input) =>
        {
            return input.Replace("_range", data.range.ToString());
        });
        variableDict.Add("_gainTurnMeter", (input) =>
        {
            return input.Replace("_gainTurnMeter", data.gainTurnMeter.ToString());
        });
        variableDict.Add("_loseTurnMeter", (input) =>
        {
            return input.Replace("_loseTurnMeter", data.loseTurnMeter.ToString());
        });
        variableDict.Add("_moveEffectRange", (input) =>
        {
            return input.Replace("_moveEffectRange", data.moveEffectRange.ToString());
        });
        variableDict.Add("_percentage", (input) =>
        {
            return input.Replace("_percentage", (data.percentage * 100).ToString() + "%");
        });
    }
}
