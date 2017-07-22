namespace RHL.EventManager {

    /// <summary>
    ///     Represents the method that will handle an event.
    /// </summary>
    /// <typeparam name="T">The type of the event data.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="eventArgs">An object that contains the event data.</param>
    public delegate void EventHandler<in T>(object sender, T eventArgs) where T : EventArgs;

}