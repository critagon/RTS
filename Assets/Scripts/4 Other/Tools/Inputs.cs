using UnityEngine;

public static class Inputs
{
    #region Mouse
    public static bool LMBHold()
    {
        if (Input.GetMouseButton(0)) return true;
        else return false;
    }

    public static bool LMBDown()
    {
        if (Input.GetMouseButtonDown(0)) return true;
        else return false;
    }

    public static bool LMBUp()
    {
        if (Input.GetMouseButtonUp(0)) return true;
        else return false;
    }

    public static bool RMBHold()
    {
        if (Input.GetMouseButton(1)) return true;
        else return false;
    }

    public static bool RMBDown()
    {
        if (Input.GetMouseButtonDown(1)) return true;
        else return false;
    }

    public static bool RMBUp()
    {
        if (Input.GetMouseButtonUp(1)) return true;
        else return false;
    }

    public static bool MMBHold()
    {
        if (Input.GetMouseButton(2)) return true;
        else return false;
    }

    public static bool MMBDown()
    {
        if (Input.GetMouseButtonDown(2)) return true;
        else return false;
    }

    public static bool MMBUp()
    {
        if (Input.GetMouseButtonUp(2)) return true;
        else return false;
    }

    #endregion

    #region Misc Keys
    public static bool CtrlHold()
    {
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftApple) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.RightApple)) return true;
        else return false;
    }
    public static bool ShiftHold()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) return true;
        else return false;

    }
    public static bool AltHold()
    {
        if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) return true;
        else return false;
    }

    public static bool EscapeDown()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) return true;
        else return false;
    }

    #endregion
}
