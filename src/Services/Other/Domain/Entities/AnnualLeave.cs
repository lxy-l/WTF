using System.ComponentModel.DataAnnotations;

using Crafty.Domain.Core.Models;

namespace Domain.Entities
{

    public class AnnualLeave : Entity<int>,IAggregateRoot
    {
        public AnnualLeave(string no, string name, int annually,string gender, DateTime birthday,DateTime? retireTime, DateTime? joinTime, DateTime? startTime, int annualLeaveCount, bool isRetired, int id = default) : this(id)
        {
            No = no ?? throw new ArgumentNullException(nameof(no));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Annually = annually;
            JoinTime = joinTime;
            StartTime = startTime;
            AnnualLeaveCount = annualLeaveCount;
            IsRetired = isRetired;
            Birthday = birthday;
            RetireTime = retireTime;
            Gender=gender;
        }

        private AnnualLeave(int id) : base(id)
        {
        }

        /// <summary>
        /// 工号
        /// </summary>
        [StringLength(55)]
        public string No { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [StringLength(255)]
        public string Name { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 年度
        /// </summary>
        public int Annually { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? JoinTime  { get; set; }

        /// <summary>
        /// 工龄起算时间
        /// </summary>
        public DateTime? StartTime  { get; set; }

        /// <summary>
        /// 年假天数
        /// </summary>
        public int AnnualLeaveCount { get; set; }

        /// <summary>
        /// 是否退休
        /// </summary>
        public bool IsRetired { get; set; }

        /// <summary>
        /// 退休时间
        /// </summary>
        public DateTime? RetireTime { get; set; }
    }
}
