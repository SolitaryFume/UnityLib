using System;
using System.Collections.Generic;
using Proto;

namespace UnityLib.Net
{
    public class NetManagerHandle
    {
        public class Disposable : IDisposable
        {
            private Action action;
            Disposable(Action action)
            {
                this.action = action;
            }

            public void Dispose()
            {
                action?.Invoke();
            }

            public static implicit operator Action(Disposable disposable)
            {
                return disposable.action;
            }

            public static implicit operator Disposable(Action action)
            { 
                return new Disposable(action);
            }
        }

        private static Dictionary<Type, Action<IMessage>> dic = new Dictionary<Type, Action<IMessage>>();

        public static void MessageHandle(IMessage message)
        {
            Debug.Log($"收到消息:{message.GetType()}");
            var ty = message.GetType();
            if (dic.TryGetValue(ty, out var handle))
            {
                handle(message);
            }
            else
            {
                Debug.LogError($"No Find MessageHandle message type is  : {ty}");
            }
        }

        public static IDisposable Register<TMessage>(Action<TMessage> handle)
            where TMessage : class,IMessage
        {
            var ty = typeof(TMessage);

            Action<IMessage> action = msg => handle(msg as TMessage);

            dic.Add(ty, action);

            Action unRegister = () => UnRegister(ty, action);
            return (Disposable)unRegister;
        }

        public static void UnRegister(Type key, Action<IMessage> value)
        {
            dic.Remove(key);
        }
    }
}