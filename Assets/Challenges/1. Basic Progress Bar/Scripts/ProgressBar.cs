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

        float globalValue;
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


            if (value > 1)
                currentValue = value % Mathf.FloorToInt(value);
            else
            {
                currentValue = value;
            }
            fillBar.localScale = new Vector3(currentValue, fillBar.localScale.y, fillBar.localScale.z);
            UpdateTextPosition();

        }

        /// <summary>
        /// The progress bar will move to the given inputValue
        /// </summary>
        /// <param name="value">Must be in range [0,1]</param>
        /// <param name="speedOverride">Will override the base speed if one is given</param>
        public void SetTargetValue(float value, float? speedOverride = null)
        {
            currentValue = fillBar.localScale.x;




            Debug.Log(currentValue);
            Debug.Log(value);


            increase = value > currentValue;
            int negate;


            if (increase)
                negate = 1; // basically we just change the aritmehic signs based on if value is increasing or decreasing
            else
                negate = -1;

            globalValue = value;               //  keeping value as global so that we can manipulate its value, but I actually dont like this,
                                               //  there might be a better solution, this is kinda messy

            StartCoroutine(UpdateProgress(negate));
        }




        void UpdateTextPosition()
        {
            switch (textPosition)
            {
                case TextPosition.BarCenter:
                    percentageText.transform.localPosition = Vector3.zero;
                    percentageText.text = Mathf.RoundToInt(currentValue * 100).ToString() + "%";

                    // Debug.Log(Mathf.RoundToInt(currentValue * 100) + "Text Value");
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


                    if (value > 1)
                    {
                        if (negate < 0)// if decreasing
                            currentValue = value % Mathf.FloorToInt(value);
                        else
                        {
                            currentValue += baseSpeed * negate * Time.deltaTime;
                            if (value > 1 && currentValue >= 1f)
                            {
                                currentValue -= 1;
                                globalValue -= 1;


                            }
                        }
                    }
                    else
                    {
                        if (negate < 0)// if decreasing
                            currentValue = value;
                        else
                        {
                            currentValue += baseSpeed * negate * Time.deltaTime;
                        }
                    }

                    break;
                case ProgressSnapOptions.SnapToHigherValue:


                    if (value > 1)
                    {
                        if (negate > 0)// if increasing
                            currentValue = value % Mathf.FloorToInt(value);
                        else
                        {
                            currentValue += baseSpeed * negate * Time.deltaTime;
                            if (value > 1 && currentValue >= 1f)
                            {
                                currentValue -= 1;
                                globalValue -= 1;


                            }
                        }

                    }

                    else
                    {
                        if (negate > 0)// if increasing
                            currentValue = value;

                        else
                            currentValue += baseSpeed * negate * Time.deltaTime;
                    }
                    break;

                case ProgressSnapOptions.DontSnap:

                    currentValue += baseSpeed * negate * Time.deltaTime;

                    if (value > 1 && currentValue >= 1f)
                    {
                        currentValue -= 1;
                        globalValue -= 1;


                    }

                    //  Debug.Log(currentValue);
                    break;
                default:
                    break;
            }
        }


        IEnumerator UpdateProgress(int negate)
        {


            while (globalValue * negate - currentValue * negate > 0)
            {

                UpdateBarValue(globalValue, negate);

                UpdateTextPosition();

                fillBar.localScale = new Vector3(currentValue, fillBar.localScale.y, fillBar.localScale.z);
                yield return null;

            }

        }

    }







}
