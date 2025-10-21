#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Stats;
using UnityEditor;
using UnityEngine;

namespace BagSerializer.Editor
{
    [CustomPropertyDrawer(typeof(SerializedTypeDictionary<>), true)]
    public class SerializedTypeDictionaryDrawer : PropertyDrawer
    {
        private bool _foldout;
        private static List<Type> _allValueTypes;
        
        [InitializeOnLoadMethod]
        private static void Init()
        {
            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                _allValueTypes?.Clear();
                _allValueTypes = null;
            };
        }
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _foldout = EditorGUI.Foldout(position, _foldout, label, true);
            if (!_foldout) return;

            EditorGUI.indentLevel++;

            SerializedProperty valuesProp = property.FindPropertyRelative("_values");
            
            if (valuesProp == null || valuesProp.arraySize == 0)
            {
                DrawButton(valuesProp);
                return;
            }
            
            DrawExistingElements(valuesProp);
            DrawButton(valuesProp);
        }

        private void DrawButton(SerializedProperty valuesProp)
        {
            if (GUILayout.Button("Add"))
                ShowDropdown(valuesProp);
            
            if (EditorGUI.indentLevel > 0)
                EditorGUI.indentLevel--;
        }
        private static void DrawExistingElements(SerializedProperty valuesProp)
        {
            for (int i = 0; i < valuesProp.arraySize; i++)
            {
                var element = valuesProp.GetArrayElementAtIndex(i);
                
                if (element == null ||
                    element.propertyType != SerializedPropertyType.ManagedReference ||
                    element.managedReferenceValue == null
                    )
                {
                    continue;
                }
                
                EditorGUILayout.BeginHorizontal("box");
                EditorGUILayout.PropertyField(
                    element,
                    new GUIContent(element.managedReferenceValue.GetType().Name),
                    true
                );
                if (GUILayout.Button("✖", GUILayout.Width(20)))
                {
                    valuesProp.DeleteArrayElementAtIndex(i);
                    i--;
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private void ShowDropdown(SerializedProperty valuesProp)
        {
            if (_allValueTypes == null) UpdateValueTypes();
            
            var existing = new HashSet<Type>();
            for (int i = 0; i < valuesProp.arraySize; i++)
            {
                var value = valuesProp.GetArrayElementAtIndex(i).managedReferenceValue;
                if (value != null) existing.Add(value.GetType());
            }

            var possible = _allValueTypes.Where(t => !existing.Contains(t)).ToList();
            GenericMenu menu = new GenericMenu();

            if (possible.Count == 0)
            {
                menu.AddDisabledItem(new GUIContent("No more values"));
            }
            else
            {
                foreach (var type in possible)
                {
                    menu.AddItem(new GUIContent(type.Name), false, () =>
                    {
                        valuesProp.serializedObject.Update();
                        valuesProp.arraySize++;
                        var newElement = valuesProp.GetArrayElementAtIndex(valuesProp.arraySize - 1);
                        newElement.managedReferenceValue = Activator.CreateInstance(type);
                        valuesProp.serializedObject.ApplyModifiedProperties();
                    });
                }
            }

            menu.ShowAsContext();
        }

        private static void UpdateValueTypes()
        {
            _allValueTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(tt => tt != null); }
                })
                .Where(t => typeof(Stat).IsAssignableFrom(t) && !t.IsAbstract)
                .ToList();
        }
    }
}
#endif