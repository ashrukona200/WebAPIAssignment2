using assigntwo.Data.Entities;

namespace assigntwo.Data
{
    public interface IMatchRepository
    {
        //Adding Match item
        void Add(Match match) ;

        //Deleting Match item
        void Delete(Match match);

        //Updating value for the specified key
        void Update(string key, int value);

        //Saving changes to repository
        Task<bool> SaveChanges();

        //If Match exists by key
        public bool Exist(string key);

        //Getting Match by key
        Match GetMatchbykey(string key);

        
    }
}
