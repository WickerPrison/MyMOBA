using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
class Additions
{
    public Addition fly;
    public Addition stormlight;
    public Addition finishing;
    public Addition rooted;
    public Addition livingShardplate;
    public Addition silenced;
    public Addition stunned;
    public Addition pancakes;
    public Addition lifelight;
    public Addition metal;

    Dictionary<string, Addition> additionDict = new Dictionary<string, Addition>();

    public Addition GetAddition(string input)
    {
        if(additionDict.Count == 0)
        {
            SetupDictionary();
        }

        return additionDict[input];
    }

    void SetupDictionary()
    {
        additionDict.Add("fly", fly);
        additionDict.Add("stormlight", stormlight);
        additionDict.Add("finishing", finishing);
        additionDict.Add("rooted", rooted);
        additionDict.Add("livingShardplate", livingShardplate);
        additionDict.Add("silenced", silenced);
        additionDict.Add("stunned", stunned);
        additionDict.Add("pancakes", pancakes);
        additionDict.Add("lifelight", lifelight);
        additionDict.Add("metal", metal);
    }
}

[System.Serializable]
public class Addition
{
    public string name;
    public string description;
}
