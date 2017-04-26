using System;
using Conference.Core.Entities.Questions;

namespace Conference.Core.Entities
{
    public class Respondent : BaseEntity
    {
        public string Name { get; set; }

        public string Uid { get; set; }

        public Guid SessionId { get; set; }

        public virtual Question CurrentQuestion { get; set; }
    }
}