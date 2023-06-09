

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AnimatorPro.Editor
{
    partial class AnimancerToolsWindow
    {
        /// <summary>[Editor-Only] [Pro-Only] Base class for panels in the <see cref="AnimancerToolsWindow"/>.</summary>
        /// <remarks>
        /// Documentation: <see href="https://website/animatorpro/docs/manual/tools">Animancer Tools</see>
        /// </remarks>
        /// https://website/animatorpro/api/AnimatorPro.Editor/Panel
        /// 
        public abstract class Panel
        {
            /************************************************************************************************************************/

            private readonly AnimBool FullAnimator = new AnimBool();
            private readonly AnimBool BodyAnimator = new AnimBool();

            private int _Index;

            /************************************************************************************************************************/

            /// <summary>Is this panel currently visible?</summary>
            public bool IsVisible => Instance._CurrentPanel == _Index || Instance._CurrentPanel < 0;

            /************************************************************************************************************************/

            /// <summary>Is the body of this panel currently visible?</summary>
            public bool IsExpanded
            {
                get { return Instance._CurrentPanel == _Index; }
                set
                {
                    if (value)
                        Instance._CurrentPanel = _Index;
                    else if (IsExpanded)
                        Instance._CurrentPanel = -1;
                }
            }

            /************************************************************************************************************************/

            /// <summary>The display name of this panel.</summary>
            public abstract string Name { get; }

            /// <summary>The usage instructions to display at the top of this panel.</summary>
            public abstract string Instructions { get; }

            /// <summary>The URL for the help button in the header to open.</summary>
            public virtual string HelpURL => Strings.DocsURLs.AnimancerTools;

            /// <summary>Called whenever the <see cref="Selection"/> changes.</summary>
            public virtual void OnSelectionChanged() { }

            /************************************************************************************************************************/

            /// <summary>Called by <see cref="AnimancerToolsWindow.OnEnable"/>.</summary>
            public virtual void OnEnable(int index)
            {
                _Index = index;
                FullAnimator.value = FullAnimator.target = IsVisible;
                BodyAnimator.value = BodyAnimator.target = IsExpanded;
            }

            /// <summary>Called by <see cref="AnimancerToolsWindow.OnDisable"/>.</summary>
            public virtual void OnDisable() { }

            /************************************************************************************************************************/

            /// <summary>Draws the GUI for this panel.</summary>
            public virtual void DoGUI()
            {
                var enabled = GUI.enabled;

                FullAnimator.target = IsVisible;

                if (EditorGUILayout.BeginFadeGroup(FullAnimator.faded))
                {
                    GUILayout.BeginVertical(EditorStyles.helpBox);

                    DoHeaderGUI();

                    BodyAnimator.target = IsExpanded;

                    if (EditorGUILayout.BeginFadeGroup(BodyAnimator.faded))
                    {
                        var instructions = Instructions;
                        if (!string.IsNullOrEmpty(instructions))
                            EditorGUILayout.HelpBox(instructions, MessageType.Info);

                        DoBodyGUI();
                    }
                    EditorGUILayout.EndFadeGroup();

                    GUILayout.EndVertical();
                }
                EditorGUILayout.EndFadeGroup();

                if (FullAnimator.isAnimating || BodyAnimator.isAnimating)
                    Repaint();

                GUI.enabled = enabled;
            }

            /************************************************************************************************************************/

            /// <summary>
            /// Draws the Header GUI for this panel which is displayed regardless of whether it is expanded or not.
            /// </summary>
            public virtual void DoHeaderGUI()
            {
                var area = AnimancerGUI.LayoutSingleLineRect(AnimancerGUI.SpacingMode.BeforeAndAfter);
                var click = GUI.Button(area, Name, EditorStyles.boldLabel);

                area.xMin = area.xMax - area.height;
                GUI.DrawTexture(area, HelpIcon);

                if (click)
                {
                    if (area.Contains(Event.current.mousePosition))
                    {
                        Application.OpenURL(HelpURL);
                        return;
                    }
                    else
                    {
                        IsExpanded = !IsExpanded;
                    }
                }
            }

            /************************************************************************************************************************/

            /// <summary>Draws the Body GUI for this panel which is only displayed while it is expanded.</summary>
            public abstract void DoBodyGUI();

            /************************************************************************************************************************/

            /// <summary>Asks the user where they want to save a modified asset, calls `modify` on it, and saves it.</summary>
            public static bool SaveModifiedAsset<T>(string saveTitle, string saveMessage,
                T obj, Action<T> modify) where T : Object
            {
                var originalPath = AssetDatabase.GetAssetPath(obj);

                var extension = Path.GetExtension(originalPath);
                if (extension[0] == '.')
                    extension = extension.Substring(1, extension.Length - 1);

                var directory = Path.GetDirectoryName(originalPath);

                var newName = Path.GetFileNameWithoutExtension(AssetDatabase.GenerateUniqueAssetPath(originalPath));
                var savePath = EditorUtility.SaveFilePanelInProject(saveTitle, newName, extension, saveMessage, directory);
                if (string.IsNullOrEmpty(savePath))
                    return false;

                if (originalPath != savePath)
                {
                    obj = Instantiate(obj);
                    AssetDatabase.CreateAsset(obj, savePath);
                }

                modify(obj);

                AssetDatabase.SaveAssets();

                return true;
            }

            /************************************************************************************************************************/

            private static Texture _HelpIcon;

            /// <summary>The help icon image used in the panel header.</summary>
            public static Texture HelpIcon
            {
                get
                {
                    if (_HelpIcon == null)
                        _HelpIcon = AnimancerGUI.LoadIcon("_Help");
                    return _HelpIcon;
                }
            }

            /************************************************************************************************************************/

            private static int _DropIndex;

            /// <summary>Adds any objects dropped in the `area` to the `list`.</summary>
            protected void HandleDragAndDropIntoList<T>(Rect area, IList<T> list, bool overwrite,
                Func<T, bool> validate = null) where T : Object
            {
                if (overwrite)
                {
                    _DropIndex = 0;
                    AnimancerGUI.HandleDragAndDrop(area, validate, (drop) =>
                    {
                        if (_DropIndex < list.Count)
                        {
                            RecordUndo();
                            list[_DropIndex++] = drop;
                        }
                    });
                }
                else
                {
                    AnimancerGUI.HandleDragAndDrop(area, validate, (drop) =>
                    {
                        list.Add(drop);
                    });
                }
            }

            /************************************************************************************************************************/
        }
    }
}

#endif

