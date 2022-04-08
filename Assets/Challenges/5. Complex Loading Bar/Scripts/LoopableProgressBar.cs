using Challenges._1._Basic_Progress_Bar.Scripts;
using TMPro;
using UnityEngine;

namespace Challenges._5._Complex_Loading_Bar.Scripts
{
    /// <summary>
    /// Uses the basic progress bar to provide an interface of a loading bar with inherent thresholds.
    /// You can imagine this like a player level bar, say your experience thresholds are [0,150,400,1500,8000]
    /// If you jump from 90XP to 1800XP, you would expect the progress bar to loop multiple times until it reaches the desired percentage
    ///
    /// The previous and next threshold texts should be update depending on where the progress currently is,
    /// if the progress bar needs to loop several times, the threshold text should be updated as it passes through each threshold.
    ///
    /// </summary>
    public class LoopableProgressBar : MonoBehaviour, ILoopableProgressBar
    {
        [SerializeField] private ProgressBar basicProgressBar;
        [SerializeField] private int[] initialThresholds;
        [SerializeField] private TMP_Text previousThresholdText;
        [SerializeField] private TMP_Text nextThresholdText;

        private void Start()
        {
            if (basicProgressBar == null) Debug.LogError("Basic progress bar is missing");
            if (previousThresholdText == null) Debug.LogError("Previous Threshold Text is missing");
            if (nextThresholdText == null) Debug.LogError("Next Threshold Text is missing");
            //Fallback
            if (initialThresholds.Length < 2)
            {
                Debug.LogWarning("Initial threshold size was less than 2, replacing it with [0,10]");
                initialThresholds = new int[] { 0, 10 };
            }
            SetThresholds(initialThresholds);

            ForceValue(initialThresholds[0]);

            // basicProgressBar.SetTargetValue(Random.value);
        }

        #region Editable Area

        int index;

        public void SetThresholds(int[] thresholds)
        {
            initialThresholds = thresholds;

            index = 0;
        }

        public void ForceValue(int value)
        {
            Debug.Log(value);
            float r = ProgressBarLoop(value);
            basicProgressBar.ForceValue(r);
        }

        public void SetTargetValue(int value, float? speedOverride = null)
        {
            Debug.Log(value);



            float r = ProgressBarLoop(value);

            basicProgressBar.SetTargetValue(r);





        }


        float ProgressBarLoop(float value)
        {
            float r = value;
            for (int i = index; i < initialThresholds.Length;)
            {

                if (r >= initialThresholds[i])
                {
                    i++;

                    index = i;              // keeping record of how many times set value exceeds the threshold
                    continue;
                }
                else
                {
                    r = (r - initialThresholds[i - 1]) / (initialThresholds[i] - initialThresholds[i - 1]); // we get the left over

                    previousThresholdText.text = initialThresholds[i - 1].ToString();
                    nextThresholdText.text = initialThresholds[i].ToString();

                    // also we should set the currentMinumum and currentMaximum after setting this so that our bar is only able to increase.
                    // But I dont know if thats a requirement since no acces is giving to that script.


                    r += index;  // adding that value to the progress value so that it will keep looping index time and use the lef over as progress value
                    index = 0; // reset the index

                    return r;

                }
            }
            return 0;
        }

        #endregion
    }
}