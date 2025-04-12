

public interface ISubsituableObstacleBehaviour
{
    public void Init(Obstacle conductor);

    public void OnClick(Obstacle conductor);

    public void OnFocus(Obstacle conductor);

    public void OnDefocus(Obstacle conductor);
}
