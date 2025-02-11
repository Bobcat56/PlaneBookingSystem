using Data.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserDBRepository
    {
        private AirlineDbContext _AirLineDBContext;

        //Constructor
        public UserDBRepository(AirlineDbContext airlineDbContext)
        {
            _AirLineDBContext = airlineDbContext;
        }

        public AdminUser? CheckAdmin(string name)
        {
            return _AirLineDBContext.AdminUsers.SingleOrDefault(x => x.UserName == name && x.IsAdmin == true);
        }
    }
}
