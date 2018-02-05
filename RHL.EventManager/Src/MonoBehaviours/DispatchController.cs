using System;
using System.Collections;
using System.Collections.Generic;
using RHL.EventManager.Collections;
using UnityEngine;

namespace RHL.EventManager.MonoBehaviours {

    internal sealed class DispatchController : MonoBehaviour {

        private readonly IDictionary<Type, IEventList> listenersByType = new Dictionary<Type, IEventList>();

        private readonly IDictionary<uint, IEventList> listenersById = new Dictionary<uint, IEventList>();

        private uint serialId = 1;

        internal void Dispatch<T>(object sender, T eventArgs, float delay) where T : EventArgs {
            Type objectType = typeof(object);
            Type iterator = eventArgs.GetType();
            do {
                Type type = iterator;
                iterator = iterator.BaseType;
                if (!this.listenersByType.ContainsKey(type)) {
                    continue;
                }
                IEventList eventList = this.listenersByType[type];
                Action[] invocationList = eventList.GetInvocationList(sender, eventArgs);
                if (invocationList == null) {
                    continue;
                }
                foreach (Action action in invocationList) {
                    this.StartCoroutine(this.ExecuteDispatch(action, delay));
                }
            } while (iterator != objectType && iterator != null);
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

        internal void Clear() {
            this.listenersById.Clear();
            this.listenersByType.Clear();
        }

        internal void Clear<T>() where T : EventArgs {
            Type type = typeof(T);
            if (!this.listenersByType.ContainsKey(type)) {
                return;
            }

            IEventList eventList = this.listenersByType[type];
            uint[] ids = eventList.Ids;
            foreach (uint id in ids) {
                this.listenersById.Remove(id);
            }
            eventList.Clear();
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

        private IEnumerator ExecuteDispatch(Action action, float delay) {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

    }

}