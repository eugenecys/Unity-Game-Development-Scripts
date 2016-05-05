using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ViveInputManager : Singleton<ViveInputManager>
{
    public SteamVR_TrackedObject leftControllerObject;
    public SteamVR_TrackedObject rightControllerObject;

	public int leftControllerIndex = -1;
    public int rightControllerIndex = -1;

    private bool leftTriggerOn;
    private bool leftTouchpadOn;
    private bool leftApplicationmenuOn;
    private bool rightTriggerOn;
    private bool rightTouchpadOn;
    private bool rightApplicationmenuOn;
	private bool leftGripOn;
	private bool rightGripOn;

    public delegate void InputFunction(params object[] args);

    private Dictionary<InputType, InputFunction> inputMap;

    public enum InputType
    {
        LeftTriggerDown,
        RightTriggerDown,
        LeftTouchpadDown,
        RightTouchpadDown,
        LeftApplicationMenuDown,
        RightApplicationMenuDown,
        LeftTriggerUp,
        RightTriggerUp,
        LeftTouchpadUp,
        RightTouchpadUp,
        LeftApplicationMenuUp,
        RightApplicationMenuUp,

        LeftTouchpadAxis,
        RightTouchpadAxis,

        LeftTriggerAndTouchpad,
        RightTriggerAndTouchpad,

        LeftGripDown,
		RightGripDown,
		LeftGripUp,
		RightGripUp
    }

    void Awake()
    {
        if (inputMap == null)
        {
            inputMap = new Dictionary<InputType, InputFunction>();
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateLeft();
        updateRight();
    }

    public void registerFunction(InputFunction func, InputType type)
    {
        if (inputMap == null)
        {
            inputMap = new Dictionary<InputType, InputFunction>();
        }
        inputMap.Add(type, func);
    }

    void updateLeft()
    {
        if (leftControllerIndex != -1 && leftControllerIndex != rightControllerIndex)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(leftControllerIndex);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTriggerDown))
            {
                leftTriggerOn = true;
                inputMap[InputType.LeftTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpadDown))
            {
                leftTouchpadOn = true;
                inputMap[InputType.LeftTouchpadDown](device.GetAxis());
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.LeftApplicationMenuDown))
            {
                leftApplicationmenuOn = true;
                inputMap[InputType.LeftApplicationMenuDown]();
            }
			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && inputMap.ContainsKey(InputType.LeftGripDown))
			{
				leftGripOn = true;
				inputMap[InputType.LeftGripDown]();
			}

            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.LeftTriggerUp))
            {
                leftTriggerOn = false;
                inputMap[InputType.LeftTriggerUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.LeftTouchpadUp))
            {
                leftTouchpadOn = false;
                inputMap[InputType.LeftTouchpadUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.LeftApplicationMenuUp))
            {
                leftApplicationmenuOn = false;
                inputMap[InputType.LeftApplicationMenuUp]();
            }
			if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && inputMap.ContainsKey(InputType.LeftGripUp))
			{
				leftGripOn = false;
				inputMap[InputType.LeftGripUp]();
			}
            
            if (leftTouchpadOn && inputMap.ContainsKey(InputType.LeftTouchpadAxis))
            {
                inputMap[InputType.LeftTouchpadAxis](device.GetAxis());
            }
        }
        else
        {
            leftControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost);
            leftControllerObject.index = (SteamVR_TrackedObject.EIndex)leftControllerIndex;
            leftControllerObject.isValid = true;
        }
    }

    void updateRight()
    {
        if (rightControllerIndex != -1 && rightControllerIndex != leftControllerIndex)
        {
            SteamVR_Controller.Device device = SteamVR_Controller.Input(rightControllerIndex);

            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTriggerDown))
			{
				rightTriggerOn = true;
                inputMap[InputType.RightTriggerDown]();
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpadDown))
			{
				rightTouchpadOn = true;
                inputMap[InputType.RightTouchpadDown](device.GetAxis());
            }
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.RightApplicationMenuDown))
            {
                inputMap[InputType.RightApplicationMenuDown]();
            }
			if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip) && inputMap.ContainsKey(InputType.RightGripDown))
			{
				rightGripOn = true;
				inputMap[InputType.RightGripDown]();
			}
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger) && inputMap.ContainsKey(InputType.RightTriggerUp))
            {
				rightTriggerOn = false;
                inputMap[InputType.RightTriggerUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && inputMap.ContainsKey(InputType.RightTouchpadUp))
            {
				rightTouchpadOn = false;
                inputMap[InputType.RightTouchpadUp]();
            }
            if (device.GetPressUp(SteamVR_Controller.ButtonMask.ApplicationMenu) && inputMap.ContainsKey(InputType.RightApplicationMenuUp))
            {
                inputMap[InputType.RightApplicationMenuUp]();
            }
			if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip) && inputMap.ContainsKey(InputType.RightGripUp))
			{
				rightGripOn = false;
				inputMap[InputType.RightGripUp]();
			}
            if (rightTouchpadOn && inputMap.ContainsKey(InputType.RightTouchpadAxis))
            {
                inputMap[InputType.RightTouchpadAxis](device.GetAxis());
            }
        }
        else
        {
            rightControllerIndex = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost);
            rightControllerObject.index = (SteamVR_TrackedObject.EIndex)rightControllerIndex;
            rightControllerObject.isValid = true;
        }
    }
}
