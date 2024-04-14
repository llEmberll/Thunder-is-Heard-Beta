
using Newtonsoft.Json;
using UnityEngine;


[System.Serializable]
public class ResourcesData
{
    public ResourcesData(
        int rubCount = 0, int maxRubCount = 0,
        int framesCount = 0,  int maxFramesCount = 0,
        int oilCount = 0, int maxOilCount = 0,
        int steelCount = 0, int maxSteelCount = 0,
        int staffCount = 0,  int maxStaffCount = 0,
        int expCount = 0, int maxExpCount = 0
        )
    {
        rub = rubCount; maxRub = maxRubCount;
        frames = framesCount; maxFrames = maxFramesCount;
        oil = oilCount; maxOil = maxOilCount;
        steel = steelCount; maxSteel = maxSteelCount;
        staff = staffCount; maxStaff = maxStaffCount;
        exp = expCount; maxExp = maxExpCount;
    }

    [SerializeField] public int rub;
    public int Rub
    {
        get { return rub; }
        set { }
    }

    [SerializeField] public int maxRub;

    [JsonIgnore]
    public int MaxRub
    {
        get { return maxRub; }
        set { }
    }

    [SerializeField] public int frames;

    [JsonIgnore]
    public int Frames
    {
        get { return frames; }
        set { }
    }

    [SerializeField] public int maxFrames;

    [JsonIgnore]
    public int MaxFrames
    {
        get { return maxFrames; }
        set { }
    }

    [SerializeField] public int oil;

    [JsonIgnore]
    public int Oil
    {
        get { return oil; }
        set { }
    }

    [SerializeField] public int maxOil;

    [JsonIgnore]
    public int MaxOil
    {
        get { return maxOil; }
        set { }
    }

    [SerializeField] public int steel;

    [JsonIgnore]
    public int Steel
    {
        get { return steel; }
        set { }
    }

    [SerializeField] public int maxSteel;

    [JsonIgnore]
    public int MaxSteel
    {
        get { return maxSteel; }
        set { }
    }

    [SerializeField] public int staff;

    [JsonIgnore]
    public int Staff
    {
        get { return staff; }
        set { }
    }

    [SerializeField] public int maxStaff;

    [JsonIgnore]
    public int MaxStaff
    {
        get { return Staff; }
        set { }
    }

    [SerializeField] public int exp;

    [JsonIgnore]
    public int Exp
    {
        get { return exp; }
        set { }
    }

    [SerializeField] public int maxExp;

    [JsonIgnore]
    public int MaxExp
    {
        get { return maxExp; }
        set { }
    }

    public ResourcesData Clone()
    {
        ResourcesData clone = new ResourcesData();
        clone.exp = exp;
        clone.rub = rub;
        clone.oil = oil;
        clone.frames = frames;
        clone.steel = steel;
        clone.staff = staff;
        clone.maxExp = maxExp;
        clone.maxFrames = maxFrames;
        clone.maxOil = maxOil;
        clone.maxSteel = maxSteel;
        clone.maxStaff = maxStaff;
        clone.maxRub = maxRub;
     return clone;
    }

    public ResourcesData GetResourcesWithoutLimits()
    {
        ResourcesData clone = new ResourcesData();
        clone.exp = exp;
        clone.rub = rub;
        clone.oil = oil;
        clone.frames = frames;
        clone.steel = steel;
        return clone;
    }

    public ResourcesData GetLimits()
    {
        ResourcesData clone = new ResourcesData();
        clone.maxExp = maxExp;
        clone.maxFrames = maxFrames;
        clone.maxOil = maxOil;
        clone.maxSteel = maxSteel;
        clone.maxStaff = maxStaff;
        clone.maxRub = maxRub;
        return clone;
    }
    
    public void Add(ResourcesData data)
    {
        exp += data.exp;
        rub += data.rub;
        oil += data.oil;
        frames += data.frames;
        steel += data.steel;
        staff += data.staff;

        maxExp += data.maxExp;
        maxFrames += data.maxFrames;
        maxOil += data.maxOil;
        maxSteel += data.maxSteel;
        maxStaff += data.maxStaff;
        maxRub += data.maxRub;
    }

    public void Substract(ResourcesData data)
    {
        exp -= data.exp;
        rub -= data.rub;
        oil -= data.oil;
        frames -= data.frames;
        steel -= data.steel;
        staff -= data.staff;

        maxExp -= data.maxExp;
        maxFrames -= data.maxFrames;
        maxOil -= data.maxOil;
        maxSteel -= data.maxSteel;
        maxStaff -= data.maxStaff;
        maxRub -= data.maxRub;
    }

    public bool IsValid()
    {
        if (
            rub < 0 ||
            oil < 0 ||
            frames < 0 ||
            steel < 0 ||
            staff < 0 ||
            maxExp < 0 ||
            maxFrames < 0 ||
            maxOil < 0 ||
            maxSteel < 0 ||
            maxRub < 0 ||
            maxStaff < 0
            )
        {
            return false;
        }

        return true;
    }

    public bool IsCoveringCost(ResourcesData cost)
    {
        ResourcesData clone = this.Clone();
        clone.Substract(cost);
        return clone.IsValid();
    }

    public bool IsOverflow()
    {
        if (
            maxOil < oil ||
            maxSteel < steel ||
            maxStaff < staff
            )
        {
            return true;
        }

        return false;
    }
}
