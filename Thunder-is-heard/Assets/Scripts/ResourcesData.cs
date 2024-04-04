
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
    public int MaxRub
    {
        get { return maxRub; }
        set { }
    }

    [SerializeField] public int frames;
    public int Frames
    {
        get { return frames; }
        set { }
    }

    [SerializeField] public int maxFrames;
    public int MaxFrames
    {
        get { return maxFrames; }
        set { }
    }

    [SerializeField] public int oil;
    public int Oil
    {
        get { return oil; }
        set { }
    }

    [SerializeField] public int maxOil;
    public int MaxOil
    {
        get { return maxOil; }
        set { }
    }

    [SerializeField] public int steel;
    public int Steel
    {
        get { return steel; }
        set { }
    }

    [SerializeField] public int maxSteel;
    public int MaxSteel
    {
        get { return maxSteel; }
        set { }
    }

    [SerializeField] public int staff;
    public int Staff
    {
        get { return staff; }
        set { }
    }

    [SerializeField] public int maxStaff;
    public int MaxStaff
    {
        get { return Staff; }
        set { }
    }

    [SerializeField] public int exp;
    public int Exp
    {
        get { return exp; }
        set { }
    }

    [SerializeField] public int maxExp;
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
}
