namespace EventManagementApp.Repositories;

public interface IRepository<T>{
    List<T> GetAll();
    T GetById(Guid id);
    void Add(T Entity);
    void Update(T Entity);
    void Delete(T Entity);
}