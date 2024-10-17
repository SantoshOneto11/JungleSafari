using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WepSocketUnity;

public class WebSocektTest : MonoBehaviour
{
    private SocketManager webSocketClient;

    void Start()
    {
        webSocketClient = gameObject.AddComponent<SocketManager>();

        // Subscribe to events
        webSocketClient.OnMessageReceived += HandleMessage;
        webSocketClient.OnConnected += () => Debug.Log("Successfully connected to server");
        webSocketClient.OnDisconnected += () => Debug.Log("Disconnected from server");
        webSocketClient.OnError += HandleError; // Subscribe to the OnError event

        // Connect to the WebSocket server
        webSocketClient.Connect("localhost", 8080); // Replace with your server details
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Example to send a message
        {
            webSocketClient.Send("Hello, server!");
        }
    }

    private void HandleMessage(string message)
    {
        Debug.Log("Received message: " + message);
    }

    private void HandleError(string errorMessage)
    {
        Debug.LogError("WebSocket Error: " + errorMessage); // Handle the error appropriately
    }

    void OnDestroy()
    {
        webSocketClient.Disconnect(); // Ensure clean disconnect
    }
}
