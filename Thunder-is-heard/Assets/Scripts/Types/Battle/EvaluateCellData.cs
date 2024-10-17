

public class EvaluateCellData : CellData
{
    // Добавлены переменные для алгоритма A*
    public int gScore; // Стоимость перехода к этой клетке
    public int hScore; // Оценка стоимости перехода от этой клетки к целевой
    public int fScore; // Общая оценка
    public EvaluateCellData parent; // Родительская клетка

    public EvaluateCellData(string cellType, Bector2Int cellPosition, bool isOccypy = false) : base(cellType, cellPosition, isOccypy)
    {
    }

    public EvaluateCellData(CellData cell) : base(cell._type, cell._position, cell._isOccypy)
    {
    }
}
