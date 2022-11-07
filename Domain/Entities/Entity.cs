using System.ComponentModel.DataAnnotations;
using System.Text;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    [Index(nameof(CreateTime), IsUnique = false)]
    [Index(nameof(ModifyTime), IsUnique = false)]
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
        [Key]
        [Required]
        public TKey Id { get; private set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreateTime { get; private set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTimeOffset ModifyTime { get; protected set; }


        public override bool Equals(object? entity)
        {
            if (entity is null)
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
            if (Id is null)
            {
                throw new Exception("主键为空！");
            }
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TKey> entity1, Entity<TKey> entity2)
        {
            if (entity1 is null && entity2 is null)
                return true;
            if (entity1 is null || entity2 is null)
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

        private StringBuilder? _description;

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
            _description?.Append(description);
        }

        protected void AddDescription<T>(string name, T value)
        {
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return;
            _description?.AppendFormat("{0}:{1},", name, value);
        }
    }
}

