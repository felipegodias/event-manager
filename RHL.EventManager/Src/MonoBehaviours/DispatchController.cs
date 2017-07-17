using System;
using System.Collections;
using System.Collections.Generic;
using RHL.EventManager.Collections;
using UnityEngine;

namespace RHL.EventManager.MonoBehaviours {

    internal sealed class DispatchController : MonoBehaviour {

        private readonly WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        private readonly IDictionary<Type, IEventList> listenersByType = new Dictionary<Type, IEventList>();

        private readonly IDictionary<uint, IEventList> listenersById = new Dictionary<uint, IEventList>();

        private uint serialId = 1;

        internal void Dispatch<T>(object sender, T eventArgs) where T : EventArgs {
            Type type = typeof(T);
            if (!this.listenersByType.ContainsKey(type)) {
                return;
            }
            EventList<T> eventList = this.listenersByType[type] as EventList<T>;
            if (eventList == null) {
                return;
            }
            Action[] invocationList = eventList.GetInvocationList(sender, eventArgs);
            foreach (Action action in invocationList) {
                this.StartCoroutine(this.ExecuteDispatch(action));
            }
        }

        internal uint AddListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            Type type = typeof(T);
            if (!this.listenersByType.ContainsKey(type)) {
                this.listenersByType.Add(type, new EventList<T>());
            }

            EventList<T> eventList = this.listenersByType[type] as EventList<T>;
            if (eventList == null) {
                return 0;
            }

            eventList.Add(this.serialId, eventHandler);
            this.listenersById.Add(this.serialId, eventList);
            this.serialId++;
            return this.serialId - 1;
        }

        internal bool RemoveListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            Type type = typeof(T);
            if (!this.listenersByType.ContainsKey(type)) {
                return false;
            }

            EventList<T> eventList = this.listenersByType[type] as EventList<T>;
            if (eventList == null) {
                return false;
            }

            uint removedId = eventList.Remove(eventHandler);

            if (removedId == 0) {
                return false;
            }

            this.listenersById.Remove(removedId);
            return true;
        }

        internal bool RemoveListener(uint id) {
            if (!this.listenersById.ContainsKey(id)) {
                return false;
            }

            IEventList eventList = this.listenersById[id];
            eventList.Remove(id);
            this.listenersById.Remove(id);
            return true;
        }

        internal bool ContainsListener<T>(EventHandler<T> eventHandler) where T : EventArgs {
            Type type = typeof(T);
            if (!this.listenersByType.ContainsKey(type)) {
                return false;
            }

            EventList<T> eventList = this.listenersByType[type] as EventList<T>;
            return eventList != null && eventList.Contains(eventHandler);
        }

        internal bool ContainsListener(uint id) {
            return this.listenersById.ContainsKey(id);
        }

        private IEnumerator ExecuteDispatch(Action action) {
            yield return this.waitForEndOfFrame;
            action?.Invoke();
        }

    }

}