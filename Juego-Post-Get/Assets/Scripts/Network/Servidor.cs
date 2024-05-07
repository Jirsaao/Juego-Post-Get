using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

public class Servidor : MonoBehaviour
{
    public NetworkDriver net_driver;
    public string ipAddress = "127.0.0.1";
    public ushort port = 9000;


    void Start()
    {
        net_driver = NetworkDriver.Create();
        var direccion_escolta = NetworkEndpoint.Parse(ipAddress, port);
        if (net_driver.Bind(direccion_escolta) != 0)
        {
            Debug.Log("SERVIDOR:: No se pudo vincular al puerto " + direccion_escolta.Port +
                " y la ip " + direccion_escolta.Address);
        }
        else
        {   
            conexiones = new NativeList<NetworkConnection>(4,Allocator.Persistent);
            net_driver.Listen();
            Debug.Log("SERVER:: Escuchando el puerto " + direccion_escolta.Port +
                " y la ip " + direccion_escolta.Address);
        }
    }

    // Update is called once per frame
    void Update()
    {
        net_driver.ScheduleUpdate().Complete();
        GestionaConexiones();
        LeerMensajesRecibidos();

    }
    private void LeerMensajesRecibidos()
    {
        for (int k = 0; k < conexiones.Length; k++)
        {
            DataStreamReader stream_lectura;
            NetworkEvent.Type net_event_type =  net_driver.PopEventForConnection(conexiones[k], out stream_lectura);
            while (net_event_type != NetworkEvent.Type.Empty)
            {
                switch (net_event_type)
                {
                    case NetworkEvent.Type.Data:
                        FixedString128Bytes text =  stream_lectura.ReadFixedString128();
                        Debug.Log("SERVIDOR: Respuesta" + text);
                        Broadcast(text.ToString());

                        break;
                    case NetworkEvent.Type.Disconnect:
                        conexiones[k] = default(NetworkConnection);

                        break;

                    default:
                        Debug.Log("SERVIDOR: Evento desconectado" + net_event_type);
                        break;

                }
             net_event_type=   net_driver.PopEventForConnection(conexiones[k], out stream_lectura);
            }

        }
    }

    private void Broadcast(string text)
    {
           for(int i=0; i>conexiones.Length; i++)
        {
            if (conexiones[i].IsCreated)
            {
                Debug.Log("SERVER:: Broadcast::"+ text);
                net_driver.BeginSend(NetworkPipeline.Null, conexiones[i], out var stream_escritura);
                stream_escritura.WriteFixedString128("- " + text);
                net_driver.EndSend(stream_escritura);
            }
        }
    }
    private NativeList<NetworkConnection> conexiones;
    private void GestionaConexiones()
    {

        for(int k = 0; k<conexiones.Length; k++)
        {
            if (!conexiones[k].IsCreated)
            {
                conexiones.RemoveAtSwapBack(k);
                k--;
            }
        }


        NetworkConnection nueva_conexion = net_driver.Accept();
        while( nueva_conexion != default(NetworkConnection))
        {
            conexiones.Add(nueva_conexion);
            Debug.Log("SERVIDOR:: Nueva conexion aceptada");

            nueva_conexion = net_driver.Accept();
        }

    }
    private void OnDestroy()
    {
        if(net_driver.IsCreated)
        {
            net_driver.Dispose();
        }
        Debug.Log("SERVIDOR: Stop");
    }
}
