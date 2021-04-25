namespace TBGServer.Handlers
{
    public interface IHandler<T>
    {
        IHandler<T> SetNext(IHandler<T> handler);
        
        void Handle(T value);
    }
}