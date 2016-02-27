using UnityEngine;
using UnityEngine.UI;

namespace HouraiTeahouse.Localization {

    /// <summary>
    /// An abstract MonoBehaviour class that localizes the strings displayed on UI Text objects.
    /// </summary>
    public abstract class AbstractLocalizedText : MonoBehaviour {

        [SerializeField]
        private Text _text;

        private LanguageManager _languageManager;
        private string _localizationKey;

        /// <summary>
        /// The UI Text object to display the localized string onto
        /// </summary>
        public Text Text {
            get { return _text; }
            set { _text = value; }
        }

        /// <summary>
        /// The localization key used to lookup the localized string.
        /// </summary>
        protected string LocalizationKey {
            get { return _localizationKey; }
            set {
                if (_localizationKey == value || value == null)
                    return;
                _localizationKey = value;
                if (_languageManager.HasKey(_localizationKey))
                    _text.text = Process(_languageManager[_localizationKey]);
                else
                    Debug.LogWarning(string.Format("Tried to localize key {0}, but LanguageManager has no such key", _localizationKey));
            }
        }

        /// <summary>
        /// Unity Callback. Called once upon object instantiation.
        /// </summary>
        protected virtual void Awake() {
            _languageManager = LanguageManager.Instance;
            if (!_text)
                _text = GetComponent<Text>();
            enabled = _languageManager && _text;
        }

        /// <summary>
        /// Unity Callback. Called on the first frame before Update is called.
        /// </summary>
        protected virtual void Start() {
            _languageManager.OnChangeLanguage += OnChangeLanguage;
            if (_localizationKey == null)
                return;
            if (_languageManager.HasKey(_localizationKey))
                _text.text = Process(_languageManager[_localizationKey]);
            else
                Debug.LogWarning(string.Format("Tried to localize key {0}, but LanguageManager has no such key", _localizationKey));
        }

        /// <summary>
        /// Event callback for when the system wide language is changed.
        /// </summary>
        /// <param name="language">the language set that was changed to.</param>
        void OnChangeLanguage(Language language) {
            if (!language)
                return;
            if (_localizationKey == null)
                return;
            if (language.ContainsKey(_localizationKey))
                _text.text = Process(language[_localizationKey]);
            else
                Debug.LogWarning(string.Format("Tried to localize key {0}, but langauge {1} has no such key", _localizationKey, language));
        }

        /// <summary>
        /// Post-Processing on the retrieved localized string.
        /// </summary>
        /// <param name="val">the pre-processed localized string</param>
        /// <returns>the post-processed localized string</returns>
        protected virtual string Process(string val) {
            return val;
        }

    }

    /// <summary>
    /// An AbstractLocalizedText where the localization key is defined via serializaiton 
    /// </summary>
    [HelpURL("http://wiki.houraiteahouse.net/index.php/Dev:Localization#Localized_Text")]
    public sealed class LocalizedText : AbstractLocalizedText {

        /// <summary>
        /// The serialized localization key
        /// </summary>
        [SerializeField]
        private string _key;

        /// <summary>
        /// The format for the localization string to be displayed in.
        /// </summary>
        /// <see cref="string.Format"/>
        [SerializeField]
        private string _format;

        /// <summary>
        /// Unity callback. Called once before the object's first frame.
        /// </summary>
        protected override void Start() {
            base.Start();
            LocalizationKey = _key;
        }

        /// <summary>
        /// Gets or sets the localization key of the LocalizedText
        /// </summary>
        public string Key {
            get { return LocalizationKey; }
            set { LocalizationKey = value; }
        }

        /// <summary>
        /// <see cref="AbstractLocalizedText"/>
        /// </summary>
        protected override string Process(string val) {
            if (string.IsNullOrEmpty(_format))
                return val;
            return string.Format(_format, val);
        }
    }

}
