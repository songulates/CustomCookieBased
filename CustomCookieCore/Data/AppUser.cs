using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomCookieCore.Data
{
    //bir userın birdenfazla userrole olabilir,bir rolun birden fazla usserrole olabilir
    //bir rolde birden fazla user ve bir userdada birden fazla role olabilir.
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        //bir kullanıcının birden fazla userrole olabilir
        public List<AppUserRole> UserRoles { get; set; }
        //bir appuserin birden fazla userrole olabilir
    }
    public class AppRole
    {
        public int Id { get; set; }
        public string Definition { get; set; }
        public List<AppUserRole> UserRoles { get; set; }
        //bir role birden fazla kullanıcı tarafından kulanılabilir
        //bir rolun birdenf fazla  userrole olabilir
    }
    public class AppUserRole
    {
        public int UserId { get; set; }
        //navigation property
        //bir user  ıd ye karşılık gelen bir user var
        //bir userrole ün bir userı var
        //bir AppUserRoleün bir AppUserü olur
        public AppUser AppUser { get; set; }
        public int RoleId { get; set; }
        //navigation property
        public AppRole AppRole { get; set; }
    }
}
