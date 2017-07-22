using System;
using JetBrains.Annotations;

namespace RHL.EventManager.Collections {

    internal interface IEventList {

        int Count { get; }

        uint[] Ids { get; }

        uint Remove(uint id);

        bool Contains(uint id);

        void Clear();

        [CanBeNull]
        Action[] GetInvocationList(object sender, EventArgs eventArgs);

    }

}