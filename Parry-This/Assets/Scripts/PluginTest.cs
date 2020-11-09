using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PluginTest : MonoBehaviour
{
    const string pluginName = "com.imagecampus.ultralogger2.MyPlugin";
    public static PluginTest loggerInstance;

    //delegate void PositiveInputCall();
    private void Awake()
    {
        Application.logMessageReceived += RegisterLog;
        DontDestroyOnLoad(this.gameObject);

        if (loggerInstance == null)
            loggerInstance = this;
        else
            Destroy(this.gameObject);
    }

    // private void OnDestroy()
    // {
    //     Application.logMessageReceived -= RegisterLog;
    // }

    class AlertViewCallback : AndroidJavaProxy
    {
        private System.Action<int> alertHandler;
        public delegate void PositiveInputCall();
        public PositiveInputCall positiveInputCall;

        public AlertViewCallback(System.Action<int> alertHandlerIn) : base(pluginName + "$AlertViewCallback")
        {
            alertHandler = alertHandlerIn;
        }
        public void onButtonTapped(int index)
        {
            if(index == -3)
            {
                positiveInputCall.Invoke();
            }
            if (alertHandler != null)
                alertHandler(index);
        }
    }

    static AndroidJavaClass _pluginClass;
    static AndroidJavaObject _pluginInstance;

    public static AndroidJavaClass PluginClass
    {
        get
        {
            if (_pluginClass == null)
            {
                _pluginClass = new AndroidJavaClass(pluginName);
                AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                _pluginClass.SetStatic<AndroidJavaObject>("mainActivity", activity);
            }
            return _pluginClass;
        }
    }

    public static AndroidJavaObject PluginInstance
    {
        get
        {
            if (_pluginInstance == null)
            {
                _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            return _pluginInstance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Elapsed Time: " + GetElapsedTime());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Noop()
    {
        ShowAlertDialog(new string[] {"Inminent Log Deletion",
         "Do you wish to clear the log file?",
          "Yes", "No"}, (int obj) =>
          {
              
          });
    }


    public void GenerateLog()
    {
        Debug.Log("This is a randomly generated Log. id: " + Random.Range(0, 255));
    }
    public string ShowLogs()
    {
        if (Application.platform == RuntimePlatform.Android)
            return PluginInstance.Call<string>("GetLogs");
        else
        {
            Debug.Log("Wrong Platform");
            return "error";
        }
    }

    public void DeleteLogs()
    {
        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call<bool>("DeleteLogs");
        else
        {
            Debug.Log("Wrong Platform");
        }
    }

    private void RegisterLog(string condition, string stackTrace, LogType type)
    {
        System.Action<int> handler = null;
        if (Application.platform == RuntimePlatform.Android)
            PluginInstance.Call("RegisterLog", new object[] { type.ToString(), condition });
        else
            Debug.Log("not on android");
    }

    public void CheckElapsedTime()
    {
        Debug.Log("Elapsed Time: " + GetElapsedTime());
    }

    private double GetElapsedTime()
    {
        if (Application.platform == RuntimePlatform.Android)
            return PluginInstance.Call<double>("GetElapsedTime");
        else
        {
            Debug.Log("Wrong Platform");
            return 0;
        }
    }

    private void ShowAlertDialog(string[] strings, System.Action<int> handler = null)
    {
        if (strings.Length < 3)
        {
            Debug.LogError("alert dialog requires >=3 strings");
            return;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            AlertViewCallback alertHandler = new AlertViewCallback(handler);
            alertHandler.positiveInputCall += DeleteLogs;
            PluginInstance.Call("ShowAlertView", new object[] { strings, alertHandler });
        }
        else
            Debug.Log("alert not supported on this platform");
    }
}
