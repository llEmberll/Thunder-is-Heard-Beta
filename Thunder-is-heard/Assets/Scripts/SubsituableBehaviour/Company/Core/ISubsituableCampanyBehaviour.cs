using System.Collections.Generic;

public interface ISubsituableCampanyBehaviour
{
    public void Init(Campany conductor);

    public List<MissionItem> GetItems(Campany conductor);

    public void Load(Campany conductor, MissionDetalization missionData);

    public void Toggle(Campany conductor);

    public void FillContent(Campany conductor);
}
