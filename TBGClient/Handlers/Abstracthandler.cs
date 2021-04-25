using Newtonsoft.Json.Linq;

namespace TBGClient.Handlers
{
    abstract class AbstractHandler : IHandler<JObject>
    {
        private IHandler<JObject> _nextHandler;

        public IHandler<JObject> SetNext(IHandler<JObject> handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual void Handle(JObject value)
        {
            if (this._nextHandler != null)
            {
                this._nextHandler.Handle(value);
            }
        }
    }
}