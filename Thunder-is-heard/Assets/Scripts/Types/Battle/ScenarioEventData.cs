using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class ScenarioEventData
{
    [JsonProperty("eventType")]
    public string eventType;
    
    [JsonProperty("delay")]
    public float delay = 0f; // Задержка перед выполнением события
    
    [JsonProperty("waitForCompletion")]
    public bool waitForCompletion = true; // Ждать ли завершения события перед следующим
    
    [JsonProperty("executeInParallel")]
    public bool executeInParallel = false; // Выполнять ли событие параллельно с другими

    // Общие параметры для всех типов событий
    [JsonProperty("parameters")]
    [JsonConverter(typeof(ScenarioEventParametersConverter))]
    public Dictionary<string, object> parameters = new Dictionary<string, object>();

    public ScenarioEventData() { }

    public ScenarioEventData(string type, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        eventType = type;
        this.delay = delay;
        this.waitForCompletion = waitForCompletion;
        this.executeInParallel = executeInParallel;
    }

    // Вспомогательные методы для работы с параметрами
    public T GetParameter<T>(string key, T defaultValue = default(T))
    {
        if (parameters.ContainsKey(key) && parameters[key] is T)
        {
            return (T)parameters[key];
        }
        return defaultValue;
    }

    public void SetParameter<T>(string key, T value)
    {
        parameters[key] = value;
    }

    // Статические методы для создания конкретных типов событий
    public static ScenarioEventData CreateUnitAttack(string attackerUnitId, string targetId, bool instantKill = false, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("UnitAttack", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("attackerUnitId", attackerUnitId);
        eventData.SetParameter("targetId", targetId);
        eventData.SetParameter("instantKill", instantKill);
        return eventData;
    }

    public static ScenarioEventData CreateMultiUnitAttack(string[] attackerUnitIds, string[] targetIds, bool instantKill = false, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = true)
    {
        var eventData = new ScenarioEventData("MultiUnitAttack", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("attackerUnitIds", attackerUnitIds);
        eventData.SetParameter("targetIds", targetIds);
        eventData.SetParameter("instantKill", instantKill);
        return eventData;
    }

    public static ScenarioEventData CreateUnitMove(string unitId, List<Bector2Int> route, float moveSpeed = 1f, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("UnitMove", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("unitId", unitId);
        eventData.SetParameter("route", route);
        eventData.SetParameter("moveSpeed", moveSpeed);
        return eventData;
    }

    public static ScenarioEventData CreateUnitRotate(string unitId, Bector2Int targetPosition, int rotation = 0, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("UnitRotate", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("unitId", unitId);
        eventData.SetParameter("targetPosition", targetPosition);
        eventData.SetParameter("rotation", rotation);
        return eventData;
    }

    public static ScenarioEventData CreateUnitRotate(string unitId, int rotation, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("UnitRotate", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("unitId", unitId);
        eventData.SetParameter("rotation", rotation);
        return eventData;
    }

    public static ScenarioEventData CreateUnitDeath(string unitId, bool playAnimation = true, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("UnitDeath", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("unitId", unitId);
        eventData.SetParameter("playAnimation", playAnimation);
        return eventData;
    }

    public static ScenarioEventData CreateMultiUnitDeath(string[] unitIds, bool playAnimation = true, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = true)
    {
        var eventData = new ScenarioEventData("MultiUnitDeath", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("unitIds", unitIds);
        eventData.SetParameter("playAnimation", playAnimation);
        return eventData;
    }

    public static ScenarioEventData CreateWait(float waitTime, float delay = 0f, bool waitForCompletion = true, bool executeInParallel = false)
    {
        var eventData = new ScenarioEventData("Wait", delay, waitForCompletion, executeInParallel);
        eventData.SetParameter("waitTime", waitTime);
        return eventData;
    }
}

// Кастомный конвертер для правильной сериализации Dictionary<string, object>
public class ScenarioEventParametersConverter : JsonConverter<Dictionary<string, object>>
{
    public override Dictionary<string, object> ReadJson(JsonReader reader, Type objectType, Dictionary<string, object> existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var dict = new Dictionary<string, object>();
        var jObject = JObject.Load(reader);
        
        foreach (var property in jObject.Properties())
        {
            var value = property.Value;
            object convertedValue = null;
            
            switch (value.Type)
            {
                case JTokenType.String:
                    convertedValue = value.Value<string>();
                    break;
                case JTokenType.Integer:
                    convertedValue = value.Value<int>();
                    break;
                case JTokenType.Float:
                    convertedValue = value.Value<float>();
                    break;
                case JTokenType.Boolean:
                    convertedValue = value.Value<bool>();
                    break;
                case JTokenType.Array:
                    // Обрабатываем массивы
                    if (value.HasValues && value.First.Type == JTokenType.Object)
                    {
                        // Массив объектов (например, Bector2Int)
                        var array = new List<object>();
                        foreach (var item in value)
                        {
                            array.Add(item.ToObject<object>(serializer));
                        }
                        convertedValue = array;
                    }
                    else
                    {
                        // Простой массив
                        convertedValue = value.ToObject<object[]>(serializer);
                    }
                    break;
                case JTokenType.Object:
                    // Обрабатываем объекты (например, Bector2Int)
                    convertedValue = value.ToObject<object>(serializer);
                    break;
                default:
                    convertedValue = value.Value<object>();
                    break;
            }
            
            dict[property.Name] = convertedValue;
        }
        
        return dict;
    }

    public override void WriteJson(JsonWriter writer, Dictionary<string, object> value, JsonSerializer serializer)
    {
        writer.WriteStartObject();
        
        foreach (var kvp in value)
        {
            writer.WritePropertyName(kvp.Key);
            serializer.Serialize(writer, kvp.Value);
        }
        
        writer.WriteEndObject();
    }
}