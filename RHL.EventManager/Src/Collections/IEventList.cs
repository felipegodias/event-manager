namespace RHL.EventManager.Collections {

    internal interface IEventList {

        int Count { get; }

        uint Remove(uint id);

        bool Contains(uint id);

        void Clear();

    }

}