

public abstract class BasicCondition : ICondition
{
    protected bool _isActive = false;
    
    public abstract bool IsComply();

    public abstract bool IsRealTimeUpdate();
    
    public virtual void Activate()
    {
        if (!_isActive)
        {
            _isActive = true;
            OnActivate();
        }
    }
    
    public virtual void Deactivate()
    {
        if (_isActive)
        {
            _isActive = false;
            OnDeactivate();
        }
    }
    
    public virtual void Reset()
    {
        OnReset();
    }
    
    /// <summary>
    /// Переопределяется в наследниках для активации слушателей событий
    /// </summary>
    protected virtual void OnActivate() { }
    
    /// <summary>
    /// Переопределяется в наследниках для деактивации слушателей событий
    /// </summary>
    protected virtual void OnDeactivate() { }
    
    /// <summary>
    /// Переопределяется в наследниках для сброса состояния
    /// </summary>
    protected virtual void OnReset() { }
}
