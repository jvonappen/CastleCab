//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/LukeTesting/InputSystem/PlayerControls.inputactions
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
            ""name"": ""Controls"",
            ""id"": ""ad0c9cd5-6517-4bc6-9a50-890e9846eb50"",
            ""actions"": [
                {
                    ""name"": ""Acceleration"",
                    ""type"": ""Value"",
                    ""id"": ""d091bc05-98cc-496a-bfe4-6941cf44e2f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Steering"",
                    ""type"": ""Value"",
                    ""id"": ""af6ed0d1-4391-45a5-a951-e267b84fef2a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""TailWhip"",
                    ""type"": ""Button"",
                    ""id"": ""085c4b4a-05f6-4bb0-8fe2-99b1c139d527"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""4cf0fca4-08cb-453a-87ea-4167e4b84a32"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""536d6838-1157-4214-9912-bd4c2546eeae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reverse"",
                    ""type"": ""Value"",
                    ""id"": ""41622d10-1084-4de4-b517-7e8b22301341"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Backflip"",
                    ""type"": ""Button"",
                    ""id"": ""e09e04a9-49ed-421f-a4f3-f0c17da74cd4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""BarrelRoll"",
                    ""type"": ""Button"",
                    ""id"": ""1b0ac286-96d7-42ff-8753-60f84cbfb3fb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""16f97ddb-45cf-43fc-8845-03964cec6a46"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""50b31332-db1c-4af9-be4a-5b0b0cd71c1c"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""3c4d8720-2d7f-4d0d-9609-cee4cdfb08b3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""366e3c12-7cd7-4b62-8a16-9a64ef30b711"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Acceleration"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""6dac5d7e-21ac-4713-8f4c-0ac96db4be1a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""c7c9366e-1193-4fae-b8c4-7aa5f9bd2955"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1e851093-4481-497b-bf65-527a7a9735ea"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""1D Axis"",
                    ""id"": ""8d148b6f-cad8-4aa1-a190-e5c05434278e"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""834f3fa5-ab3a-4432-9527-b93452d7396b"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7dd40250-bb04-462a-b4da-9a6e4ce21491"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Steering"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ab0b5b9d-a093-4f8d-b6b5-51e58402b7f3"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TailWhip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81af09df-5b55-48f5-840b-e9d5f9fdf0d4"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TailWhip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""261328fb-4efe-4547-8a33-2c7772fbe515"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f6ebfee8-4e66-40c4-a3e8-296171ef7227"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7fe84ff9-c76d-4eb1-aef3-cd7aa633a840"",
                    ""path"": ""<Joystick>/{Hatswitch}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e521cc2a-6c3c-4837-94c8-710f740ac134"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8af66891-dd52-4809-bacc-bbaedc338d65"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Controller"",
                    ""id"": ""99ba21f6-75aa-48a0-bdf2-ebaeede71e7f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverse"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""04044f88-6c4d-41a7-a3d3-89acf36e19ae"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""2eeefc4d-3381-4520-bb4e-e1f5fbba5860"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverse"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""7394c7ac-e47f-4b14-933e-a493e7dc39d7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reverse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9327b074-0e51-4cfa-bac2-d968ce0c2666"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Backflip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0130c7a0-5b13-4f58-9641-cb700dd641db"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Backflip"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""465a9372-acdc-4b4e-a0d3-31aec1eecf71"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BarrelRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f21136f9-4083-48df-81f1-e1bc72431902"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""BarrelRoll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""f0649746-fa63-4800-9523-5e64729da5a1"",
            ""actions"": [
                {
                    ""name"": ""MenuNavigation"",
                    ""type"": ""Button"",
                    ""id"": ""65d69f26-30f9-4d64-bd46-dab9c537187f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dc27657f-5984-4be4-b4d4-0acdd5baafef"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuNavigation"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Acceleration = m_Controls.FindAction("Acceleration", throwIfNotFound: true);
        m_Controls_Steering = m_Controls.FindAction("Steering", throwIfNotFound: true);
        m_Controls_TailWhip = m_Controls.FindAction("TailWhip", throwIfNotFound: true);
        m_Controls_Look = m_Controls.FindAction("Look", throwIfNotFound: true);
        m_Controls_Boost = m_Controls.FindAction("Boost", throwIfNotFound: true);
        m_Controls_Reverse = m_Controls.FindAction("Reverse", throwIfNotFound: true);
        m_Controls_Backflip = m_Controls.FindAction("Backflip", throwIfNotFound: true);
        m_Controls_BarrelRoll = m_Controls.FindAction("BarrelRoll", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_MenuNavigation = m_UI.FindAction("MenuNavigation", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private IControlsActions m_ControlsActionsCallbackInterface;
    private readonly InputAction m_Controls_Acceleration;
    private readonly InputAction m_Controls_Steering;
    private readonly InputAction m_Controls_TailWhip;
    private readonly InputAction m_Controls_Look;
    private readonly InputAction m_Controls_Boost;
    private readonly InputAction m_Controls_Reverse;
    private readonly InputAction m_Controls_Backflip;
    private readonly InputAction m_Controls_BarrelRoll;
    public struct ControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Acceleration => m_Wrapper.m_Controls_Acceleration;
        public InputAction @Steering => m_Wrapper.m_Controls_Steering;
        public InputAction @TailWhip => m_Wrapper.m_Controls_TailWhip;
        public InputAction @Look => m_Wrapper.m_Controls_Look;
        public InputAction @Boost => m_Wrapper.m_Controls_Boost;
        public InputAction @Reverse => m_Wrapper.m_Controls_Reverse;
        public InputAction @Backflip => m_Wrapper.m_Controls_Backflip;
        public InputAction @BarrelRoll => m_Wrapper.m_Controls_BarrelRoll;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void SetCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterface != null)
            {
                @Acceleration.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAcceleration;
                @Acceleration.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAcceleration;
                @Acceleration.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnAcceleration;
                @Steering.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
                @Steering.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
                @Steering.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnSteering;
                @TailWhip.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTailWhip;
                @TailWhip.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTailWhip;
                @TailWhip.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnTailWhip;
                @Look.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnLook;
                @Boost.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBoost;
                @Reverse.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReverse;
                @Reverse.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReverse;
                @Reverse.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnReverse;
                @Backflip.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBackflip;
                @Backflip.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBackflip;
                @Backflip.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBackflip;
                @BarrelRoll.started -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBarrelRoll;
                @BarrelRoll.performed -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBarrelRoll;
                @BarrelRoll.canceled -= m_Wrapper.m_ControlsActionsCallbackInterface.OnBarrelRoll;
            }
            m_Wrapper.m_ControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Acceleration.started += instance.OnAcceleration;
                @Acceleration.performed += instance.OnAcceleration;
                @Acceleration.canceled += instance.OnAcceleration;
                @Steering.started += instance.OnSteering;
                @Steering.performed += instance.OnSteering;
                @Steering.canceled += instance.OnSteering;
                @TailWhip.started += instance.OnTailWhip;
                @TailWhip.performed += instance.OnTailWhip;
                @TailWhip.canceled += instance.OnTailWhip;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Reverse.started += instance.OnReverse;
                @Reverse.performed += instance.OnReverse;
                @Reverse.canceled += instance.OnReverse;
                @Backflip.started += instance.OnBackflip;
                @Backflip.performed += instance.OnBackflip;
                @Backflip.canceled += instance.OnBackflip;
                @BarrelRoll.started += instance.OnBarrelRoll;
                @BarrelRoll.performed += instance.OnBarrelRoll;
                @BarrelRoll.canceled += instance.OnBarrelRoll;
            }
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_MenuNavigation;
    public struct UIActions
    {
        private @PlayerControls m_Wrapper;
        public UIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MenuNavigation => m_Wrapper.m_UI_MenuNavigation;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @MenuNavigation.started -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigation;
                @MenuNavigation.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigation;
                @MenuNavigation.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnMenuNavigation;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MenuNavigation.started += instance.OnMenuNavigation;
                @MenuNavigation.performed += instance.OnMenuNavigation;
                @MenuNavigation.canceled += instance.OnMenuNavigation;
            }
        }
    }
    public UIActions @UI => new UIActions(this);
    public interface IControlsActions
    {
        void OnAcceleration(InputAction.CallbackContext context);
        void OnSteering(InputAction.CallbackContext context);
        void OnTailWhip(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnReverse(InputAction.CallbackContext context);
        void OnBackflip(InputAction.CallbackContext context);
        void OnBarrelRoll(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnMenuNavigation(InputAction.CallbackContext context);
    }
}
