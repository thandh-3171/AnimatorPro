

using System;

namespace AnimatorPro
{
    /// <summary>[Editor-Conditional]
    /// Causes an Inspector field in an <see cref="ITransition"/> to be drawn after its events where the events would
    /// normally be drawn last.
    /// </summary>
    /// https://website/animatorpro/api/AnimatorPro/DrawAfterEventsAttribute
    /// 
    [AttributeUsage(AttributeTargets.Field)]
    [System.Diagnostics.Conditional(Strings.UnityEditor)]
    public sealed class DrawAfterEventsAttribute : Attribute { }
}

