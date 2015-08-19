using UnityEngine;
using UnityEngine.Audio;

namespace JamOptions.Audio {

    public class AudioPrefs : MonoBehaviour {

        [System.Serializable]
        public class Parameter {
            public AudioMixer mixer;
            public string label, name;
            public Vector2 range = new Vector2(-80, 0);

            public float value {
                get {
                    if (PlayerPrefs.HasKey(name))
                        return PlayerPrefs.GetFloat(name);

                    var value = range[0];
                    mixer.GetFloat(name, out value);

                    return value;
                }
                set {
                    PlayerPrefs.SetFloat(name, value);
                    mixer.SetFloat(name, value);
                }
            }
        }

        public bool loadOnStart = true;
        public bool saveOnDestroy = true;

        public Parameter[] keptParameters;


        private void Start() {
            if (loadOnStart) Load();
        }

        private void OnDestroy() {
            if (saveOnDestroy) Save();
        }


        public void Load() {
            foreach (var param in keptParameters)
                if (PlayerPrefs.HasKey(param.name))
                    param.mixer.SetFloat(param.name, PlayerPrefs.GetFloat(param.name));
        }

        public void Save() {
            foreach (var param in keptParameters) {
                var value = 0f;
                if (param.mixer.GetFloat(param.name, out value))
                    PlayerPrefs.SetFloat(param.name, value);
            }
        }
    }
}
