﻿using buildeR.DAL.Entities.Common;
using buildeR.DAL.Enums;

namespace buildeR.DAL.Entities
{
    public class TeamMember: Entity
    {
        public int UserId { get; set; }
        public UserRole MemberRole { get; set; }
        public int GroupId { get; set; }

        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}