using System;
using System.Runtime.Serialization;

namespace Torpedo.GameElement
{
    [Serializable]
    internal class GameOverExeption : Exception
    {
        private string v;
        private string name;

        public GameOverExeption()
        {
        }

        public GameOverExeption(string? message) : base(message)
        {
        }

        public GameOverExeption(string v, string name)
        {
            this.v = v;
            this.name = name;
        }

        public GameOverExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GameOverExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}