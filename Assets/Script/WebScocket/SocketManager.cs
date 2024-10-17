using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;


namespace WepSocketUnity
{
    public class SocketManager : MonoBehaviour
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private bool isConnected = false;

        public event Action<string> OnMessageReceived; // Event for receiving messages
        public event Action OnConnected; // Event for connection established
        public event Action OnDisconnected; // Event for disconnection
        public event Action<string> OnError; // Event for errors

        public void Connect(string url, int port)
        {
            client = new TcpClient();
            try
            {
                client.Connect(url, port);
                stream = client.GetStream();
                isConnected = true;

                // Trigger the connected event
                OnConnected?.Invoke();

                // Start the thread to listen for incoming messages
                receiveThread = new Thread(ReceiveMessages);
                receiveThread.Start();
                Debug.Log("Connected to WebSocket server");
            }
            catch (Exception e)
            {
                Debug.LogError("Connection error: " + e.Message);
                OnError?.Invoke("Connection error: " + e.Message); // Trigger error event
            }
        }

        private void ReceiveMessages()
        {
            while (isConnected)
            {
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead > 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        OnMessageReceived?.Invoke(message); // Trigger event for received message
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("Receiving error: " + e.Message);
                    OnError?.Invoke("Receiving error: " + e.Message); // Trigger error event
                    Disconnect(); // Disconnect on error
                }
            }
        }

        public void Send(string message)
        {
            if (isConnected)
            {
                try
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(message);
                    stream.Write(buffer, 0, buffer.Length);
                    Debug.Log("Sent: " + message);
                }
                catch (Exception e)
                {
                    Debug.LogError("Sending error: " + e.Message);
                    OnError?.Invoke("Sending error: " + e.Message); // Trigger error event
                }
            }
            else
            {
                Debug.LogError("Cannot send message. Not connected.");
                OnError?.Invoke("Cannot send message. Not connected."); // Trigger error event
            }
        }

        public void Disconnect()
        {
            if (isConnected)
            {
                isConnected = false;
                stream.Close();
                client.Close();
                receiveThread?.Abort();

                // Trigger the disconnected event
                OnDisconnected?.Invoke();
                Debug.Log("Disconnected from WebSocket server");
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }
    }
}

