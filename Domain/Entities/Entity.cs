using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity<TKey>
    {

        //private IValidation 

        protected Entity(TKey id)
        {
            Id = id;
            CreateTime = DateTimeOffset.Now;
            ModifyTime = DateTimeOffset.Now;
        }

        /// <summary>
        /// 唯一标识
        /// </summary>
        [Required]
        public TKey Id { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; private set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset ModifyTime { get; set; }


        public override bool Equals(object entity)
        {
            if (entity == null)
            {
                return false;
            }
            if (!(entity is Entity<TKey>))
            {
                return false;
            }
            return this == (Entity<TKey>)entity;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TKey> entity1, Entity<TKey> entity2)
        {
            if ((object)entity1 == null && (object)entity2 == null)
                return true;
            if ((object)entity1 == null || (object)entity2 == null)
                return false;
            if (entity1.Id == null)
                return false;
            if (entity1.Id.Equals(default(TKey)))
                return false;
            return entity1.Id.Equals(entity2.Id);
        }

        public static bool operator !=(Entity<TKey> entity1, Entity<TKey> entity2)
        {
            return !(entity1 == entity2);
        }

        private StringBuilder _description;

        public override string ToString()
        {
            _description = new StringBuilder();
            AddDescriptions();
            return _description.ToString().TrimEnd().TrimEnd(',');
        }

        protected virtual void AddDescriptions()
        {
        }

        protected void AddDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return;
            _description.Append(description);
        }

        protected void AddDescription<T>(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(value.ToString()))
                return;
            _description.AppendFormat("{0}:{1},", name, value);
        }
    }
}

