using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PirateToolChest
{
    public class LanguageManager : MonoBehaviour
    {

        public static LanguageManager Instance;
        public static event Action<ELanguage> OnLanguageChanged;

        [SerializeField] private TextAsset csvFile;
        [SerializeField] private TMP_FontAsset chineseFontRegular;

        public ELanguage currentLanguage;

        private readonly char lineSeperater = '\n'; // It defines line seperate character
        private readonly char fieldSeperator = ';'; // It defines field seperate chracter
        private List<SLocalizedText> textsDatabase = new List<SLocalizedText>(); //contains all the TMP in your scene that has a text equal to an ID at the start of the game
        private List<SCSVLine> CSVLines = new List<SCSVLine>(); //contains all the translated text of your CSV

        List<TMP_Text> allTexts = new List<TMP_Text>();

        /// <summary>
        /// Switch from EN to FR and from FR to EN
        /// </summary>
        public void SwitchLanguage()
        {
            switch (currentLanguage)
            {
                case ELanguage.nothing:
                    ChangeLanguage(ELanguage.nothing);
                    break;
                case ELanguage.en:
                    ChangeLanguage(ELanguage.en);
                    break;
                case ELanguage.fr:
                    ChangeLanguage(ELanguage.fr);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Set current language to a specific language you want
        /// </summary>
        /// <param name="newLanguage"></param>
        public void ChangeLanguage(string newLanguage)
        {
            currentLanguage = (ELanguage)Enum.Parse(typeof(ELanguage), newLanguage);
            SetTexts();
            OnLanguageChanged?.Invoke(currentLanguage);
        }

        /// <summary>
        /// Set current language to a specific language you want
        /// </summary>
        /// <param name="newLanguage"></param>
        public void ChangeLanguage(ELanguage newLanguage)
        {
            currentLanguage = newLanguage;

            SetTexts();
            OnLanguageChanged?.Invoke(currentLanguage);
        }

        /// <summary>
        /// Search in the database the text you want in the current language.
        /// example : you want to get the translation of a specific sentence in the current language
        /// </summary>
        /// <param name="_id">The ID of the text you want</param>
        /// <returns>The text associated with the id in the current language</returns>
        public string GetTextById(string _id)
        {
            try
            {
                return CSVLines.Find(x => x.ID == _id).translations[currentLanguage];
            }
            catch
            {
                Debug.LogWarning($"ID {_id} does not exists in the CSV");
                return _id;
            }
        }

        /// <summary>
        /// Search in the database the SLocalizedText you want in the current language.
        /// Example : you need a SLocalizedText from a TextMeshPro content to retrieve its ID
        /// </summary>
        /// <param name="_content">The text currently in the scene</param>
        /// <returns>The SLocalizedText associated with the text currently in the scene, in the current language</returns>
        public SLocalizedText GetTMPByContent(string _content)
        {
            return textsDatabase.Find(x => x.TMP.text == _content);
        }

        /// <summary>
        /// Search in the CSV the translation of a certain text. Do not really translate of course.
        /// Example : you have a text in english and you want the translation of this text in french.
        /// </summary>
        /// <param name="_text">The source text</param>
        /// <param name="_lang">The language you want the new text in</param>
        /// <returns></returns>
        public string GetTranslation(string _text, ELanguage _lang)
        {
            try
            {
                return CSVLines.Find(x => x.translations.ContainsValue(_text)).translations[_lang];
            }
            catch
            {
                Debug.LogWarning($"There is no line in the CSV with this translation {_text}");
                return _text;
            }
        }

        #region Private Methods

        private void Awake()
        {
            Instance = this;
            allTexts.AddRange(FindObjectsOfTypeIncludingDisabled<TMP_Text>());
            
            foreach (var text in allTexts)
            {
                Debug.Log(text.text);
            }
        }

        private void Start()
        {
            ReadData(); //create CSVLines database

            //create textsDatabase
            foreach (TMP_Text t in allTexts)
            {
                if (CSVLines.Exists(x => x.ID == t.text))
                    textsDatabase.Add(new SLocalizedText(t, t.text, t.font));
            }

            //Translate everything in the default language
            SetTexts();
        }

        /// <summary>
        /// Read data from the CSV File then create a bunch of SLocalizedText for each line of your CSV.
        /// Each SLocalizedText contains a sentence in everylanguage and sometimes a references to a TMP_Text in the scene.
        /// </summary>
        private void ReadData()
        {
            string[] lines = csvFile.text.Split(lineSeperater);
            string[] fieldsOfFirstLine = lines[0].Split(fieldSeperator); //first line is like : id;en;fr

            if (fieldsOfFirstLine[0] != "id")
            {
                Debug.LogError($"There is an error in the csv, the first field should be 'id' and it's {fieldsOfFirstLine[0]}");
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] fields = lines[i].Split(fieldSeperator);
                if (string.IsNullOrEmpty(fields[0]))
                    continue;

                SCSVLine line = new SCSVLine(fields[0], new Dictionary<ELanguage, string>());
                for (int lang = 1; lang < 4; lang++)
                {
                    line.translations.Add((ELanguage)lang, fields[lang]);
                }
                CSVLines.Add(line);
            }
        }

        /// <summary>
        /// Take all localizedTexts and set their font to the provided font
        /// </summary>
        /// <param name="_font">The new font you want</param>
        private void SetFont(TMP_FontAsset _font)
        {
            foreach (SLocalizedText lt in textsDatabase)
            {
                lt.TMP.font = _font;
            }
        }

        /// <summary>
        /// Take all localizedTexts and set their font to their original font
        /// </summary>
        private void ResetFont()
        {
            foreach (SLocalizedText lt in textsDatabase)
            {
                lt.TMP.font = lt.originalFont;
            }
        }

        /// <summary>
        /// Take all localizedTexts and set their TextMeshPro or TextMeshProUGUI to the current language
        /// </summary>
        private void SetTexts()
        {
            ResetFont();
            
            Debug.Log("SetTexts Called");
            
            foreach (SLocalizedText lt in textsDatabase)
            {
                lt.TMP.text = GetTextById(lt.ID);
                Debug.Log(lt.TMP.text);
            }
        }

        //found here https://gist.github.com/SoylentGraham/bef991c9cd38f9b9c39e549bfcfb05a9
        static T[] FindObjectsOfTypeIncludingDisabled<T>()
        {
            var ActiveScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var RootObjects = ActiveScene.GetRootGameObjects();
            var MatchObjects = new List<T>();

            foreach (var ro in RootObjects)
            {
                var Matches = ro.GetComponentsInChildren<T>(true);
                MatchObjects.AddRange(Matches);
            }

            return MatchObjects.ToArray();
        }

        #endregion
    }

    [System.Serializable]
    public struct SLocalizedText
    {
        public TMP_Text TMP;
        public TMP_FontAsset originalFont;
        public string ID;

        public SLocalizedText(TMP_Text _tmp, string _id, TMP_FontAsset _originalFont)
        {
            TMP = _tmp;
            ID = _id;
            originalFont = _originalFont;
        }
    }

    [System.Serializable]
    public struct SCSVLine
    {
        public string ID;
        public Dictionary<ELanguage, string> translations;

        public SCSVLine(string _id, Dictionary<ELanguage, string> _text)
        {
            ID = _id;
            translations = _text;
        }
    }

    //The order is very important !!
    public enum ELanguage
    {
        nothing,
        en,
        fr
    }
}