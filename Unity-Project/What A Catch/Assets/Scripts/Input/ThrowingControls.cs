using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class ThrowingControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private Color activeColor;
    [SerializeField] private Color passiveColor;

    [SerializeField] private Vector3 startDownPos;
    [SerializeField] GameObject pointerBase;
    [SerializeField] GameObject pointerPoint;

    public enum AxisOption
    {
        // Options for which axes to use
        Both, // Use both
        OnlyHorizontal, // Only horizontal
        OnlyVertical // Only vertical
    }

    public int MovementRange = 100;
    public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
    public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
    public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

    bool m_UseX; // Toggle for using the x axis
    bool m_UseY; // Toggle for using the Y axis
    CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input
    CrossPlatformInputManager.VirtualAxis m_VerticalVirtualAxis; // Reference to the joystick in the cross platform input

    void OnEnable()
    {
        //print("ThrowingControls.OnEnable()");
        CreateVirtualAxes();
        startDownPos = transform.position;
        pointerBase.GetComponent<Image>().color = passiveColor;
        pointerPoint.GetComponent<Image>().color = passiveColor;
    }

    void Start()
    {
    }

    void UpdateVirtualAxes(Vector3 value)
    {
        var delta = startDownPos - value;
        delta.y = -delta.y;
        delta /= MovementRange;
        if (m_UseX)
        {
            m_HorizontalVirtualAxis.Update(-delta.x);
        }

        if (m_UseY)
        {
            m_VerticalVirtualAxis.Update(delta.y);
        }
    }

    void CreateVirtualAxes()
    {
        // set axes to use
        m_UseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
        m_UseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

        // create new axes based on axes to use
        if (m_UseX)
        {
            m_HorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
        }
        if (m_UseY)
        {
            m_VerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
            CrossPlatformInputManager.RegisterVirtualAxis(m_VerticalVirtualAxis);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        Vector3 newPos = Vector3.zero;

        if (m_UseX)
        {
            int delta = (int)(data.position.x - startDownPos.x);
            newPos.x = delta;
        }

        if (m_UseY)
        {
            int delta = (int)(data.position.y - startDownPos.y);
            newPos.y = delta;
        }
        /*
        transform.position = Vector3.ClampMagnitude(new Vector3
            (newPos.x, newPos.y, newPos.z),
            MovementRange) +
            m_StartPos;
            */

        pointerPoint.transform.position = Vector3.ClampMagnitude(new Vector3
            (newPos.x, newPos.y, newPos.z),
            MovementRange) +
            startDownPos;


        UpdateVirtualAxes(pointerPoint.transform.position);
    }
    public void OnPointerUp(PointerEventData data)
    {
        //print("OnPointerUp()");
        Vector3 delta = Vector3.zero;
        if (m_UseX)
        {
            int x = (int)(data.position.x - startDownPos.x);
            delta.x = x;
        }

        if (m_UseY)
        {
            int y = (int)(data.position.y - startDownPos.y);
            delta.y = y;
        }
        InputUI inputUI = GameObject.FindGameObjectWithTag("InputUI").GetComponent<InputUI>();
        inputUI.AcceptThrowDelta(delta);

        pointerPoint.transform.position = startDownPos;
        UpdateVirtualAxes(startDownPos);

        pointerBase.GetComponent<Image>().color = passiveColor;
        pointerPoint.GetComponent<Image>().color = passiveColor;
    }
    public void OnPointerDown(PointerEventData data)
    {
        //print("OnPointerDown()");
        startDownPos = data.position;
        pointerBase.transform.position = startDownPos;
        pointerPoint.transform.position = startDownPos;

        pointerBase.GetComponent<Image>().color = activeColor;
        pointerPoint.GetComponent<Image>().color = activeColor;

    }
    void OnDisable()
    {
        // remove the joysticks from the cross platform input
        if (m_UseX)
        {
            m_HorizontalVirtualAxis.Remove();
        }
        if (m_UseY)
        {
            m_VerticalVirtualAxis.Remove();
        }
    }

}


