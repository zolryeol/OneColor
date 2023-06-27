//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/Junhyeong/Scripts/TestInput.inputactions
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

public partial class @TestInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @TestInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TestInput"",
    ""maps"": [
        {
            ""name"": ""TestMaps"",
            ""id"": ""712f6a56-07be-47bd-bbc6-e70ee08946bf"",
            ""actions"": [
                {
                    ""name"": ""Test"",
                    ""type"": ""Button"",
                    ""id"": ""c6280a76-4612-418c-8cb4-2c9219f22092"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9ae845be-1891-4868-88d2-ea120973945d"",
                    ""path"": ""<Keyboard>/t"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Test"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MobileInput"",
            ""id"": ""6fb8e9e3-019c-4fc4-b346-7321c00f4bf8"",
            ""actions"": [
                {
                    ""name"": ""ESC"",
                    ""type"": ""Button"",
                    ""id"": ""90ecf342-aa8c-4962-8b71-63bf2444cde9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""c151424e-de30-4209-9e37-a9f424d38322"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Mobile"",
                    ""action"": ""ESC"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Mobile"",
            ""bindingGroup"": ""Mobile"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // TestMaps
        m_TestMaps = asset.FindActionMap("TestMaps", throwIfNotFound: true);
        m_TestMaps_Test = m_TestMaps.FindAction("Test", throwIfNotFound: true);
        // MobileInput
        m_MobileInput = asset.FindActionMap("MobileInput", throwIfNotFound: true);
        m_MobileInput_ESC = m_MobileInput.FindAction("ESC", throwIfNotFound: true);
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

    // TestMaps
    private readonly InputActionMap m_TestMaps;
    private ITestMapsActions m_TestMapsActionsCallbackInterface;
    private readonly InputAction m_TestMaps_Test;
    public struct TestMapsActions
    {
        private @TestInput m_Wrapper;
        public TestMapsActions(@TestInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Test => m_Wrapper.m_TestMaps_Test;
        public InputActionMap Get() { return m_Wrapper.m_TestMaps; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TestMapsActions set) { return set.Get(); }
        public void SetCallbacks(ITestMapsActions instance)
        {
            if (m_Wrapper.m_TestMapsActionsCallbackInterface != null)
            {
                @Test.started -= m_Wrapper.m_TestMapsActionsCallbackInterface.OnTest;
                @Test.performed -= m_Wrapper.m_TestMapsActionsCallbackInterface.OnTest;
                @Test.canceled -= m_Wrapper.m_TestMapsActionsCallbackInterface.OnTest;
            }
            m_Wrapper.m_TestMapsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Test.started += instance.OnTest;
                @Test.performed += instance.OnTest;
                @Test.canceled += instance.OnTest;
            }
        }
    }
    public TestMapsActions @TestMaps => new TestMapsActions(this);

    // MobileInput
    private readonly InputActionMap m_MobileInput;
    private IMobileInputActions m_MobileInputActionsCallbackInterface;
    private readonly InputAction m_MobileInput_ESC;
    public struct MobileInputActions
    {
        private @TestInput m_Wrapper;
        public MobileInputActions(@TestInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @ESC => m_Wrapper.m_MobileInput_ESC;
        public InputActionMap Get() { return m_Wrapper.m_MobileInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MobileInputActions set) { return set.Get(); }
        public void SetCallbacks(IMobileInputActions instance)
        {
            if (m_Wrapper.m_MobileInputActionsCallbackInterface != null)
            {
                @ESC.started -= m_Wrapper.m_MobileInputActionsCallbackInterface.OnESC;
                @ESC.performed -= m_Wrapper.m_MobileInputActionsCallbackInterface.OnESC;
                @ESC.canceled -= m_Wrapper.m_MobileInputActionsCallbackInterface.OnESC;
            }
            m_Wrapper.m_MobileInputActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ESC.started += instance.OnESC;
                @ESC.performed += instance.OnESC;
                @ESC.canceled += instance.OnESC;
            }
        }
    }
    public MobileInputActions @MobileInput => new MobileInputActions(this);
    private int m_MobileSchemeIndex = -1;
    public InputControlScheme MobileScheme
    {
        get
        {
            if (m_MobileSchemeIndex == -1) m_MobileSchemeIndex = asset.FindControlSchemeIndex("Mobile");
            return asset.controlSchemes[m_MobileSchemeIndex];
        }
    }
    public interface ITestMapsActions
    {
        void OnTest(InputAction.CallbackContext context);
    }
    public interface IMobileInputActions
    {
        void OnESC(InputAction.CallbackContext context);
    }
}