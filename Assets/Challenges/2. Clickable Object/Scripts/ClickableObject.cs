using System;
using UnityEngine;
using Unity.Collections;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

using TMPro;

namespace Challenges._2._Clickable_Object.Scripts
{
    public class InvalidInteractionMethodException : Exception
    {
        private const string MessageWithMethodArgument =
            "Attempted to register to an invalid method of clickable interaction. The ClickableObject '{0}' does not allow interaction of type {1}";
        public InvalidInteractionMethodException(string gameObjectName, ClickableObject.InteractionMethod interactionMethod) : base(string.Format(MessageWithMethodArgument, gameObjectName, interactionMethod))
        {
        }
    }
    [RequireComponent(typeof(Collider))]
    public class ClickableObject : MonoBehaviour, IClickableObject, IPointerDownHandler, IPointerUpHandler
    {


        TextMeshProUGUI clickedText;
        // Do not remove the provided 3 options, you can add more if you like
        [Flags]
        public enum InteractionMethod
        {
            Tap = 2,
            DoubleTap = 4,
            TapAndHold = 8
        }

        /// <summary>
        /// Dont edit
        /// </summary>
        [SerializeField]
        private InteractionMethod allowedInteractionMethods;

        public float doubleClickThreshold = 0.4f;
        public float tapAndHoldThreshold = 0.5f;
        public UnityEvent OnDoubleClickDetected;
        private float lastClickTime = -1;
        private float holdTime = -1;
        int clicks;
        bool loop;
        bool hold;





        void Start()
        {
            clickedText = GameObject.FindWithTag("Clicked").GetComponent<TextMeshProUGUI>();
            addPhysicsRaycaster();
        }

        void addPhysicsRaycaster()
        {
            PhysicsRaycaster physicsRaycaster = GameObject.FindObjectOfType<PhysicsRaycaster>();
            if (physicsRaycaster == null)
            {
                Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
            }
        }

        /// <summary>
        /// Checks if the given interaction method is valid for this clickable object.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public bool IsInteractionMethodValid(InteractionMethod method)
        {
            return allowedInteractionMethods.HasFlag(method);
        }


        /// <summary>
        /// Updates the interaction method of the clickable object. Can contain more than one value due to bitflags
        /// </summary>
        public void SetInteractionMethod(InteractionMethod method)
        {
            // Debug.Log(allowedInteractionMethods);
        }


        /// <summary>
        /// Will invoke the given callback when the clickable object is interacted with alongside the method of interaction
        /// </summary>
        /// <param name="callback">Function to invoke</param>
        public void RegisterToClickable(OnClickableClicked callback)
        {
            callback(this, allowedInteractionMethods);
        }

        /// <summary>
        /// Will unregister a previously provided callback
        /// </summary>
        /// <param name="callback">Function previously given</param>
        public void UnregisterFromClickable(OnClickableClicked callback)
        {
            callback(this, allowedInteractionMethods);

        }

        public void UnregisterFromClickable(OnClickableClickedUnspecified callback)
        {
            callback();
        }

        /// <summary>
        /// Will invoke the given callback when the clickable object is tapped. 
        /// </summary>
        /// <param name="onTapCallback"></param>
        /// <exception cref="InvalidInteractionMethodException">If tapping is not allowed for this clickable</exception>
        public void RegisterToClickableTap(OnClickableClickedUnspecified onTapCallback)
        {
            onTapCallback();
        }

        /// <summary>
        /// Will invoke the given callback when the clickable object is tapped. 
        /// </summary>
        /// <param name="onTapCallback"></param>
        /// <exception cref="InvalidInteractionMethodException">If double tapping is not allowed for this clickable</exception>
        public void RegisterToClickableDoubleTap(OnClickableClickedUnspecified onTapCallback)
        {
            onTapCallback();
        }

        public void OnPointerDown(PointerEventData eventData)
        {

            holdTime = Time.time;
            loop = true;
            hold = true;
            StartCoroutine(TapAndHoldTimer());
            clicks++;

        }



        IEnumerator TapAndHoldTimer()
        {
            lastClickTime = Time.time;
            while (loop)
            {
                if (hold)
                {
                    if (Time.time - holdTime > tapAndHoldThreshold)
                    {
                        RegisterToClickable(GetClickableObjTapAndHold);

                        Debug.Log("Hold");
                        clicks = 0;
                        loop = false;
                        hold = false;
                        break;

                    }
                }


                else if (Time.time - lastClickTime > doubleClickThreshold && clicks > 1)
                {
                    RegisterToClickableDoubleTap(GetClickableObjDoubleTap);


                    clicks = 0;
                    loop = false;
                    Debug.Log("D tap");
                    break;
                }



                else if (Time.time - lastClickTime > doubleClickThreshold + 0.1f)
                {
                    RegisterToClickableTap(GetClickableObjTap);


                    clicks = 0;
                    Debug.Log("Tap");
                    loop = false;

                    break;
                }



                yield return null;
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UnregisterFromClickable(Clickable);
            hold = false;
        }


        void Clickable(ClickableObject obj, InteractionMethod m)
        {


        }
        void GetClickableObjDoubleTap()
        {
            if ((allowedInteractionMethods & (InteractionMethod.DoubleTap)) > 0)
                clickedText.text = this.name + " : " + InteractionMethod.DoubleTap;
        }

        void GetClickableObjTap()
        {

            if ((allowedInteractionMethods & (InteractionMethod.Tap)) > 0)
                clickedText.text = this.name + " : " + InteractionMethod.Tap;
        }

        void GetClickableObjTapAndHold(ClickableObject obj, InteractionMethod m)
        {

            if ((allowedInteractionMethods & (InteractionMethod.TapAndHold)) > 0)
                clickedText.text = this.name + " : " + InteractionMethod.TapAndHold;
        }


    }
}
