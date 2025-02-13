using UnityEngine;
using TMPro;

namespace ShowFPS
{
    public class ShowFPS : MonoBehaviour
    {
        public TextMeshProUGUI FpsText;

        private float pollingTime = 1f;
        private float time;
        private int frameCount;
        void Update()
        {
            time += Time.deltaTime;

            frameCount++;

            if (time >= pollingTime)
            {
                int frameRate = Mathf.RoundToInt(frameCount / time);
                FpsText.text = "Fps : " +frameRate.ToString();

                time -= pollingTime;
                frameCount = 0;
            }
        }
    }

}
