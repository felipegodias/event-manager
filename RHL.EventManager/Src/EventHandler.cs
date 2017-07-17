namespace RHL.EventManager {

    public delegate void EventHandler<in T>(object sender, T eventArgs) where T : EventArgs;

}