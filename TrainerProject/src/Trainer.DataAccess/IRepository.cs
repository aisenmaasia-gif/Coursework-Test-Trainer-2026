namespace Trainer.DataAccess;

public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    void SaveAll(IEnumerable<T> entities);
}