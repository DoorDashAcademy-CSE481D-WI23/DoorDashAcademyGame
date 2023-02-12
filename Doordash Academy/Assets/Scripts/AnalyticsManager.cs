using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using System.Collections.Generic;
using Unity.Services.Core.Environments;

public class AnalyticsManager : MonoBehaviour
{

    // this makes it so that there is always only one AnalyticsManager, and its initialized
    // when the first scene is loaded
    public static AnalyticsManager instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        try
        {
            var options = new InitializationOptions();
            #if UNITY_EDITOR
                options.SetEnvironmentName("development");
            #endif
            await UnityServices.InitializeAsync(options);
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
          // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
          Debug.Log(e);
        }
    }

    public static void LogLevelLoad(string levelName) {
        AnalyticsService.Instance.CustomData("loadLevel",  new Dictionary<string, object>
        {
            { "levelName", levelName }
        });
    }

    public static void LogPauseGame() {
        AnalyticsService.Instance.CustomData("pauseGame",  new Dictionary<string, object>
        {
        });
    }

    public static void LogUnpauseGame() {
        AnalyticsService.Instance.CustomData("unpauseGame",  new Dictionary<string, object>
        {
        });
    }

    public static void LogGetDeliveryRoute(string pickupLocation, string dropoffLocation) {
        AnalyticsService.Instance.CustomData("getDeliveryRoute",  new Dictionary<string, object>
        {
            { "pickupLocation", pickupLocation },
            { "dropoffLocation", dropoffLocation }
        });
    }

    public static void LogGetFood() {
        AnalyticsService.Instance.CustomData("getFood",  new Dictionary<string, object>
        {
        });
    }

    public static void LogDeliveryComplete(float moneyEarned) {
        AnalyticsService.Instance.CustomData("deliveryComplete",  new Dictionary<string, object>
        {
            { "moneyEarned", moneyEarned }
        });
    }

        public static void LogLevelEnd() {
        AnalyticsService.Instance.CustomData("levelEnd",  new Dictionary<string, object>
        {
        });
    }
}
