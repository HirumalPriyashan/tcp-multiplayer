using System;
using Newtonsoft.Json.Linq;
using TBGServer.Players;

namespace TBGServer.Handlers
{
    abstract class AbstractHandler : IHandler<Tuple<JObject, ClientPlayer>>
    {
        private IHandler<Tuple<JObject, ClientPlayer>> _nextHandler;

        public IHandler<Tuple<JObject, ClientPlayer>> SetNext(IHandler<Tuple<JObject, ClientPlayer>> handler)
        {
            this._nextHandler = handler;
            return handler;
        }

        public virtual void Handle(Tuple<JObject, ClientPlayer> value)
        {
            if (this._nextHandler != null)
            {
                this._nextHandler.Handle(value);
            }
        }
    }
}