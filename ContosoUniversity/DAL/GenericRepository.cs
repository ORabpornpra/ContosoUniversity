using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.Entity;
using ContosoUniversity.Models;
using System.Linq.Expressions;

namespace ContosoUniversity.DAL
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal SchoolContext context; //object for database contex
        internal DbSet<TEntity> dbSet; //object for entity set

        public GenericRepository(SchoolContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        // Func is use for the lambda Expression
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "") // if no parameters are pass in, by default filter, orderBy, and includeProperties are equal to null
        {

            /* Explaination for the lambda Expression:
             
             * The code Expression<Func<TEntity, bool>> filter means the caller will provide a lambda expression based on the TEntity type, 
             * and this expression will return a Boolean value. For example, if the repository is instantiated for the Student entity type, 
             * the code in the calling method might specify student => student.LastName == "Smith" for the filter parameter.
             * 
             * The code Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy also means the caller will provide a lambda expression. 
             * But in this case, the input to the expression is an IQueryable object for the TEntity type. The expression will return an ordered 
             * version of that IQueryable object. For example, if the repository is instantiated for the Student entity type, the code in the 
             * calling method might specify q => q.OrderBy(s => s.LastName) for the orderBy parameter.
             */

            IQueryable<TEntity> query = dbSet;

            if (filter != null)//The code in the Get method creates an IQueryable object and then applies the filter expression if there is one:
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))//applies the eager-loading expressions after parsing the comma-delimited list
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)//applies the orderBy expression if there is one and returns the results; otherwise it returns the results from the unordered query
            {
                return orderBy(query).ToList();//create the query
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);// can't have an Eager Loading for the find method
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)//for concurrency handling you need a Delete method that takes 
            //an entity instance that includes the original value of a tracking property.
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual IEnumerable<TEntity> GetWithRawSql(string query, params object[] parameters)//return entity types
        {
            return dbSet.SqlQuery(query, parameters).ToList();//dbSet.SqlQuery to connect directly to the database
        }

    }
}