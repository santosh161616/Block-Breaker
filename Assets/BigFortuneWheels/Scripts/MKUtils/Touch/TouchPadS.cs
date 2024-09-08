using UnityEngine;
using System.Collections.Generic;

using System;
using UnityEngine.EventSystems;
// mouse https://answers.unity.com/questions/448771/simulate-touch-with-mouse.html
// https://answers.unity.com/questions/1284788/how-to-convert-touch-input-to-mouse-input-c-unity.html
// https://docs.unity3d.com/ScriptReference/Input-simulateMouseWithTouches.html
// https://romanluks.eu/blog/how-to-simulate-touch-with-mouse-in-unity/
// https://gist.github.com/sdabet/3bda94676a4674e6e4a0
namespace Mkey
{
    public class TouchPadS : MonoBehaviour
    {
        #region events
        public Action<TouchPadEventArgs> ScreenDragEvent;
        public Action<TouchPadEventArgs> ScreenPointerDownEvent;
        public Action<TouchPadEventArgs> ScreenPointerUpEvent;
        #endregion events

        #region properties
        /// <summary>
        /// Return drag direction in screen coord
        /// </summary>
        public Vector2 ScreenDragDirection
        {
            get { return ScreenTouchPos - oldPosition; }
        }

        /// <summary>
        /// Return world position of touch.
        /// </summary>
        public Vector3 WorldTouchPos
        {
            get { return Camera.main.ScreenToWorldPoint(ScreenTouchPos); }
        }

        public Vector2 ScreenTouchPos { get; private set; }

        /// <summary>
        /// Return true if touchpad is touched with mouse or finger
        /// </summary>
        public bool IsTouched { get; private set; }

        /// <summary>
        /// Return true if touch activity enabled
        /// </summary>
        public bool IsActive { get; private set; }
        #endregion properties

        [SerializeField]
        private bool dlog = false;

        [Tooltip("Send touch message to the top collider only")]
        [SerializeField]
        private bool onlyTopCollider = true;

        #region temp vars
        private List<Collider2D> hitList;
        private List<Collider2D> newHitList;
        private TouchPadEventArgs tpea;
        private int pointerID;
        private Vector2 oldPosition;
        private bool dragBeginSended = false;
        #endregion temp vars

        public static TouchPadS Instance;

        #region regular
        void Awake()
        {
            IsActive = true;
            hitList = new List<Collider2D>();
            newHitList = new List<Collider2D>();
            tpea = new TouchPadEventArgs();

            if (Instance) Destroy(gameObject);
            else Instance = this;
        }

        void Update()
        {
            #region pointer down
            bool mouse = Input.GetMouseButtonDown(0);
            if (!IsTouched && (Input.touchCount > 0 || mouse) ) // pointerdown
            {
                IsTouched = true;
                if (IsActive)
                {
                    if (mouse || (Input.touchCount>0 && Input.GetTouch(0).phase == TouchPhase.Began ))
                    {
                        pointerID = (!mouse)? Input.GetTouch(0).fingerId : 10;
#if UNITY_EDITOR
                        if (dlog) Debug.Log("----------------POINTER DOWN (began)--------------( " +  pointerID);
#endif
                        tpea = new TouchPadEventArgs();
                        ScreenTouchPos =(!mouse) ? Input.GetTouch(0).position : (Vector2) Input.mousePosition;
                        oldPosition = ScreenTouchPos;

                        tpea.SetTouch(ScreenTouchPos, Vector2.zero, TouchPhase.Began, onlyTopCollider);
                        hitList = new List<Collider2D>();
                        hitList.AddRange(tpea.hits);

                        if (hitList.Count > 0)
                        {
                            for (int i = 0; i < hitList.Count; i++)
                            {
                                ExecuteEvents.Execute<TouchPadMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.PointerDown(tpea));
                                if (tpea.firstSelected == null) tpea.firstSelected = hitList[i].GetComponent<TouchPadMessageTarget>();
                            }
                        }

                        ScreenPointerDownEvent?.Invoke(tpea);
                        dragBeginSended = false;
                    }
                }
                return;
            } // end pointer down 
            #endregion pointer down

            #region drag
            mouse = Input.GetMouseButton(0);
            if (IsActive && IsTouched)// drag begin,  drag enter, drag, drag exit
            {
                if (mouse || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && pointerID == Input.GetTouch(0).fingerId))
                {
                    ScreenTouchPos = (!mouse) ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
                    tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Moved, onlyTopCollider);
                    oldPosition = ScreenTouchPos;
                    newHitList = new List<Collider2D>(tpea.hits); // garbage

                    //0 ---------------------------------- send drag begin message --------------------------------------------------
                    if (!dragBeginSended)
                    {
#if UNITY_EDITOR
                        if (dlog) Debug.Log("----------------BEGIN DRAG (moved)--------------( " + pointerID);
#endif

                        for (int i = 0; i < hitList.Count; i++)
                        {
                            if (hitList[i]) ExecuteEvents.Execute<TouchPadMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.DragBegin(tpea));
                        }
                        dragBeginSended = true;
                    }

                    //1 ------------------ send drag exit message and drag message --------------------------------------------------

#if UNITY_EDITOR
                    if (dlog) Debug.Log("---------------- ONDRAG --------------( " + pointerID);
#endif

                    foreach (Collider2D cHit in hitList)
                    {
                        if (newHitList.IndexOf(cHit) == -1)
                        {
                            if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.gameObject, null, (x, y) => x.DragExit(tpea));
                        }
                        else
                        {
                            if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.gameObject, null, (x, y) => x.Drag(tpea));
                        }

                    }

                    //2 ------------------ send drag enter message -----------------------------------------------------------------
                    for (int i = 0; i < newHitList.Count; i++)
                    {
                        if (hitList.IndexOf(newHitList[i]) == -1)
                        {
                            if (newHitList[i]) ExecuteEvents.Execute<TouchPadMessageTarget>(newHitList[i].gameObject, null, (x, y) => x.DragEnter(tpea));
                        }
                    }

                    hitList = newHitList;
                    ScreenDragEvent?.Invoke(tpea);
                    return;
                }
            }
            #endregion drag

            #region ended, canceled
            mouse = Input.GetMouseButtonUp(0);
            if (IsTouched)
            {
                if (mouse || (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled)))
                {
                    IsTouched = false;
                    if (IsActive && (mouse || (Input.touchCount > 0 && pointerID == Input.GetTouch(0).fingerId) ))
                    {
#if UNITY_EDITOR
                        if (dlog) Debug.Log("----------------POINTER EXIT, DROP ( ended, canceled )--------------( " + pointerID);
#endif

                        ScreenTouchPos = (!mouse) ? Input.GetTouch(0).position : (Vector2)Input.mousePosition;
                        tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Ended, onlyTopCollider);
                        oldPosition = ScreenTouchPos;

                        foreach (Collider2D cHit in hitList)
                        {
                            if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                        }

                        newHitList = new List<Collider2D>(tpea.hits);
                        foreach (Collider2D cHit in newHitList)
                        {
                            if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                            if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragDrop(tpea));
                        }
                        hitList = new List<Collider2D>();
                        newHitList = new List<Collider2D>();
                    }

                    return;
                }
            }
            #endregion ended, canceled

            if (!IsTouched && pointerID != -1)
            {
                if (hitList != null)
                    foreach (Collider2D cHit in hitList)
                    {
                        if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }

                if (newHitList != null)
                    foreach (Collider2D cHit in newHitList)
                    {
                        if (cHit) ExecuteEvents.Execute<TouchPadMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }
                hitList = new List<Collider2D>();
                newHitList = new List<Collider2D>();
                pointerID = -1;
            }
        }
        #endregion regular

        /// <summary>
        /// Return world position of touch.
        /// </summary>
        public Vector3 GetWorldTouchPos()
        {
            return Camera.main.ScreenToWorldPoint(ScreenTouchPos);
        }

        internal void SetTouchActivity(bool activity)
        {
            IsActive = activity;
#if UNITY_EDITOR
            if (dlog) Debug.Log("touch activity: " + activity);
#endif
        }
    }
}

