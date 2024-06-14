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
    int damage;
    int duration;

    Dictionary<string, Func<string, string>> variableDict = new Dictionary<string, Func<string, string>>();

    private void Start()
    {
        additions = JsonUtility.FromJson<Additions>(additionsJson.text);
        SetUpTooltipVariables();
    }

    public void SetupTooltip(AbilityData abilityData, PlayerScript currentPlayer)
    {
        damage = abilityData.damage;
        duration = abilityData.duration;

        playerScript = currentPlayer;
        abilityName.text = abilityData.name;
        APcost.text = "AP Cost: " + abilityData.APcost;
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
        //image.enabled = true;
        abilityName.gameObject.SetActive(true);
        APcost.gameObject.SetActive(true);
        cooldown.gameObject.SetActive(true);
        range.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        additionsObject.SetActive(true);
    }

    public void HideTooltip()
    {
        //image.enabled = false;
        abilityName.gameObject.SetActive(false);
        APcost.gameObject.SetActive(false);
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
            return input.Replace("_damage", damage.ToString());
        });
        variableDict.Add("_duration", (input) =>
        {
            return input.Replace("_duration", duration.ToString());
        });
    }
}


[System.Serializable]
class Additions
{
    Addition error = new Addition("error", "error");
    public Addition fly;
    public Addition stormlight;
    public Addition finishing;
    public Addition rooted;
    public Addition livingShardplate;

    public Addition GetAddition(string input)
    {
        switch (input)
        {
            case "fly":
                return fly;
            case "stormlight":
                return stormlight;
            case "finishing":
                return finishing;
            case "rooted":
                return rooted;
            case "livingShardplate":
                return livingShardplate;
            default:
                return error;
        }
    }
}

[System.Serializable]
public class Addition
{
    public Addition(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public string name;
    public string description;
}
