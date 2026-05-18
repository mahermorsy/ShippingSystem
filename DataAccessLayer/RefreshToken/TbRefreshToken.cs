
using System;
using System.Collections.Generic;
using System.Text;
using Domains.Models;
using DataAccessLayer.Identity;


namespace DataAccessLayer.RefreshToken
{
    public class TbRefreshToken : BaseTable
    {
        public string Token { get; set; } = null!;

        public DateTime ExpiresOn { get; set; }

        public Guid UserId { get; set; }

        public ApplicationUser User { get; set; } = null!;
    }
}
