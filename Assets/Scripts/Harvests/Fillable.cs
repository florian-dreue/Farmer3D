using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fillable : MonoBehaviour
{
    private int filling = 0;
    [SerializeField]
    private FillableData fillableData;

    public void FillTool(int quantity)
    {
        if (filling + quantity <= fillableData.GetMaxFilling())
        {
            filling += quantity;
        }
        else
        {
            filling = fillableData.GetMaxFilling();
        }

    }

    public void DrainTool(int quantity)
    {
        if (filling >= quantity)
        {
            filling -= quantity;
        }
        else
        {
            throw new InsufisentQuantityException();
        }
    }

    public int GetFilling() { return filling; }
}
