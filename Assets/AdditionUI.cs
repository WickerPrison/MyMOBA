using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdditionUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;

    public void Setup(Addition addition)
    {
        title.text = addition.name;
        description.text = addition.description;
    }

    public void Hide()
    {
        title.text = "";
        description.text = "";
    }
}
