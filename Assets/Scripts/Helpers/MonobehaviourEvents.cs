using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JuanPayan.CodeSnippets.HelperComponents
{
    public abstract class MonobehaviourEvents : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler, IDragHandler
    {
        [System.Flags]
        public enum MonobehaviourEventsFlags
        {
            Awake = 1 << 0,
            OnEnable = 1 << 1,
            Start = 1 << 2,
            FixedUpdate = 1 << 3,
            OnMouseEnter = 1 << 4,
            OnMouseOver = 1 << 5,
            OnMouseDown = 1 << 6,
            OnMouseDrag = 1 << 7,
            OnMouseClick = 1 << 8,
            OnMouseExit = 1 << 9,
            OnMouseUp = 1 << 10,
            Update = 1 << 11,
            LateUpdate = 1 << 12,
            OnDisable = 1 << 13,
            OnDestroy = 1 << 14
        }

        [SerializeField] private MonobehaviourEventsFlags triggers;


        public abstract void Behaviour();

        protected virtual void Awake()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.Awake)) Behaviour();

        }//Closes Awake method

        protected virtual void OnEnable()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnEnable)) Behaviour();

        }//Closes OnEnable method

        protected virtual void Start()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.Start)) Behaviour();

        }//Closes Start method

        protected virtual void FixedUpdate()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.FixedUpdate)) Behaviour();
        }//Closes FixedUpdate method

        protected virtual void OnMouseEnter()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseEnter)) Behaviour();

        }//Closes OnMouseEnter method

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseEnter)) Behaviour();
        }//Closes OnPointerEnter method

        protected virtual void OnMouseOver()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseOver)) Behaviour();
        }//Closes OnMouseOver method

        public virtual void OnPointerMove(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseOver)) Behaviour();
        }//Closes OnPointerMove method

        protected virtual void OnMouseDown()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseDown)) Behaviour();

        }//Closes OnMouseDown method

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseDown)) Behaviour();
        }//Closes OnPointerDown method

        protected virtual void OnMouseDrag()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseDrag)) Behaviour();

        }//Closes OnDrag method 

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseDrag)) Behaviour();
        }//Closes OnDrag method

        protected virtual void OnMouseUpAsButton()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseClick)) Behaviour();
        }//Closes OnMouseUpAsButton method

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseClick)) Behaviour();
        }//Closes OnPointerClick method

        protected virtual void OnMouseExit()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseExit)) Behaviour();
        }//Closes OnMouseExit method

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseExit)) Behaviour();
        }//Closes OnPointerExit method

        protected virtual void OnMouseUp()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseUp)) Behaviour();
        }//Closes OnMouseUp method

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnMouseUp)) Behaviour();
        }//Closes OnPointerUp method

        protected virtual void Update()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.Update)) Behaviour();
        }//Closes Update method

        protected virtual void LateUpdate()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.LateUpdate)) Behaviour();
        }//Closes LateUpdate method

        protected virtual void OnDisable()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnDisable)) Behaviour();
        }//Closes OnDisable method

        protected virtual void OnDestroy()
        {
            if (triggers.HasFlag(MonobehaviourEventsFlags.OnDestroy)) Behaviour();
        }//Closes OnDestroy method

    }//Closes MonobehaviorEvents class
}//Closes Namespace declaration