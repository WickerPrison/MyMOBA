//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.3
//     from Assets/PlayerControls.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControls : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerTurn"",
            ""id"": ""9e5f05a0-6703-4bb8-9ba8-15c94d8fa809"",
            ""actions"": [
                {
                    ""name"": ""LeftClick"",
                    ""type"": ""Button"",
                    ""id"": ""5bda21ef-725f-4ea6-aa4c-a43a20ecc178"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EndTurn"",
                    ""type"": ""Button"",
                    ""id"": ""cb8d62bb-6414-4901-b50d-436b1e131e24"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DeselectAbility"",
                    ""type"": ""Button"",
                    ""id"": ""8c92a7cd-5e48-49fb-8785-20da235c77b2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""One"",
                    ""type"": ""Button"",
                    ""id"": ""8b054a82-ba24-4cd8-9a2d-d33c643868ae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Two"",
                    ""type"": ""Button"",
                    ""id"": ""5749b1db-1b53-4543-b572-c36bc02198a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Three"",
                    ""type"": ""Button"",
                    ""id"": ""43e36458-1f60-4a93-ad4f-5ecade34c4dd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Four"",
                    ""type"": ""Button"",
                    ""id"": ""b2902641-62df-41be-9712-fbf1b212042f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Five"",
                    ""type"": ""Button"",
                    ""id"": ""f483de0e-f2cd-42c2-a70e-702e5346548d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Six"",
                    ""type"": ""Button"",
                    ""id"": ""a842b619-84c8-4455-9a84-d4609894943d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Ultimate"",
                    ""type"": ""Button"",
                    ""id"": ""c6cffcf9-170d-4b86-a1e6-0f1ac5f6625f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RightClick"",
                    ""type"": ""Button"",
                    ""id"": ""e4e571a4-4758-4213-8e28-7d5d0ae4ddaf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""88701f02-971d-49bb-8f0a-bfddc656a7a7"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""25784fd4-8f70-446c-83ff-29a4ca2b9d8c"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EndTurn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9e3221d7-708a-4036-8211-5dcc39c85abd"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DeselectAbility"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""23ca83c7-c55f-4f80-a9e7-6c327d542a10"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1bd3921a-aa0b-484d-bd90-55f43ee06b7c"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""94635469-52b0-46ce-81d3-308e9da3cc5c"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c754c586-1549-4c2a-983c-2b71c3051d64"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Four"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d1d0c77e-c903-42c4-8447-d15da6c7ee63"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Five"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ac7ea855-a127-4a72-afd9-6955bd4c0c27"",
                    ""path"": ""<Keyboard>/6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Six"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11c49d31-b134-40f0-81fb-c1d95be4ab3d"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Ultimate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e4eef49c-6c1c-4998-bb4e-ecac5a229141"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightClick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""CameraControls"",
            ""id"": ""d15215b9-f948-43f6-9972-c60d8f340e0a"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""4a2268a9-493b-40cd-b3c4-b4b2a9dfa3f1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""PassThrough"",
                    ""id"": ""512a75c6-fe70-4405-967c-f37b0609da55"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5a51f748-988b-45b0-ac7c-7c56080c6b47"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""668538a8-51ec-49de-bab9-52c34901459e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""da03144d-5e55-4dda-8e65-f81708a21e13"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ed40a300-a7e3-4b0c-80eb-9e9c9da58a70"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""776a82b8-8c24-4e1e-8b0d-bb76d6dd5d16"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9d807689-bda4-478a-83bd-73bb34020a80"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerTurn
        m_PlayerTurn = asset.FindActionMap("PlayerTurn", throwIfNotFound: true);
        m_PlayerTurn_LeftClick = m_PlayerTurn.FindAction("LeftClick", throwIfNotFound: true);
        m_PlayerTurn_EndTurn = m_PlayerTurn.FindAction("EndTurn", throwIfNotFound: true);
        m_PlayerTurn_DeselectAbility = m_PlayerTurn.FindAction("DeselectAbility", throwIfNotFound: true);
        m_PlayerTurn_One = m_PlayerTurn.FindAction("One", throwIfNotFound: true);
        m_PlayerTurn_Two = m_PlayerTurn.FindAction("Two", throwIfNotFound: true);
        m_PlayerTurn_Three = m_PlayerTurn.FindAction("Three", throwIfNotFound: true);
        m_PlayerTurn_Four = m_PlayerTurn.FindAction("Four", throwIfNotFound: true);
        m_PlayerTurn_Five = m_PlayerTurn.FindAction("Five", throwIfNotFound: true);
        m_PlayerTurn_Six = m_PlayerTurn.FindAction("Six", throwIfNotFound: true);
        m_PlayerTurn_Ultimate = m_PlayerTurn.FindAction("Ultimate", throwIfNotFound: true);
        m_PlayerTurn_RightClick = m_PlayerTurn.FindAction("RightClick", throwIfNotFound: true);
        // CameraControls
        m_CameraControls = asset.FindActionMap("CameraControls", throwIfNotFound: true);
        m_CameraControls_Move = m_CameraControls.FindAction("Move", throwIfNotFound: true);
        m_CameraControls_Zoom = m_CameraControls.FindAction("Zoom", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerTurn
    private readonly InputActionMap m_PlayerTurn;
    private IPlayerTurnActions m_PlayerTurnActionsCallbackInterface;
    private readonly InputAction m_PlayerTurn_LeftClick;
    private readonly InputAction m_PlayerTurn_EndTurn;
    private readonly InputAction m_PlayerTurn_DeselectAbility;
    private readonly InputAction m_PlayerTurn_One;
    private readonly InputAction m_PlayerTurn_Two;
    private readonly InputAction m_PlayerTurn_Three;
    private readonly InputAction m_PlayerTurn_Four;
    private readonly InputAction m_PlayerTurn_Five;
    private readonly InputAction m_PlayerTurn_Six;
    private readonly InputAction m_PlayerTurn_Ultimate;
    private readonly InputAction m_PlayerTurn_RightClick;
    public struct PlayerTurnActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerTurnActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @LeftClick => m_Wrapper.m_PlayerTurn_LeftClick;
        public InputAction @EndTurn => m_Wrapper.m_PlayerTurn_EndTurn;
        public InputAction @DeselectAbility => m_Wrapper.m_PlayerTurn_DeselectAbility;
        public InputAction @One => m_Wrapper.m_PlayerTurn_One;
        public InputAction @Two => m_Wrapper.m_PlayerTurn_Two;
        public InputAction @Three => m_Wrapper.m_PlayerTurn_Three;
        public InputAction @Four => m_Wrapper.m_PlayerTurn_Four;
        public InputAction @Five => m_Wrapper.m_PlayerTurn_Five;
        public InputAction @Six => m_Wrapper.m_PlayerTurn_Six;
        public InputAction @Ultimate => m_Wrapper.m_PlayerTurn_Ultimate;
        public InputAction @RightClick => m_Wrapper.m_PlayerTurn_RightClick;
        public InputActionMap Get() { return m_Wrapper.m_PlayerTurn; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerTurnActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerTurnActions instance)
        {
            if (m_Wrapper.m_PlayerTurnActionsCallbackInterface != null)
            {
                @LeftClick.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnLeftClick;
                @LeftClick.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnLeftClick;
                @LeftClick.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnLeftClick;
                @EndTurn.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnEndTurn;
                @EndTurn.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnEndTurn;
                @EndTurn.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnEndTurn;
                @DeselectAbility.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnDeselectAbility;
                @DeselectAbility.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnDeselectAbility;
                @DeselectAbility.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnDeselectAbility;
                @One.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnOne;
                @One.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnOne;
                @One.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnOne;
                @Two.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnTwo;
                @Two.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnTwo;
                @Two.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnTwo;
                @Three.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnThree;
                @Three.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnThree;
                @Three.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnThree;
                @Four.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFour;
                @Four.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFour;
                @Four.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFour;
                @Five.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFive;
                @Five.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFive;
                @Five.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnFive;
                @Six.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnSix;
                @Six.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnSix;
                @Six.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnSix;
                @Ultimate.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnUltimate;
                @Ultimate.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnUltimate;
                @Ultimate.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnUltimate;
                @RightClick.started -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnRightClick;
                @RightClick.performed -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnRightClick;
                @RightClick.canceled -= m_Wrapper.m_PlayerTurnActionsCallbackInterface.OnRightClick;
            }
            m_Wrapper.m_PlayerTurnActionsCallbackInterface = instance;
            if (instance != null)
            {
                @LeftClick.started += instance.OnLeftClick;
                @LeftClick.performed += instance.OnLeftClick;
                @LeftClick.canceled += instance.OnLeftClick;
                @EndTurn.started += instance.OnEndTurn;
                @EndTurn.performed += instance.OnEndTurn;
                @EndTurn.canceled += instance.OnEndTurn;
                @DeselectAbility.started += instance.OnDeselectAbility;
                @DeselectAbility.performed += instance.OnDeselectAbility;
                @DeselectAbility.canceled += instance.OnDeselectAbility;
                @One.started += instance.OnOne;
                @One.performed += instance.OnOne;
                @One.canceled += instance.OnOne;
                @Two.started += instance.OnTwo;
                @Two.performed += instance.OnTwo;
                @Two.canceled += instance.OnTwo;
                @Three.started += instance.OnThree;
                @Three.performed += instance.OnThree;
                @Three.canceled += instance.OnThree;
                @Four.started += instance.OnFour;
                @Four.performed += instance.OnFour;
                @Four.canceled += instance.OnFour;
                @Five.started += instance.OnFive;
                @Five.performed += instance.OnFive;
                @Five.canceled += instance.OnFive;
                @Six.started += instance.OnSix;
                @Six.performed += instance.OnSix;
                @Six.canceled += instance.OnSix;
                @Ultimate.started += instance.OnUltimate;
                @Ultimate.performed += instance.OnUltimate;
                @Ultimate.canceled += instance.OnUltimate;
                @RightClick.started += instance.OnRightClick;
                @RightClick.performed += instance.OnRightClick;
                @RightClick.canceled += instance.OnRightClick;
            }
        }
    }
    public PlayerTurnActions @PlayerTurn => new PlayerTurnActions(this);

    // CameraControls
    private readonly InputActionMap m_CameraControls;
    private ICameraControlsActions m_CameraControlsActionsCallbackInterface;
    private readonly InputAction m_CameraControls_Move;
    private readonly InputAction m_CameraControls_Zoom;
    public struct CameraControlsActions
    {
        private @PlayerControls m_Wrapper;
        public CameraControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CameraControls_Move;
        public InputAction @Zoom => m_Wrapper.m_CameraControls_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_CameraControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICameraControlsActions instance)
        {
            if (m_Wrapper.m_CameraControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnMove;
                @Zoom.started -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.performed -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
                @Zoom.canceled -= m_Wrapper.m_CameraControlsActionsCallbackInterface.OnZoom;
            }
            m_Wrapper.m_CameraControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Zoom.started += instance.OnZoom;
                @Zoom.performed += instance.OnZoom;
                @Zoom.canceled += instance.OnZoom;
            }
        }
    }
    public CameraControlsActions @CameraControls => new CameraControlsActions(this);
    public interface IPlayerTurnActions
    {
        void OnLeftClick(InputAction.CallbackContext context);
        void OnEndTurn(InputAction.CallbackContext context);
        void OnDeselectAbility(InputAction.CallbackContext context);
        void OnOne(InputAction.CallbackContext context);
        void OnTwo(InputAction.CallbackContext context);
        void OnThree(InputAction.CallbackContext context);
        void OnFour(InputAction.CallbackContext context);
        void OnFive(InputAction.CallbackContext context);
        void OnSix(InputAction.CallbackContext context);
        void OnUltimate(InputAction.CallbackContext context);
        void OnRightClick(InputAction.CallbackContext context);
    }
    public interface ICameraControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
