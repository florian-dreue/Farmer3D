using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "fillable", menuName = "item/New fillable")]
public class FillableData : ToolData
{
    private int filling = 0;
    [SerializeField]
    private int maxFilling;

    public void FillTool(int quantity)
    {
        if(filling + quantity <= maxFilling)
        {
            filling += quantity;
        }
        else
        {
            filling = maxFilling;
        }
        
    }

    public void DrainTool(int quantity)
    {
        if(filling >= quantity)
        {
            filling -= quantity;
        }
        else
        {
            throw new InsufisentQuantityException();
        }
    }

    public int GetFilling() { return filling; }

    public int GetMaxFilling() { return maxFilling; }
}