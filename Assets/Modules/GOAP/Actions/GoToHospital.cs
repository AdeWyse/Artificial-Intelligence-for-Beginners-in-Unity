using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToHospital : GAction
{
    public override bool PrePerform()
    {
        Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHH");
        return true;    
    }

    public override bool PostPerform()
    {
        return true;
    }
}
