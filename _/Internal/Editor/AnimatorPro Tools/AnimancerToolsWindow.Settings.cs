

#if UNITY_EDITOR

using System;

namespace AnimatorPro.Editor
{
    partial class AnimancerToolsWindow
    {
        /// <summary>[Editor-Only] Displays the <see cref="AnimancerSettings"/>.</summary>
        /// https://website/animatorpro/api/AnimatorPro.Editor/Settings
        internal sealed class Settings : Panel
        {
            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override string Name => "Settings";

            /// <inheritdoc/>
            public override string Instructions => null;

            /// <inheritdoc/>
            public override string HelpURL => Strings.DocsURLs.APIDocumentation + "." + nameof(Editor) + "/" + nameof(AnimancerSettings);

            /************************************************************************************************************************/

            [NonSerialized]
            private UnityEditor.Editor _SettingsEditor;

            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void OnEnable(int index)
            {
                base.OnEnable(index);

                var settings = AnimancerSettings.Instance;
                if (settings != null)
                    _SettingsEditor = UnityEditor.Editor.CreateEditor(settings);
            }

            /// <inheritdoc/>
            public override void OnDisable()
            {
                base.OnDisable();
                DestroyImmediate(_SettingsEditor);
            }

            /************************************************************************************************************************/

            /// <inheritdoc/>
            public override void DoBodyGUI()
            {
                if (_SettingsEditor != null)
                    _SettingsEditor.OnInspectorGUI();
            }

            /************************************************************************************************************************/
        }
    }
}

#endif

