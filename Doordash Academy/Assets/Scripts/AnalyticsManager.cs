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

    public static void LogGetDeliveryRoute(string pickupLocation, string dropoffLocation, string levelName) {
        AnalyticsService.Instance.CustomData("getDeliveryRoute",  new Dictionary<string, object>
        {
            { "pickupLocation", pickupLocation },
            { "dropoffLocation", dropoffLocation },
            { "levelName", levelName }
        });
    }

    public static void LogGetFood(string pickupLocation,
                                  string dropoffLocation,
                                  float foodTemp,
                                  string levelName,
                                  bool failed) {
        AnalyticsService.Instance.CustomData("getFood",  new Dictionary<string, object>
        {
            { "pickupLocation", pickupLocation },
            { "dropoffLocation", dropoffLocation },
            { "foodTemp", foodTemp },
            { "levelName", levelName },
            { "failed", failed }
        });
    }

    public static void LogDeliveryComplete(string pickupLocation,
                                           string dropoffLocation,
                                           string levelName,
                                           float moneyEarned,
                                           float foodTemp,
                                           bool failed) {
        AnalyticsService.Instance.CustomData("deliveryComplete",  new Dictionary<string, object>
        {
            { "pickupLocation", pickupLocation },
            { "dropoffLocation", dropoffLocation },
            { "foodTemp", foodTemp },
            { "moneyEarned", moneyEarned },
            { "levelName", levelName },
            { "failed", failed }
        });
    }

    public static void LogLevelEnd(string levelName, float currentMoney) {
        AnalyticsService.Instance.CustomData("levelEnd",  new Dictionary<string, object>
        {
            { "levelName", levelName },
            { "currentMoney", currentMoney }
        });
    }

    public static void LogDeath(string levelName, int timeMS) {
        AnalyticsService.Instance.CustomData("death",  new Dictionary<string, object>
        {
            { "levelName", levelName },
            { "time", timeMS }
        });
    }

    public static void LogCollision(string levelName, float x, float y, float damage, int timeMS) {
        AnalyticsService.Instance.CustomData("death",  new Dictionary<string, object>
        {
            { "levelName", levelName },
            { "x", x },
            { "y", y },
            { "damage", damage },
            { "time", timeMS }
        });
    }
}
