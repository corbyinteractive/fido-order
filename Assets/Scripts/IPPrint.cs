using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class IPPrint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Socket clientSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSock.NoDelay = true;
        IPAddress ip = IPAddress.Parse("192.168.1.250");
        IPEndPoint remoteEP = new IPEndPoint(ip, 9100);
        clientSock.Connect(remoteEP);
        Encoding enc = Encoding.ASCII;

        // Line feed hexadecimal values
        byte[] bEsc = new byte[4];
        bEsc[0] = 0x0A;
        bEsc[1] = 0x0A;
        bEsc[2] = 0x0A;
        bEsc[3] = 0x0A;
       

        // Sends an ESC/POS command to the printer to cut the paper
        string output = Convert.ToChar(29) + "V" + Convert.ToChar(65) + Convert.ToChar(0);
        char[] array = output.ToCharArray();
        byte[] byData = enc.GetBytes(array);
        clientSock.Send(byData);
        clientSock.Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
