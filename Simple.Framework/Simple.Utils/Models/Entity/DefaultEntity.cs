using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simple.Utils.Models.Entity
{
    /// <summary>主键为int型的基类</summary>
    public class DefaultEntityInt : DefaultBaseEntity<int>
    {
    }

    /// <summary>主键为Guid醒的基类</summary>
    public class DefaultEntityGuid : DefaultBaseEntity<Guid>
    {
    }

    /// <summary>基础模型 包含属性 Id、UpdateTime、CreateTime</summary>
    /// <typeparam name="TKey"></typeparam>
    public class DefaultBaseEntity<TKey>
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; } = default;

        /// <summary>是否已删除</summary>
        public bool Deleted { get; set; } = false;

        /// <summary>更新时间</summary>
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>创建时间</summary>
        public virtual DateTime CreateTime { get; set; } = DateTime.Now;
    }
}