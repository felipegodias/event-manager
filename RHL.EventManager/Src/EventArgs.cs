namespace RHL.EventManager {

    /// <summary>
    ///     Represents the base class for classes that contain event data, and provides a value to use for events that do not
    ///     include event data.
    /// </summary>
    public abstract class EventArgs {

        /// <summary>
        /// </summary>
        public void Dispatch() {
            this.Dispatch(null);
        }

        /// <summary>
        /// </summary>
        public void Dispatch(object sender) {
            EventDispacher.Dispatch(sender, this);
        }

    }

}