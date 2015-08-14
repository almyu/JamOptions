using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace JamOptions.Audio {

    [RequireComponent(typeof(RectTransform))]
    public class AudioSliders : MonoBehaviour {

        public RectTransform template;

        [System.Serializable]
        public struct MixerSetup {
            public AudioMixer mixer;

            [System.Serializable]
            public struct Parameter {
                public string label, name;
                public Vector2 range;
            }

            public Parameter[] parameters;
        }

        public MixerSetup[] mixers;


        private void OnValidate() {
            if (mixers == null) return; // Wait for initial deserialization
            if (mixers.Length != 0) return;

            mixers = System.Array.ConvertAll(Resources.FindObjectsOfTypeAll<AudioMixer>(), mixer => {
                return new MixerSetup {
                    mixer = mixer,
                    parameters = new MixerSetup.Parameter[] {
                        new MixerSetup.Parameter { label = "Volume", name = "Volume", range = new Vector2(-80, 20) }
                    }
                };
            });
        }

        private void OnEnable() {
            var itemIndex = 0;

            foreach (var setup in mixers) {
                foreach (var param in setup.parameters) {
                    ++itemIndex;

                    var xf = Instantiate(template);
                    xf.SetParent(transform, false);
                    xf.pivot = new Vector2(xf.pivot.x, itemIndex);
                    xf.gameObject.SetActive(true);

                    var label = xf.GetComponentInChildren<Text>();
                    if (label)
                        label.text = param.label;

                    var slider = xf.GetComponentInChildren<Slider>();
                    if (slider) {
                        var mixer = setup.mixer;
                        var paramName = param.name;

                        slider.onValueChanged.AddListener(level => {
                            mixer.SetFloat(paramName, level);
                        });

                        slider.minValue = param.range[0];
                        slider.maxValue = param.range[1];
                    }
                }
            }

            var selfXf = GetComponent<RectTransform>();
            var padding = Mathf.Abs(template.anchoredPosition.y * 2);
            selfXf.sizeDelta = new Vector2(selfXf.sizeDelta.x, itemIndex * template.sizeDelta.y + padding);
        }

        private void OnDisable() {
            foreach (RectTransform child in transform)
                if (child != template)
                    Destroy(child.gameObject);
        }
    }
}
