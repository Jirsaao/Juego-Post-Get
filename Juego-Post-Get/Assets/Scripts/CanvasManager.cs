using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
public class CanvasManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldNombreUsuario;
    [SerializeField] private TMP_InputField inputFieldTextMsg;
    [SerializeField] private TextMeshProUGUI contenidoChat;


    public static CanvasManager Instance;

    public void AddTextChat(string text)
    {
        contenidoChat.text += "\n" + text;
    }
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
    [SerializeField] private GameObject servidor_manager_prefab;
    private GameObject servidor_manager;
    public void clickStartServidor()
    {
        if (!servidor_manager)
        {
            servidor_manager = Instantiate(servidor_manager_prefab);
        }
        else
        {
            Destroy(servidor_manager );
        }
    }
    [SerializeField] private GameObject cliente_manager_prefab;
    private GameObject cliente_manager;
    public void clickStartClient()
    {
        if (!cliente_manager)
        {
            cliente_manager = Instantiate(cliente_manager_prefab);
        }
        else
        {
            Destroy(cliente_manager );
        }
    }

    public void clickButtonSendMsgServer() {
        cliente_manager.GetComponent<Cliente>().SendMessageServer(inputFieldTextMsg.text.ToString());

    }

}
