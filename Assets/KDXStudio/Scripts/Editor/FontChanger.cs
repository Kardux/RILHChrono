#region Author
/************************************************************************************************************
Author: BODEREAU Roy
Website: http://roy-bodereau.fr/
GitHub: https://github.com/Kardux
LinkedIn: https://fr.linkedin.com/pub/roy-bodereau/b2/94/82b
************************************************************************************************************/
#endregion Author

#region Copyright
/************************************************************************************************************
CC-BY-SA 4.0
http://creativecommons.org/licenses/by-sa/4.0/
Cette oeuvre est mise a disposition selon les termes de la Licence Creative Commons Attribution 4.0
Partage dans les Memes Conditions 4.0 International.
************************************************************************************************************/
#endregion Copyright

namespace KDX.Editor
{
    using UnityEngine;
    using System.Collections;
    using UnityEditor;
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine.UI;
    using System;
    using UnityEditor.SceneManagement;

    /// <summary>
    /// An editor tool to quickly change all UGUI Texts (and also MeshTexts if needed) on all opened scenes.
    /// Hint : to set ALL project texts, open all project scenes additively in editor and run this tool afterward.
    /// </summary>
    public class FontChanger : EditorWindow
    {
        #region Static Fields
        private static FontChanger INSTANCE;

        private static List<Font> _fonts;
        private static string[] _fontsStrings;

        //Character
        private static bool _changeFont;
        private static bool _changeFontStyle;
        private static bool _changeFontSize;
        private static bool _changeLineSpacing;
        private static bool _changeRichText;

        private static int _font;
        private static FontStyle _fontStyle;
        private static int _fontSize;
        private static int _lineSpacing;
        private static bool _richText;

        //Paragraph
        private static bool _changeAlignment;
        private static bool _changeAlignByGeometry;
        private static bool _changeHorizontalOverflow;
        private static bool _changeVerticalOverflow;
        private static bool _changeBestFit;
        private static bool _changeMinSize;
        private static bool _changeMaxSize;

        private static TextAnchor _alignment;
        private static bool _alignByGeometry;
        private static HorizontalWrapMode _horizontalOverflow;
        private static VerticalWrapMode _verticalOverflow;
        private static bool _bestFit;
        private static int _minSize;
        private static int _maxSize;

        //Other
        private static bool _changeColor;
        private static bool _changeMaterial;
        private static bool _changeRaycastTarget;

        private static Color _color;
        private static Material _material;
        private static bool _raycastTarget;

        private static bool _applySettingsOnTextMeshes;
        private static string _displayDialog;
        #endregion Static Fields

        #region EditorWindow
        [MenuItem("KDX/Editor Tools/Change scenes fonts", false, 40)]
        private static void Init()
        {
            if(INSTANCE == null)
            {
                INSTANCE = CreateInstance<FontChanger>();
            }

            _fonts = new List<Font>();
            GetAllFontsInProject_r(Application.dataPath);

            if(_fonts.Count > 0)
            {
                _fontsStrings = new string[_fonts.Count];
                for(int i = 0; i < _fonts.Count; i++)
                {
                    _fontsStrings[i] = _fonts[i].name;
                }
            }

            _applySettingsOnTextMeshes = false;
            _displayDialog = "";

            if(_fonts.Count == 0)
            {
                _displayDialog = "No font find on project (fonts should be .ttf or .otf format).";
            }

            //Character
            _changeFont = true;
            _changeFontStyle = false;
            _changeFontSize = true;
            _changeLineSpacing = false;
            _changeRichText = true;

            _font = 0;
            _fontStyle = FontStyle.Normal;
            _fontSize = 12;
            _lineSpacing = 1;
            _richText = true;

            //Paragraph
            _changeAlignment = true;
            _changeAlignByGeometry = false;
            _changeHorizontalOverflow = false;
            _changeVerticalOverflow = false;
            _changeBestFit = true;
            _changeMinSize = true;
            _changeMaxSize = true;

            _alignment = TextAnchor.MiddleCenter;
            _alignByGeometry = false;
            _horizontalOverflow = HorizontalWrapMode.Wrap;
            _verticalOverflow = VerticalWrapMode.Truncate;
            _bestFit = true;
            _minSize = 4;
            _maxSize = 128;

            //Other
            _changeColor = false;
            _changeMaterial = false;
            _changeRaycastTarget = false;

            _color = Color.black;
            _material = null;
            _raycastTarget = true;

            if(_fonts.Count > 0)
            {
                INSTANCE.Show();
            }
        }

        private void OnGUI()
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            GUILayout.Label("Change the text settings and choose the ones you want to apply :", EditorStyles.boldLabel, GUILayout.ExpandWidth(true));
            GUILayout.BeginHorizontal();
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("Settings to change", labelStyle, GUILayout.Width(200.0f));
            labelStyle.alignment = TextAnchor.MiddleRight;
            GUILayout.Label("Apply your settings", labelStyle, GUILayout.Width(200.0f));
            GUILayout.EndHorizontal();

            labelStyle = new GUIStyle(EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            labelStyle.alignment = TextAnchor.MiddleLeft;
            GUILayout.Label("⇩⇩", labelStyle, GUILayout.Width(200.0f));
            labelStyle.alignment = TextAnchor.MiddleRight;
            GUILayout.Label("⇩⇩", labelStyle, GUILayout.Width(200.0f));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            #region Settings
            //Settings
            GUILayout.BeginVertical(GUILayout.Width(385.0f));

            EditorGUI.indentLevel++;
            GUILayout.Label("Character", EditorStyles.boldLabel);
            _font = EditorGUILayout.Popup("Font", _font, _fontsStrings);
            _fontStyle = (FontStyle)EditorGUILayout.Popup("Font Style", (int)_fontStyle, Enum.GetNames(typeof(FontStyle)));
            _fontSize = EditorGUILayout.IntField("Font Size", _fontSize);
            _lineSpacing = EditorGUILayout.IntField("Line Spacing", _lineSpacing);
            _richText = EditorGUILayout.Toggle("Rich Text", _richText);

            EditorGUILayout.Space();
            GUILayout.Label("Paragraph", EditorStyles.boldLabel);
            _alignment = (TextAnchor)EditorGUILayout.Popup("Alignment", (int)_alignment, Enum.GetNames(typeof(TextAnchor)));
            _alignByGeometry = EditorGUILayout.Toggle("Align By Geometry", _alignByGeometry);
            _horizontalOverflow = (HorizontalWrapMode)EditorGUILayout.Popup("Horizontal Overflow", (int)_horizontalOverflow, Enum.GetNames(typeof(HorizontalWrapMode)));
            _verticalOverflow = (VerticalWrapMode)EditorGUILayout.Popup("Vertical Overflow", (int)_verticalOverflow, Enum.GetNames(typeof(VerticalWrapMode)));
            _bestFit = EditorGUILayout.Toggle("Best Fit", _bestFit);
            _minSize = EditorGUILayout.IntField("Min Size", _minSize);
            _maxSize = EditorGUILayout.IntField("Max Size", _maxSize);
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            _color = EditorGUILayout.ColorField("Color", _color);
            _material = (Material)EditorGUILayout.ObjectField("Material", _material, typeof(Material), false);
            _raycastTarget = EditorGUILayout.Toggle("Raycast Target", _raycastTarget);

            GUILayout.EndVertical();
            #endregion

            #region Apply Settings
            //Apply settings
            GUILayout.BeginVertical(GUILayout.Width(15.0f));

            GUILayout.Label("", EditorStyles.boldLabel);
            _changeFont = EditorGUILayout.Toggle(_changeFont);
            _changeFontStyle = EditorGUILayout.Toggle(_changeFontStyle);
            _changeFontSize = EditorGUILayout.Toggle(_changeFontSize);
            _changeLineSpacing = EditorGUILayout.Toggle(_changeLineSpacing);
            _changeRichText = EditorGUILayout.Toggle(_changeRichText);

            EditorGUILayout.Space();
            GUILayout.Label("", EditorStyles.boldLabel);
            _changeAlignment = EditorGUILayout.Toggle(_changeAlignment);
            _changeAlignByGeometry = EditorGUILayout.Toggle(_changeAlignByGeometry);
            _changeHorizontalOverflow = EditorGUILayout.Toggle(_changeHorizontalOverflow);
            _changeVerticalOverflow = EditorGUILayout.Toggle(_changeVerticalOverflow);
            _changeBestFit = EditorGUILayout.Toggle(_changeBestFit);
            _changeMinSize = EditorGUILayout.Toggle(_changeMinSize);
            _changeMaxSize = EditorGUILayout.Toggle(_changeMaxSize);

            EditorGUILayout.Space();
            _changeColor = EditorGUILayout.Toggle(_changeColor);
            _changeMaterial = EditorGUILayout.Toggle(_changeMaterial);
            _changeRaycastTarget = EditorGUILayout.Toggle(_changeRaycastTarget);

            GUILayout.EndVertical();
            #endregion

            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Apply settings on TextMeshe(s) ? (can be heavy)");
            GUILayout.FlexibleSpace();
            _applySettingsOnTextMeshes = EditorGUILayout.Toggle(_applySettingsOnTextMeshes, GUILayout.ExpandWidth(false));
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();

            if(GUILayout.Button("Apply selected settings", GUILayout.ExpandWidth(true)))
            {
                _displayDialog = "";
                int uguiTextsChanged = 0;
                int textMeshesChanged = 0;

                Text[] texts = FindObjectsOfType<Text>();
                for(int i = 0; i < texts.Length; i++)
                {
                    //Character
                    bool changed = false;
                    if(_changeFont && texts[i].font != _fonts[_font])
                    {
                        texts[i].font = _fonts[_font];
                        changed = true;
                    }

                    if(_changeFontStyle && texts[i].fontStyle != _fontStyle)
                    {
                        texts[i].fontStyle = _fontStyle;
                        changed = true;
                    }

                    if(_changeFontSize && texts[i].fontSize != _fontSize)
                    {
                        texts[i].fontSize = _fontSize;
                        changed = true;
                    }

                    if(_changeLineSpacing && texts[i].lineSpacing != _lineSpacing)
                    {
                        texts[i].lineSpacing = _lineSpacing;
                        changed = true;
                    }

                    if(_changeRichText && texts[i].supportRichText != _richText)
                    {
                        texts[i].supportRichText = _richText;
                        changed = true;
                    }

                    //Paragraph
                    if(_changeAlignment && texts[i].alignment != _alignment)
                    {
                        texts[i].alignment = _alignment;
                        changed = true;
                    }

                    if(_changeAlignByGeometry && texts[i].alignByGeometry != _alignByGeometry)
                    {
                        texts[i].alignByGeometry = _alignByGeometry;
                        changed = true;
                    }

                    if(_changeHorizontalOverflow && texts[i].horizontalOverflow != _horizontalOverflow)
                    {
                        texts[i].horizontalOverflow = _horizontalOverflow;
                        changed = true;
                    }

                    if(_changeVerticalOverflow && texts[i].verticalOverflow != _verticalOverflow)
                    {
                        texts[i].verticalOverflow = _verticalOverflow;
                        changed = true;
                    }

                    if(_changeBestFit && texts[i].resizeTextForBestFit != _bestFit)
                    {
                        texts[i].resizeTextForBestFit = _bestFit;
                        changed = true;
                    }

                    if(_changeMinSize && texts[i].resizeTextMinSize != _minSize)
                    {
                        texts[i].resizeTextMinSize = _minSize;
                        changed = true;
                    }

                    if(_changeMaxSize && texts[i].resizeTextMaxSize != _maxSize)
                    {
                        texts[i].resizeTextMaxSize = _maxSize;
                        changed = true;
                    }

                    //Other
                    if(_changeColor && texts[i].color != _color)
                    {
                        texts[i].color = _color;
                        changed = true;
                    }

                    if(_changeMaterial && texts[i].material != _material)
                    {
                        texts[i].material = _material;
                        changed = true;
                    }

                    if(_changeRaycastTarget && texts[i].raycastTarget != _raycastTarget)
                    {
                        texts[i].raycastTarget = _raycastTarget;
                        changed = true;
                    }

                    if(changed)
                    {
                        uguiTextsChanged++;
                    }
                }

                if(_applySettingsOnTextMeshes)
                {
                    TextMesh[] textMeshes = FindObjectsOfType<TextMesh>();
                    for(int i = 0; i < textMeshes.Length; i++)
                    {
                        //Character
                        bool changed = false;
                        if(_changeFont && textMeshes[i].font != _fonts[_font])
                        {
                            textMeshes[i].font = _fonts[_font];
                            changed = true;
                        }

                        if(_changeFontStyle && textMeshes[i].fontStyle != _fontStyle)
                        {
                            textMeshes[i].fontStyle = _fontStyle;
                            changed = true;
                        }

                        if(_changeFontSize && textMeshes[i].fontSize != _fontSize)
                        {
                            textMeshes[i].fontSize = _fontSize;
                            changed = true;
                        }

                        if(_changeLineSpacing && textMeshes[i].lineSpacing != _lineSpacing)
                        {
                            textMeshes[i].lineSpacing = _lineSpacing;
                            changed = true;
                        }

                        if(_changeRichText && textMeshes[i].richText != _richText)
                        {
                            textMeshes[i].richText = _richText;
                            changed = true;
                        }

                        //Paragraph
                        if(_changeAlignment && (
                            (textMeshes[i].alignment == TextAlignment.Left && _alignment != TextAnchor.LowerLeft && _alignment != TextAnchor.MiddleLeft && _alignment != TextAnchor.UpperLeft)
                            || (textMeshes[i].alignment == TextAlignment.Center && _alignment != TextAnchor.LowerCenter && _alignment != TextAnchor.MiddleCenter && _alignment != TextAnchor.UpperCenter)
                            || (textMeshes[i].alignment == TextAlignment.Right && _alignment != TextAnchor.LowerRight && _alignment != TextAnchor.MiddleRight && _alignment != TextAnchor.UpperRight)))
                        {
                            if(_alignment == TextAnchor.LowerLeft || _alignment == TextAnchor.MiddleLeft || _alignment == TextAnchor.UpperLeft)
                            {
                                textMeshes[i].alignment = TextAlignment.Left;
                            }
                            else if(_alignment == TextAnchor.LowerCenter || _alignment == TextAnchor.MiddleCenter || _alignment == TextAnchor.UpperCenter)
                            {
                                textMeshes[i].alignment = TextAlignment.Center;
                            }
                            else if(_alignment == TextAnchor.LowerRight || _alignment == TextAnchor.MiddleRight || _alignment == TextAnchor.UpperRight)
                            {
                                textMeshes[i].alignment = TextAlignment.Right;
                            }
                            changed = true;
                        }

                        //Other
                        if(_changeColor && textMeshes[i].color != _color)
                        {
                            textMeshes[i].color = _color;
                            changed = true;
                        }

                        if(_changeMaterial && textMeshes[i].GetComponent<MeshRenderer>().sharedMaterial != _material)
                        {
                            textMeshes[i].GetComponent<MeshRenderer>().sharedMaterial = _material;
                            changed = true;
                        }

                        if(changed)
                        {
                            textMeshesChanged++;
                        }
                    }
                }

                if(uguiTextsChanged > 0 || textMeshesChanged > 0)
                {
                    EditorSceneManager.MarkAllScenesDirty();
                }

                _displayDialog = uguiTextsChanged + " UGUI Text(s) changed";
                if(_applySettingsOnTextMeshes)
                {
                    _displayDialog += " + " + textMeshesChanged + " TextMeshe(s) changed";
                }
            }

            EditorGUILayout.LabelField(_displayDialog, EditorStyles.miniLabel, GUILayout.ExpandWidth(true));

            EditorGUILayout.Space();
        }
        #endregion EditorWindow

        #region Methods
        private static void GetAllFontsInProject_r(string searchPath)
        {
            string[] fontsPaths = Directory.GetFiles(searchPath, "*.ttf");
            for(int i = 0; i < fontsPaths.Length; i++)
            {
                fontsPaths[i] = fontsPaths[i].Substring(fontsPaths[i].IndexOf('\\'));
                fontsPaths[i] = fontsPaths[i].Replace('\\', '/');
                Font font = AssetDatabase.LoadAssetAtPath("Assets" + fontsPaths[i], typeof(Font)) as Font;
                if(font)
                {
                    _fonts.Add(font);
                }
            }

            fontsPaths = Directory.GetFiles(Application.dataPath, "*.otf");
            for(int i = 0; i < fontsPaths.Length; i++)
            {
                fontsPaths[i] = fontsPaths[i].Substring(fontsPaths[i].IndexOf('\\'));
                fontsPaths[i] = fontsPaths[i].Replace('\\', '/');
                Font font = AssetDatabase.LoadAssetAtPath("Assets" + fontsPaths[i], typeof(Font)) as Font;
                if(font)
                {
                    _fonts.Add(font);
                }
            }

            string[] directoriesPath = Directory.GetDirectories(searchPath);
            for(int i = 0; i < directoriesPath.Length; i++)
            {
                GetAllFontsInProject_r(directoriesPath[i]);
            }
        }
        #endregion Methods
    }
}