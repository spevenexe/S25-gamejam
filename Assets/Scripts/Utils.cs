using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Utils
{
    public static Vector3 CubeBezier3(Vector3 start, Vector3 p1, Vector3 p2, Vector3 end, float t)
    {
        float r = 1f - t;
        float f0 = r * r * r;
        float f1 = r * r * t * 3;
        float f2 = r * t * t * 3;
        float f3 = t * t * t;
        return new Vector3(
            f0*start.x + f1*p1.x + f2*p2.x + f3*end.x,
            f0*start.y + f1*p1.y + f2*p2.y + f3*end.y,
            f0*start.z + f1*p1.z + f2*p2.z + f3*end.z
        );
    }

    public static string getKeys(PlayerInput PInput,string action)
    {
        string message = ""; 
        InputAction InteractAction = PInput.actions.FindAction(action);
        ReadOnlyArray<InputBinding> allKeys = InteractAction.bindings;
        InputBinding mask = InputBinding.MaskByGroup(PInput.currentControlScheme);
        int i;
        for(i = 0; i < allKeys.Count-1; i++)
        {
            if(mask.Matches(allKeys[i]))
                message+="["+allKeys[i].ToDisplayString()+"]/ ";
        }
        if(mask.Matches(allKeys[i]))
            message+="["+allKeys[i].ToDisplayString()+"]";
        return message;
    }
}
