using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace JamOptions.Audio {

    [RequireComponent(typeof(RectTransform))]
    public class AudioSliders : MonoBehaviour {

        public RectTransform template;
        public AudioPrefs prefs;


        private void OnEnable() {
            prefs = prefs.GetInstance();

            var itemIndex = 0;

            foreach (var param in prefs.keptParameters) {
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
                    var localParam = param;

                    slider.onValueChanged.AddListener(value => {
                        localParam.value = value;
                    });

                    slider.minValue = param.range[0];
                    slider.maxValue = param.range[1];

                    slider.value = param.value;
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
