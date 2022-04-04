using UnityEngine;
using TMPro;
using System;
using System.Collections;

namespace Challenges._1._Basic_Progress_Bar.Scripts
{
    /// <summary>
    /// Edit this script for the ProgressBar challenge.
    /// </summary>
    public class ProgressBar : MonoBehaviour, IProgressBar
    {



        Transform fillBar;
        TextMeshProUGUI percentageText;


        float barWidth; // width of the progress bar
        float textWidth;  // width of the text area

        float textMoveDelta; // interval of text obj movement
        float currentValue;
        bool increase; // check if increasing or decreasing
        private void Awake()
        {
            barWidth = transform.GetComponent<RectTransform>().rect.width;
            percentageText = transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            textWidth = percentageText.transform.GetComponent<RectTransform>().rect.width;
            textMoveDelta = barWidth - textWidth;

            fillBar = transform.GetChild(1);

        }
        /// <summary>
        /// You can add more options
        /// </summary>
        private enum ProgressSnapOptions
        {
            SnapToLowerValue,
            SnapToHigherValue,
            DontSnap
        }

        /// <summary>
        /// You can add more options
        /// </summary>
        private enum TextPosition
        {
            BarCenter,
            Progress,
            NoText
        }

        /// <summary>
        /// These settings below must function
        /// </summary>
        [Header("Options")]
        [SerializeField]
        private float baseSpeed;
        [SerializeField]
        private ProgressSnapOptions snapOptions;
        [SerializeField]
        private TextPosition textPosition;



        /// <summary>
        /// Sets the progress bar to the given normalized inputValue instantly.
        /// </summary>
        /// <param name="value">Must be in range [0,1]</param>
        public void ForceValue(float value)
        {

            currentValue = value;
            fillBar.localScale = new Vector3(currentValue, fillBar.localScale.y, fillBar.localScale.z);
            percentageText.text = Mathf.RoundToInt(currentValue * 100).ToString() + "%";

        }

        /// <summary>
        /// The progress bar will move to the given inputValue
        /// </summary>
        /// <param name="value">Must be in range [0,1]</param>
        /// <param name="speedOverride">Will override the base speed if one is given</param>
        public void SetTargetValue(float value, float? speedOverride = null)
        {
            currentValue = fillBar.localScale.x;



            increase = value > currentValue;
            int negate;


            if (increase)
                negate = 1; // basically we just change the aritmehic signs based on if value is increasing or decreasing
            else
                negate = -1;


            StartCoroutine(UpdateProgress(value, negate));
        }




        void UpdateTextPosition()
        {
            switch (textPosition)
            {
                case TextPosition.BarCenter:
                    percentageText.transform.localPosition = Vector3.zero;
                    percentageText.text = Mathf.RoundToInt(currentValue * 100).ToString() + "%";
                    break;
                case TextPosition.Progress:
                    percentageText.transform.localPosition = new Vector3(currentValue * textMoveDelta - textMoveDelta / 2,
                        percentageText.transform.localPosition.y,
                        percentageText.transform.localPosition.z);

                    percentageText.text = Mathf.RoundToInt(currentValue * 100).ToString() + "%";
                    break;
                case TextPosition.NoText:
                    percentageText.text = "";
                    break;
                default:
                    break;
            }
        }



        void UpdateBarValue(float value, int negate)
        {
            switch (snapOptions)
            {
                case ProgressSnapOptions.SnapToLowerValue:
                    if (negate < 0)// if decreasing
                        currentValue = value;
                    else
                        currentValue += baseSpeed * negate * Time.deltaTime;
                    break;
                case ProgressSnapOptions.SnapToHigherValue:
                    if (negate > 0)// if increasing
                        currentValue = value;

                    else
                        currentValue += baseSpeed * negate * Time.deltaTime;
                    break;
                case ProgressSnapOptions.DontSnap:
                    currentValue += baseSpeed * negate * Time.deltaTime;
                    break;
                default:
                    break;
            }
        }


        IEnumerator UpdateProgress(float value, int negate)
        {


            while (value * negate - currentValue * negate > 0)
            {
                UpdateBarValue(value, negate);

                UpdateTextPosition();

                fillBar.localScale = new Vector3(currentValue, fillBar.localScale.y, fillBar.localScale.z);
                yield return null;

            }





        }

    }







}
