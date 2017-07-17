using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RHL.EventManager.Collections {

    internal sealed class EventList<T> : IEventList where T : EventArgs {

        private readonly LinkedList<EventListNode<T>> nodesList;

        private readonly IDictionary<uint, EventListNode<T>> nodesDic;

        private readonly IDictionary<EventHandler<T>, uint> eventHandlersDic;

        private int count;

        public EventList() {
            this.nodesList = new LinkedList<EventListNode<T>>();
            this.nodesDic = new Dictionary<uint, EventListNode<T>>();
            this.eventHandlersDic = new Dictionary<EventHandler<T>, uint>();
        }

        public int Count {
            get { return this.count; }
        }

        public uint Remove(uint id) {
            if (!this.Contains(id)) {
                return 0;
            }
            EventListNode<T> node = this.nodesDic[id];
            if (node.RemoveFlag) {
                return 0;
            }
            node.MarkToRemove();
            this.count--;
            return id;
        }

        public bool Contains(uint id) {
            return this.nodesDic.ContainsKey(id);
        }

        public void Clear() {
            this.nodesList.Clear();
            this.nodesDic.Clear();
            this.eventHandlersDic.Clear();
        }

        public bool Add(uint id, EventHandler<T> eventHandler) {
            if (this.Contains(eventHandler) || this.Contains(id)) {
                throw new InvalidOperationException("The same listener cannot be added twice.");
            }

            EventListNode<T> node = new EventListNode<T>(id, eventHandler);
            this.nodesList.AddLast(node);
            this.nodesDic.Add(id, node);
            this.eventHandlersDic.Add(eventHandler, id);
            this.count++;
            return true;
        }

        public uint Remove(EventHandler<T> eventHandler) {
            if (!this.Contains(eventHandler)) {
                return 0;
            }
            uint id = this.eventHandlersDic[eventHandler];
            return this.Remove(id);
        }

        public bool Contains(EventHandler<T> eventHandler) {
            return this.eventHandlersDic.ContainsKey(eventHandler);
        }

        public Action[] GetInvocationList(object sender, EventArgs eventArgs) {
            if (!(eventArgs is T)) {
                return null;
            }
            Action[] invocationList = new Action[this.count];
            int i = 0;
            LinkedListNode<EventListNode<T>> iterator = this.nodesList.First;
            while (iterator != null) {
                EventListNode<T> node = iterator.Value;
                if (node.RemoveFlag) {
                    this.nodesDic.Remove(node.Id);
                    this.eventHandlersDic.Remove(node.EventHandler);
                    LinkedListNode<EventListNode<T>> next = iterator.Next;
                    this.nodesList.Remove(iterator);
                    iterator = next;
                    continue;
                }
                invocationList[i] = () => { node.Invoke(sender, eventArgs as T); };
                i++;
                iterator = iterator.Next;
            }
            return invocationList;
        }

    }

}