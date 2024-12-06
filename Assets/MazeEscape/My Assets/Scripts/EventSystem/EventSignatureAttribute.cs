using System;
using UnityEngine;


[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class EventSignatureAttribute : PropertyAttribute
{
    public Type[] Signature { get; }

    public EventSignatureAttribute(params Type[] types)
    {
        Signature = types;
    }
}

namespace EventSystemInternal
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MustMatchEventSignatureGetAttribute : PropertyAttribute
    { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class MustMatchEventSignatureRaiseAttribute : PropertyAttribute
    { }
}