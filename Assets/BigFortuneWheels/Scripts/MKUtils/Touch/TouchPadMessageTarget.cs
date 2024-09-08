using System;
using UnityEngine;
using UnityEngine.EventSystems;

/*
    23.08.2020 - first
 */
namespace Mkey
{
    public class TouchPadMessageTarget : MonoBehaviour, IEventSystemHandler //, ICustomMessageTarget
    {
        public Action<TouchPadEventArgs> PointerDownEvent;
        public Action<TouchPadEventArgs> DragBeginEvent;
        public Action<TouchPadEventArgs> DragEnterEvent;
        public Action<TouchPadEventArgs> DragExitEvent;
        public Action<TouchPadEventArgs> DragDropEvent;
        public Action<TouchPadEventArgs> PointerUpEvent;
        public Action<TouchPadEventArgs> DragEvent;

        GameObject dataIcon;

        public void PointerDown(TouchPadEventArgs tpea)
        {
            PointerDownEvent?.Invoke(tpea);
        }

        public void DragBegin(TouchPadEventArgs tpea)
        {
            DragBeginEvent?.Invoke(tpea);
        }

        public void DragEnter(TouchPadEventArgs tpea)
        {
            DragEnterEvent?.Invoke(tpea);
        }

        public void DragExit(TouchPadEventArgs tpea)
        {
            DragExitEvent?.Invoke(tpea);
        }

        public void DragDrop(TouchPadEventArgs tpea)
        {
            DragDropEvent?.Invoke(tpea);
        }

        public void PointerUp(TouchPadEventArgs tpea)
        {
            PointerUpEvent?.Invoke(tpea);
        }

        public void Drag(TouchPadEventArgs tpea)
        {
            DragEvent?.Invoke(tpea);
        }

        public GameObject GetDataIcon()
        {
            return dataIcon;
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}