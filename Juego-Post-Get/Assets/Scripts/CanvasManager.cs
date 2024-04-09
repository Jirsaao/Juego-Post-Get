using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldNombreUsuario;
    public static CanvasManager Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void clickGET()
    {
        string nombreUsuario = inputFieldNombreUsuario.text;
        string url = "http://localhost/CARPETA_PHP/Modulo9/gestionaGET.php?nombre=" + nombreUsuario;
        StartCoroutine(EnviaGet(url));
    }
    IEnumerator EnviaGet(string url)
    {
        UnityWebRequest peticion = UnityWebRequest.Get(url);
        yield return peticion.SendWebRequest();
        if (peticion.result!=UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error:" + peticion.error);
        }
        else
        {
            string respuesta = peticion.downloadHandler.text;
            Debug.Log("Respuesta: " + respuesta);
            InfoPlayer info = JsonUtility.FromJson<InfoPlayer>(respuesta);
            Debug.Log("nombre::" + info.nombre + " email::" + info.email);
        }
    }
    public void clickPOST()
    {
        string nombreUsuario = inputFieldNombreUsuario.text;
        string url = "http://localhost/CARPETA_PHP/Modulo9/gestionaPOST.php";
        WWWForm parametros = new WWWForm();
        //se pueden pasar varios parametros
        parametros.AddField("nombre", nombreUsuario);
       // parametros.AddField("nombre", nombreUsuario);


        StartCoroutine(EnviaPost(url, parametros));
    }
    IEnumerator EnviaPost(string url, WWWForm parametros)
    {
        UnityWebRequest peticion = UnityWebRequest.Post(url, parametros);
        yield return peticion.SendWebRequest();
        if (peticion.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error:" + peticion.error);
        }
        else
        {
            string respuesta = peticion.downloadHandler.text;
            Debug.Log("Respuesta: " + respuesta);
            InfoPlayer info = JsonUtility.FromJson<InfoPlayer>(respuesta);
            Debug.Log("nombre::" + info.nombre + " email::" + info.email);
        }

    }
}
