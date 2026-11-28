namespace Repository.Mapping
{
    public interface IMapper<TModel, TEntity>
    {
        TEntity ToEntity(TModel model);
        TModel ToModel(TEntity entity);
    }
}
