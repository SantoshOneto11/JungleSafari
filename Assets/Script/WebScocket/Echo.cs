using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WepSocketUnity
{
    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            Send(e.Data);
        }
    }

    public class Chat : WebSocketBehavior
    {
        private string _suffix;

        public Chat()
        {
            _suffix = String.Empty;
        }

        public string Suffix
        {
            get
            {
                return _suffix;
            }

            set
            {
                _suffix = value ?? String.Empty;
            }
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Sessions.Broadcast(e.Data + _suffix);
        }
    }
}
