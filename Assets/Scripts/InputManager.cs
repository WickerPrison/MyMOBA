using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //public PlayerControls controls;
    public List<PlayerControls> controls = new List<PlayerControls> ();
    
    TurnManager tm;

    // Start is called before the first frame update
    void Awake()
    {
        tm = GetComponent<TurnManager>();
        foreach(PlayerScript player in tm.players)
        {
            controls.Add(new PlayerControls());
        }
    }

    public void DisableAll()
    {
        foreach(PlayerControls control in controls)
        {
            control.Disable();
        }
    }

    private void OnEnable()
    {
        foreach(PlayerControls control in controls)
        {
            control.Enable();
        }
    }

    private void OnDisable()
    {
        foreach (PlayerControls control in controls)
        {
            control.Disable();
        }
    }
}
