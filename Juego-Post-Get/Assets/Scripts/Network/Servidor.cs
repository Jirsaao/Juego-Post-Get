using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
public class Servidor : MonoBehaviour
{
    public NetworkDriver net_driver;
    public string ipAddress = "127.0.0.1";
    public ushort port = 9000;


    void Start()
    {
        net_driver = NetworkDriver.Create();
        var direccion_escolta = NetworkEndpoint.Parse(ipAddress, port);
        if( net_driver.Bind(direccion_escolta) != 0)
        {
            Debug.Log("SERVIDOR:: No se pudo vincular al puerto " + direccion_escolta.Port+
                " y la ip " + direccion_escolta.Address);
        }
        else
        {
            net_driver.Listen();
            Debug.Log("SERVER:: Escuchando el puerto " + direccion_escolta.Port +
                " y la ip " + direccion_escolta.Address);
        }
    }

    // Update is called once per frame
    void Update()
    {
        

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
