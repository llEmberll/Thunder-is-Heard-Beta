


public interface ICondition
{
    public bool IsComply();

    public bool IsRealTimeUpdate();
    
    /// <summary>
    /// Активирует условие - подписывает на события, инициализирует слушатели
    /// </summary>
    public void Activate();
    
    /// <summary>
    /// Деактивирует условие - отписывает от событий, очищает слушатели
    /// </summary>
    public void Deactivate();
    
    /// <summary>
    /// Сбрасывает состояние условия к начальному
    /// </summary>
    public void Reset();
}
