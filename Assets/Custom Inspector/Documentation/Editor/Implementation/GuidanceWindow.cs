using CustomInspector.Helpers;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CustomInspector.Documentation
{
    public class GuidanceWindow : EditorWindow
    {
        // Add menu item named "CustomInspector Documentation" to the Window menu
        [MenuItem("Window/CustomInspector Documentation")]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            var w = EditorWindow.GetWindow(typeof(GuidanceWindow), false, "CustomInspector Documentation", true);
            w.minSize = new Vector2(650, 280);
            w.maximized = true;
        }
        [SerializeField]
        Sprite icon = null;
        public Sprite Icon { get => icon; }

        SnippetsReader snippetsReader;
        Vector2 scrollPos = Vector2.zero;
        int currentIndex = -1; //defines first attribute that will be selected (-1 means none, because it is not valid)
        NewPropertyD Current
        {
            get { return (NewPropertyD)currentIndex; }
            set { currentIndex = (int)value; }
        }
        void OnGUI()
        {
            try
            {
                TryGUI();
            }
            catch (ExitGUIException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        void TryGUI()
        {
            //Define some sizes
            float leftBarPointWidth = 200; //each sub point
            float pageOuterSpacing = 10; //some space around inner pages
            // float pageInnerSpacing = 10; //some space in inner pages

            float iconSize = 50;
            float topBarHeight = 65;
            // Color defaultFontColor = GUI.color;

            // Get all names shown on left
            string[] names = Enum.GetNames(typeof(NewPropertyD));

            Rect leftBarRect = new Rect(pageOuterSpacing, pageOuterSpacing, leftBarPointWidth + Common.scrollbarThickness, position.height - 2 * pageOuterSpacing);

            // Move with arrows
            if (!EditorGUIUtility.editingTextField //prevent if we are selecting in a text field
                && Event.current.type == EventType.KeyDown
                && (Event.current.keyCode == KeyCode.UpArrow
                    || Event.current.keyCode == KeyCode.DownArrow))
            {

                if (Event.current.keyCode == KeyCode.UpArrow)
                {
                    currentIndex = Math.Max(currentIndex - 1, 0);
                }
                else // (Event.current.keyCode == KeyCode.DownArrow)
                {
                    currentIndex = Math.Min(currentIndex + 1, names.Length - 1);
                }


                float currentSelectedHeight = PropertyDList.MostUsedHeight + PropertyDList.headerDistance + currentIndex * PropertyDList.entryDistance;

                scrollPos.y = Math.Clamp(currentSelectedHeight - leftBarRect.height / 2, 0, PropertyDList.TotalHeight - leftBarRect.height);

                GUIUtility.keyboardControl = 0; // to deselct old label
                Repaint();
                return;
            }
            // Show left bar
            using (var scrollScope = new GUI.ScrollViewScope(leftBarRect, scrollPos, new Rect(0, 0, leftBarPointWidth, PropertyDList.TotalHeight)))
            {
                scrollPos = scrollScope.scrollPosition;

                Rect currentRect = new()
                {
                    width = leftBarPointWidth,
                };
                foreach ((string header, List<NewPropertyD> entrys) section in PropertyDList.Sections)
                {
                    currentRect.y += PropertyDList.headerSpacing;
                    currentRect.height = PropertyDList.headerHeight;
                    GUI.Label(currentRect, section.header);
                    currentRect.y += currentRect.height;

                    GUIStyle buttonStyle = GUI.skin.button;
                    buttonStyle.richText = true;
                    GUIContent[] gc = new GUIContent[1] { new GUIContent() };

                    foreach (NewPropertyD entry in section.entrys)
                    {
                        currentRect.y += PropertyDList.entrySpacing;
                        currentRect.height = PropertyDList.entryHeight;


                        gc[0].text = entry.ToString();

                        if (gc[0].text.Length > "Attribute".Length //grey out the "Attribute"
                            && gc[0].text[^"Attribute".Length..] == "Attribute")
                        {
                            gc[0].text = gc[0].text[..^"Attribute".Length] + "<color=#888888>Attribute</color>";
                        }
                        if (gc[0].text.Length > "Serializable".Length //grey out the "serializable"
                            && gc[0].text[.."Serializable".Length] == "Serializable")
                        {
                            gc[0].text = "<color=#888888>Serializable</color>" + gc[0].text["Serializable".Length..];
                        }

                        if (Current == entry)
                            GUI.Toolbar(currentRect, 0, gc, buttonStyle);
                        else
                        {
                            int res = GUI.Toolbar(currentRect, -1, gc, buttonStyle);
                            if (res != -1)
                            {
                                GUIUtility.keyboardControl = 0; // to deselct old label
                                Current = entry;
                            }
                        }

                        currentRect.y += currentRect.height;
                    }
                }
            }
            // Icon
            if (icon != null && icon.texture != null)
            {
                Rect iconRect = new(position.width - iconSize, 0, iconSize, iconSize);
                GUI.DrawTexture(iconRect, icon.texture);
            }
            // Site
            if (currentIndex == -1)
            {
                // draw start site
                Rect siteRect = new(leftBarRect.x + leftBarRect.width, 0, -1, position.height - topBarHeight);
                {
                    siteRect.width = position.width - siteRect.x;
                    siteRect = Shrinked(siteRect, pageOuterSpacing);

                    DocWelcomeSite.Draw(siteRect);
                }
            }
            else
            {
                // draw infos about specific attribute

                DrawAttributeSite(Current);

                void DrawAttributeSite(NewPropertyD property)
                {
                    //Header
                    Rect headerRect = new Rect(leftBarRect.x + leftBarRect.width, 0, -1, topBarHeight);
                    {
                        headerRect.width = position.width - headerRect.x;
                        EditorGUI.DrawRect(headerRect, InternalEditorStylesConvert.DarkerBackground);

                        GUIContent headerLabel = new GUIContent(Current.ToString());
                        GUI.skin.label.fontSize += 10;
                        Vector2 hSize = GUI.skin.label.CalcSize(headerLabel);
                        Rect hRect = new(headerRect.position + headerRect.size / 2 - hSize / 2, hSize);
                        GUI.Label(hRect, headerLabel);
                        GUI.skin.label.fontSize -= 10;
                    }
                    //Site
                    if (snippetsReader == null)
                    {
                        string[] guids = AssetDatabase.FindAssets("t:SnippetsReader");
                        if (guids.Length != 1)
                        {
                            Debug.LogError($"There should be exactly one CodeSnippets! (current: {guids.Length})");
                            return;
                        }
                        else
                        {
                            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);
                            snippetsReader = AssetDatabase.LoadAssetAtPath<SnippetsReader>(assetPath);
                        }

                        if (snippetsReader == null)
                        {
                            Debug.LogError("SnippetsReader not found. Please reopen window!");
                            return;
                        }
                    }
                    Rect siteRect = new(headerRect.x, topBarHeight, headerRect.width, position.height - topBarHeight);
                    {
                        siteRect = Shrinked(siteRect, pageOuterSpacing);

                        //Description
                        float descriptionHeight = siteRect.height * 0.2f;
                        Rect descriptionRect = new(siteRect)
                        {
                            x = siteRect.x,
                            y = siteRect.y,
                            height = descriptionHeight - pageOuterSpacing
                        };
                        if (!CustomInspectorAttributeDescriptions.descriptions.TryGetValue(Current, out (string text, bool validOnScriptableObjects) description))
                            description = ($"No description for '{Current}' available", true);
                        snippetsReader.DrawDescription(descriptionRect, description.text);

                        Rect pageRect = new()
                        {
                            x = siteRect.x,
                            y = descriptionRect.y + descriptionRect.height + pageOuterSpacing,
                            width = siteRect.width / 2 - pageOuterSpacing / 2,
                            height = siteRect.height - descriptionHeight,
                        };
                        //Code
                        snippetsReader.DrawCode(pageRect, Current);
                        //Preview
                        pageRect.x += pageRect.width + pageOuterSpacing;
                        if (description.validOnScriptableObjects)
                            snippetsReader.DrawPreview(pageRect, Current);
                        else
                            snippetsReader.DrawInvalidOnScriptables(pageRect);
                    }
                }
            }
        }
        public static Rect Shrinked(Rect r, float value)
        {
            r.x += value;
            r.y += value;
            r.width -= 2 * value;
            r.height -= 2 * value;
            return r;
        }
    }
}