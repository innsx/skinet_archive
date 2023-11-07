using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private Hashtable _hashtableRepositories;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;            
        }

        public async Task<int> Complete()
        {
            return await _storeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storeContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // Check if repository has the pass in TENTITY type in HashTable
            if (_hashtableRepositories == null)
            {
                _hashtableRepositories = new Hashtable();
            }

            // if HashTable DOES NOT have an TENTITY, then get the TENTITY by name
            var entityByName = typeof(TEntity).Name;

            // if the TENTITY is NOT in HashtableRespository
            if (!_hashtableRepositories.ContainsKey(entityByName))
            {
                var respositoryWithEntities = typeof(GenericRepository<>);

                var createRepositoryInstance = Activator.CreateInstance(respositoryWithEntities.MakeGenericType(typeof(TEntity)), _storeContext);

                _hashtableRepositories.Add(entityByName, createRepositoryInstance);
            }
            return (IGenericRepository<TEntity>) _hashtableRepositories[entityByName];
        }
    }
}