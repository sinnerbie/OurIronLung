using System.Collections.Generic;
using System.Threading;
using System.IO.Ports;
using UnityEngine;
using System;

public class ArduinoSerialReadWrite : MonoBehaviour
{
    [SerializeField] public string port = "";
    private static string _port = "COM4";
    SerialPort serial = new SerialPort(_port, 115200);
    Thread serialThread;

    private string data;
    private string toRead;
    public bool canRead = false;

    public int forwardPress = 0;
    public int backPress = 0;
    public Vector3 angularVelocity = Vector3.zero;

    [ContextMenu("StartReading")]
    public void StartReading()
    { 
        if (port != "")
        {
            _port = port;
            serial = new SerialPort(_port, 115200);
        }
        serial.DtrEnable = true;
        serial.ReadTimeout = 100;
        serial.Open();
        serialThread = new Thread(new ThreadStart(ReadFromPort));
        serialThread.IsBackground = true;
        serialThread.Start();
        canRead = true;
    }

    [ContextMenu("StopReading")]
    public void StopReading()
    {
        canRead = false;
        serial.Close();
    }

    void ReadFromPort()
    {
        while (true)
        {
            if (serial.IsOpen)
            {
                try
                {
                    data = serial.ReadLine();
                    toRead = "";
                    if (data.Length > 0)
                        toRead = data;
                    SortLine(toRead);
                }
                catch (System.Exception)
                {

                }
            }
        }
    }

    void SortLine(string line)
    {
        char type = line[0];
        float value = float.Parse(line.Substring(1));

        switch (type)
        {
            case 'X':
                if (value != angularVelocity.x)
                    angularVelocity = new Vector3(value, angularVelocity.y, angularVelocity.z);
                break;
            case 'Y':
                if (value != angularVelocity.y)
                    angularVelocity = new Vector3(angularVelocity.x, value, angularVelocity.z);
                break;
            case 'Z':
                if (value != angularVelocity.z) 
                    angularVelocity = new Vector3(angularVelocity.x, angularVelocity.y, value);
                break;
            case 'F':
                if (value == 0)
                    forwardPress = 1;
                else
                    forwardPress = 0;
                break;
            case 'B':
                if (value == 0)
                    backPress = 1;
                else
                    backPress = 0;
                break;
            case 'A':
                Debug.Log("IT worked!!!");
                break;
            default:
                Debug.LogError(type + " is not a recognized variable type");
                break;
        }
    }

    public void WriteToControler(string toWrite)
    {
        serial.Write(toWrite);
        Debug.Log("Written to ard");
    }
}
