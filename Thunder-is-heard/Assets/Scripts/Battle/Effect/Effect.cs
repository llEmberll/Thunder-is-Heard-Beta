

public abstract class Effect : IEffect
{
    public string _name;
    public string Name { get { return _name; } }


    public string _type;
    public string Type { get { return _type; } }


    public int _duration;
    public int Duration { get { return _duration; } set { _duration = value; } }


    public ICondition[] _conditionsForEnd;


    public Effect(string name, string type, int duration, ICondition[] conditionsForEnd)
    {
        _name = name;
        _type = type;
        _duration = duration;
        _conditionsForEnd = conditionsForEnd;
    }

    public virtual bool IsEnd()
    {
        foreach (var condition in _conditionsForEnd)
        {
            if (!condition.IsComply())
            {
                return false;
            }
        }

        return true;
    }

    public virtual bool IsMustTic()
    {
        return true;
    }

    public virtual void Tic()
    {

    }
}
