namespace RHL.EventManager.Collections {

    internal sealed class EventListNode<T> where T : EventArgs {

        private readonly uint id;

        private readonly EventHandler<T> eventHandler;

        private bool removeFlag;

        public EventListNode(uint id, EventHandler<T> eventHandler) {
            this.id = id;
            this.eventHandler = eventHandler;
        }

        public uint Id {
            get { return this.id; }
        }

        public EventHandler<T> EventHandler {
            get { return this.eventHandler; }
        }

        public bool RemoveFlag {
            get { return this.removeFlag; }
        }

        public void MarkToRemove() {
            this.removeFlag = true;
        }

        public void Invoke(object sender, T eventArgs) {
            this.eventHandler.Invoke(sender, eventArgs);
        }

    }

}