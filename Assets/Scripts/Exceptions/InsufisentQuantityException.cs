using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsufisentQuantityException : Exception
{
    public InsufisentQuantityException() { }

    public InsufisentQuantityException(string message) : base(message) { }

}
