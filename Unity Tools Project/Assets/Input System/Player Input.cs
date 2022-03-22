// GENERATED AUTOMATICALLY FROM 'Assets/Input System/Player Input.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Input"",
    ""maps"": [
        {
            ""name"": ""First Person Character"",
            ""id"": ""24d03c00-121e-4f0f-a9a6-86ef435fde24"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""458912a1-04de-4a55-80eb-6d8e23582b2e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""805ba226-ee04-480a-9cf2-0ea61ccfec23"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""9a49e2e3-a866-46a0-bbe4-9911a50f6f06"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraLook"",
                    ""type"": ""Value"",
                    ""id"": ""495c5af8-66c1-4e66-9023-0c608abf3ffc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Button"",
                    ""id"": ""a5aaf35a-ddb5-429c-9c61-69e4c8e7c279"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""37d479cc-2d1d-47f5-931f-ad948895d5ae"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e5a8cc66-052e-4181-9c1a-b42f9852def9"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""fb2c0672-141b-412a-9308-38d08b4e8141"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a8fae943-1062-4c56-b759-cbe3e8c8c0f2"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bc4dc1a8-b625-4868-97c0-0d53eddba179"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""474bd822-e29b-4787-a747-042b49571539"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54c80c2c-a2f1-4bc2-9b70-f13f210f784b"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ba8bc797-b8f7-4353-b755-d5af57b33d7d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraLook"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2009f8ad-6ec9-41e4-a280-34323f6c44ee"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // First Person Character
        m_FirstPersonCharacter = asset.FindActionMap("First Person Character", throwIfNotFound: true);
        m_FirstPersonCharacter_Movement = m_FirstPersonCharacter.FindAction("Movement", throwIfNotFound: true);
        m_FirstPersonCharacter_Jump = m_FirstPersonCharacter.FindAction("Jump", throwIfNotFound: true);
        m_FirstPersonCharacter_Sprint = m_FirstPersonCharacter.FindAction("Sprint", throwIfNotFound: true);
        m_FirstPersonCharacter_CameraLook = m_FirstPersonCharacter.FindAction("CameraLook", throwIfNotFound: true);
        m_FirstPersonCharacter_Fire = m_FirstPersonCharacter.FindAction("Fire", throwIfNotFound: true);
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

    // First Person Character
    private readonly InputActionMap m_FirstPersonCharacter;
    private IFirstPersonCharacterActions m_FirstPersonCharacterActionsCallbackInterface;
    private readonly InputAction m_FirstPersonCharacter_Movement;
    private readonly InputAction m_FirstPersonCharacter_Jump;
    private readonly InputAction m_FirstPersonCharacter_Sprint;
    private readonly InputAction m_FirstPersonCharacter_CameraLook;
    private readonly InputAction m_FirstPersonCharacter_Fire;
    public struct FirstPersonCharacterActions
    {
        private @PlayerInput m_Wrapper;
        public FirstPersonCharacterActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_FirstPersonCharacter_Movement;
        public InputAction @Jump => m_Wrapper.m_FirstPersonCharacter_Jump;
        public InputAction @Sprint => m_Wrapper.m_FirstPersonCharacter_Sprint;
        public InputAction @CameraLook => m_Wrapper.m_FirstPersonCharacter_CameraLook;
        public InputAction @Fire => m_Wrapper.m_FirstPersonCharacter_Fire;
        public InputActionMap Get() { return m_Wrapper.m_FirstPersonCharacter; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FirstPersonCharacterActions set) { return set.Get(); }
        public void SetCallbacks(IFirstPersonCharacterActions instance)
        {
            if (m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnMovement;
                @Jump.started -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnJump;
                @Sprint.started -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnSprint;
                @Sprint.performed -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnSprint;
                @Sprint.canceled -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnSprint;
                @CameraLook.started -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnCameraLook;
                @CameraLook.performed -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnCameraLook;
                @CameraLook.canceled -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnCameraLook;
                @Fire.started -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnFire;
                @Fire.performed -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnFire;
                @Fire.canceled -= m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface.OnFire;
            }
            m_Wrapper.m_FirstPersonCharacterActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
                @CameraLook.started += instance.OnCameraLook;
                @CameraLook.performed += instance.OnCameraLook;
                @CameraLook.canceled += instance.OnCameraLook;
                @Fire.started += instance.OnFire;
                @Fire.performed += instance.OnFire;
                @Fire.canceled += instance.OnFire;
            }
        }
    }
    public FirstPersonCharacterActions @FirstPersonCharacter => new FirstPersonCharacterActions(this);
    public interface IFirstPersonCharacterActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnCameraLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
    }
}
