using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Model;
using SSC.Infrastructure.Utilities;

namespace DataAccess.Repositories
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        public TEntity Delete(TEntity dto)
        {
            TEntity entity = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    entity = db.Set<TEntity>().Find(dto.Id);
                    if (entity != null)
                    {
                        entity = db.Set<TEntity>().Remove(entity);
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
                entity = null;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IEnumerable<TEntity> result = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    result = db.Set<TEntity>().Where(x => !x.IsDelete).Where(predicate).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
            return result ?? Enumerable.Empty<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> result = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    result = db.Set<TEntity>().Where(x => !x.IsDelete).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
            return result ?? Enumerable.Empty<TEntity>();
        }

        public virtual TEntity GetById(Guid id)
        {
            TEntity entity = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    entity = db.Set<TEntity>().Find(id);
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }
            return entity;
        }

        public virtual IEnumerable<TEntity> GetRange(int pageIndex, int pageSize)
        {
            IEnumerable<TEntity> result = null;

            try
            {
                using (var db = new FinanceContext())
                {
                    result = db.Set<TEntity>().Where(i => !i.IsDelete).OrderBy(i => i.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize)
        {
            IEnumerable<TEntity> result = null;

            try
            {
                using (var db = new FinanceContext())
                {
                    result = db.Set<TEntity>().Where(i => !i.IsDelete).Where(predicate).OrderBy(i => i.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetRange(int pageIndex, int pageSize, out int count)
        {
            IEnumerable<TEntity> result = null;
            count = 0;
            try
            {
                using (var db = new FinanceContext())
                {
                    var list = db.Set<TEntity>().Where(i => !i.IsDelete);
                    count = list.Count();
                    result = list.OrderBy(i => i.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> predicate, int pageIndex, int pageSize, out int count)
        {
            count = 0;
            IEnumerable<TEntity> result = null;

            try
            {
                using (var db = new FinanceContext())
                {
                    var list = db.Set<TEntity>().Where(i => !i.IsDelete).Where(predicate).ToList();
                    count = list.Count();
                    result = list.Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return result ?? Enumerable.Empty<TEntity>();
        }

        public IEnumerable<TEntity> Insert(IEnumerable<TEntity> list)
        {
            using (var db = new FinanceContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    IEnumerable<TEntity> tList = null;
                    try
                    {
                        tList = db.Set<TEntity>().AddRange(list);
                        db.SaveChanges();
                        trans.Commit();
                    }
                    catch (DbException ex)
                    {
                        Logger.Error(ex.Message, ex);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();//Error Occured during data saved. Transaction Rolled Back
                        Logger.Error(ex.Message, ex);
                    }
                    return tList;
                }
            }
        }

        public virtual TEntity Insert(TEntity dto)
        {
            TEntity entity = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    entity = db.Set<TEntity>().Add(dto);
                    db.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                entity = null;
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
                entity = null;
            }
            catch (Exception ex)
            {
                entity = null;
                Logger.Error(ex.Message, ex);
            }
            return entity;
        }

        public virtual TEntity SingleBy(Expression<Func<TEntity, bool>> predicate)
        {
            TEntity entity = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    entity = db.Set<TEntity>().Where(x => !x.IsDelete).FirstOrDefault(predicate);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }

        public bool Update(List<TEntity> dtoList)
        {
            using (var db = new FinanceContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    TEntity entity = null;
                    try
                    {
                        var dbSet = db.Set<TEntity>();
                        dtoList.ForEach(x =>
                        {
                            entity = dbSet.Find(x.Id);
                            if (entity != null)
                            {
                                db.Entry(entity).CurrentValues.SetValues(x);
                            }
                        });
                        db.SaveChanges();
                        trans.Commit();
                        return true;
                    }
                    catch (DbEntityValidationException dbEx)
                    {
                        foreach (var validationErrors in dbEx.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                            }
                        }
                        return false;
                    }
                    catch (DbException ex)
                    {
                        Logger.Error(ex.Message, ex);
                        return false;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();//Error Occured during data saved. Transaction Rolled Back
                        Logger.Error(ex.Message, ex);
                        return false;
                    }
                }
            }
        }

        public virtual TEntity Update(TEntity dto)
        {
            TEntity entity = null;
            try
            {
                using (var db = new FinanceContext())
                {
                    entity = db.Set<TEntity>().Find(dto.Id);
                    if (entity != null)
                    {
                        db.Entry(entity).CurrentValues.SetValues(dto);
                        db.SaveChanges();
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Logger.Error(string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage));
                    }
                }
                entity = null;
            }
            catch (DbException ex)
            {
                Logger.Error(ex.Message, ex);
                entity = null;
            }
            catch (Exception ex)
            {
                entity = null;
                Logger.Error(ex.Message, ex);
            }

            return entity;
        }
    }
}
