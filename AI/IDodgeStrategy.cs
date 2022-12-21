public interface IDodgeStrategy
{
    void Init(AIController controller);
    bool CanDodge(Unit target);
    void Dodge(Unit target);
}
