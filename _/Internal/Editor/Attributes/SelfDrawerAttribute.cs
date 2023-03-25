

using UnityEngine;

#if UNITY_EDITOR
using AnimatorPro.Editor;
using UnityEditor;
#endif

namespace AnimatorPro
{
    /// <summary>[Editor-Conditional]
    /// A <see cref="PropertyAttribute"/> which draws itself rather than needing a separate <see cref="PropertyDrawer"/>.
    /// </summary>
    /// https://website/animatorpro/api/AnimatorPro/SelfDrawerAttribute
    /// 
    [System.Diagnostics.Conditional(Strings.UnityEditor)]
    public abstract class SelfDrawerAttribute : PropertyAttribute
    {
        /************************************************************************************************************************/
#if UNITY_EDITOR
        /************************************************************************************************************************/

        /// <summary>[Editor-Only] Can the GUI for the `property` be cached?</summary>
        public virtual bool CanCacheInspectorGUI(SerializedProperty property) => true;

        /// <summary>[Editor-Only] Calculates the height of the GUI for the `property`.</summary>
        public virtual float GetPropertyHeight(SerializedProperty property, GUIContent label) => AnimancerGUI.LineHeight;

        /// <summary>[Editor-Only] Draws the GUI for the `property`.</summary>
        public abstract void OnGUI(Rect area, SerializedProperty property, GUIContent label);

        /************************************************************************************************************************/
#endif
        /************************************************************************************************************************/
    }
}

#if UNITY_EDITOR

namespace AnimatorPro.Editor
{
    /// <summary>[Editor-Only] Draws the GUI for a <see cref="SelfDrawerAttribute"/> field.</summary>
    /// https://website/animatorpro/api/AnimatorPro.Editor/SelfDrawerDrawer
    /// 
    [CustomPropertyDrawer(typeof(SelfDrawerAttribute), true)]
    internal sealed class SelfDrawerDrawer : PropertyDrawer
    {
        /************************************************************************************************************************/

        /// <summary>Casts the <see cref="PropertyDrawer.attribute"/>.</summary>
        public SelfDrawerAttribute Attribute => (SelfDrawerAttribute)attribute;

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="SelfDrawerAttribute.CanCacheInspectorGUI"/>.</summary>
        public override bool CanCacheInspectorGUI(SerializedProperty property)
            => Attribute.CanCacheInspectorGUI(property);

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="SelfDrawerAttribute.GetPropertyHeight"/>.</summary>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => Attribute.GetPropertyHeight(property, label);

        /************************************************************************************************************************/

        /// <summary>Calls <see cref="SelfDrawerAttribute.OnGUI"/>.</summary>
        public override void OnGUI(Rect area, SerializedProperty property, GUIContent label)
            => Attribute.OnGUI(area, property, label);

        /************************************************************************************************************************/
    }
}

#endif

