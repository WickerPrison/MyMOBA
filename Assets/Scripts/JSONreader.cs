using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JSONreader
{
    public static CharacterData ReadJSON(TextAsset jsonAsset)
    {
        CharacterData characterData = JsonUtility.FromJson<CharacterData>(jsonAsset.text);
        return characterData;
    }
}
