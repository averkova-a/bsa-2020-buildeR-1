﻿using buildeR.DAL.Entities.Common;
using buildeR.DAL.Enums;
using System.Collections.Generic;

namespace buildeR.DAL.Entities
{
    public class SocialNetwork: Entity
    {
        public SocialNetwork()
        {
            UserSocialNetworks = new HashSet<UserSocialNetwork>();
        }

        public Provider ProviderName { get; set; }

        public virtual ICollection<UserSocialNetwork> UserSocialNetworks { get; set; }
    }
}