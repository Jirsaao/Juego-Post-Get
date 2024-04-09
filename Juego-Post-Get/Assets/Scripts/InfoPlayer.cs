using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class InfoPlayer 
{
    public string nombre;
    public string email;
    
    public InfoPlayer(string nombre, string email)
    {
        this.nombre = nombre;
        this.email = email;
    }

}
