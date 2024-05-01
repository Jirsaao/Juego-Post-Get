using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;
using JetBrains.Annotations;

public class Cliente : MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public ushort port = 9000;
    public NetworkDriver net_driver;
    public NetworkConnection conexion;
    void Start()
    {
        net_driver = NetworkDriver.Create();
        conexion = default(NetworkConnection);
        var direccion_servidor = NetworkEndpoint.Parse(ipAddress, port);
        conexion = net_driver.Connect(direccion_servidor);
        Debug.Log("CLIENTE:: Conectando a " + direccion_servidor.Port + " y " + direccion_servidor.Address);

    }

    public void OnDestroy()
    {
        conexion.Disconnect(net_driver);
        conexion = default(NetworkConnection);
        net_driver.Dispose();
    }
    // Update is called once per frame
    void Update()
    {
        net_driver.ScheduleUpdate().Complete();
        checkConexion();
        leerMensajesRecibidos();
    }
     private void checkConexion()
    {
        if(!conexion.IsCreated)
        {
            Debug.Log("CLIENTE:: Error de conexion con el servidor");

        }
    }

    public void leerMensajesRecibidos()
    {
        DataStreamReader stream_lectura;
        NetworkEvent.Type net_evt_type = net_driver.PopEventForConnection(conexion, out stream_lectura);
        while (net_evt_type != NetworkEvent.Type.Empty)
        {
            switch (net_evt_type)
            {
                case NetworkEvent.Type.Connect:
                    Debug.Log("CLIENTE::Se ha conectado con el servidor");
                    SendMessageServer("Soy cliente");
                    break;
                case NetworkEvent.Type.Data:

                    FixedString128Bytes txt = stream_lectura.ReadFixedString128();
                    Debug.Log("CLIENTE::Ha recibido mensaje del servidor:" + txt);

                    break;
                case NetworkEvent.Type.Disconnect:
                    Debug.Log("CLIENTE::Servidor desconectado");
                    conexion = default(NetworkConnection);

                    break;
            }

            net_evt_type = net_driver.PopEventForConnection(conexion, out stream_lectura);
    
        }

    }
    public void SendMessageServer(string txt)
    {
        net_driver.BeginSend(conexion, out var streamEscritura);
        streamEscritura.WriteFixedString128("TEXTO CLIENTE::" +txt);
        net_driver.EndSend(streamEscritura);
    }

}
