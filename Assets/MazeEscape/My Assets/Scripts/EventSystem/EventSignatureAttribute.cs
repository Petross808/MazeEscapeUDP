using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class EventSignatureAttribute : PropertyAttribute
{
    public Type[] Signature { get; }

    public EventSignatureAttribute(params Type[] types)
    {
        Signature = types;
    }
}

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class MustMatchEventSignatureAttribute : PropertyAttribute
{ }
