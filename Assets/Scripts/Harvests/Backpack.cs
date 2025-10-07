using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    public BackpackData globalBackpack; // référence originale
    public BackpackData backpack;       // copie runtime

    public void Initialize(BackpackData runtimeData)
    {
        backpack = runtimeData;
    }
    void Start()
    {
        if (backpack == null && globalBackpack != null)
        {
            backpack = Instantiate(globalBackpack); // clone à l’instanciation
        }
    }
}
