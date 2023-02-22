using System.ComponentModel.DataAnnotations;

using Domain.Core.Models;

namespace Domain.Entities
{

    public class AnnualLeave : AggregateRoot<int>
    {
        public AnnualLeave(string no, string name, int annually, DateTime? joinTime, DateTime? startTime, int annualLeaveCount, bool isRetired, int id = default) : this(id)
        {
            No = no ?? throw new ArgumentNullException(nameof(no));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Annually = annually;
            JoinTime = joinTime;
            StartTime = startTime;
            AnnualLeaveCount = annualLeaveCount;
            IsRetired = isRetired;
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
    }
}
