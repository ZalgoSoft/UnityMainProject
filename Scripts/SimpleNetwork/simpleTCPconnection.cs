using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEditor;
public class simpleTCPconnection : MonoBehaviour
{
    public bool socketReady = false;
    public TcpClient tcpClient;
    public NetworkStream networkStream;
    public StreamWriter streamWriter;
    public StreamReader streamReader;
    public String Host = "192.168.1.231";
    public Int32 Port = 12345;
    public string toSend = "Ping!";
    public string toReceive;
    byte[] sendBytes;
    void closeConnection()
    {
        Debug.Log("Closing socket...");
        socketReady = false;
        try
        {
            networkStream.Close();
            tcpClient.Close();
            Debug.Log("OK socket closed.");
        }
        catch (Exception e)
        {
            Debug.Log("ERR socket error:" + e);
        }
    }
    void openConnection()
    {
        Debug.Log("Opening socket...");
        try
        {
            tcpClient = new TcpClient(Host, Port);
            networkStream = tcpClient.GetStream();
            networkStream.ReadTimeout = 100;
            streamWriter = new StreamWriter(networkStream);
            streamReader = new StreamReader(networkStream);
            socketReady = true;
            Debug.Log("OK socket opened.");
        }
        catch (Exception e)
        {
            Debug.Log("ERR socket error:" + e);
        }
    }
    private void Reset()
    {
        sendBytes = new byte[1024];
        streamWriter.Dispose();
        streamReader.Dispose();
        networkStream.Dispose();
        tcpClient.Dispose();
    }
    public void sendTestString()
    {
        Debug.Log("Sending string...");
        sendBytes = Encoding.UTF8.GetBytes(toSend);
        try
        {
            networkStream.Write(sendBytes, 0, sendBytes.Length);
            networkStream.Flush();
            Debug.Log("OK sent message. Waiting for response");
        }
        catch (Exception e)
        {
            Debug.Log("ERR socket error:" + e);
            socketReady = false;
        }
        try
        {
            networkStream.Read(sendBytes, 0, sendBytes.Length);
            toReceive = Encoding.UTF8.GetString(sendBytes);
            Debug.Log("OK received: " + toReceive);
        }
        catch (Exception e)
        {
            Debug.Log("ERR socket error:" + e);
            socketReady = false;
        }
    }
    [CustomEditor(typeof(simpleTCPconnection))]
    class simpleTCPconnectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var gen = (simpleTCPconnection)target;
            if (DrawDefaultInspector())
            {
            }
            if (!gen.socketReady && GUILayout.Button("Open connection"))
            {
                gen.openConnection();
            }
            if (gen.socketReady && GUILayout.Button("Close connection"))
            {
                gen.closeConnection();
            }
            if (GUILayout.Button("Send test string"))
            {
                gen.sendTestString();
            }
        }
    }
}