using LdapForNet;
using LdapForNet.Native;
using Microsoft.AspNetCore.Mvc;
using static LdapForNet.Native.Native;

namespace Ldapsmit.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public class UserDto
        {
            public string Cn { get; set; }
            public string Sn { get; set; }
            public string Password { get; set; }
        }

        string auth = Native.LdapAuthMechanism.SIMPLE;
        string host = "65.21.53.215";
        string @base = "dc=example,dc=org";
        int port = 1389;
        string who = "cn=admin,dc=example,dc=org";
        string password = "adminpassword";

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            using (var cn = new LdapConnection())
            {
                cn.Connect(host, port);

                var who = "cn=admin,dc=example,dc=org";
                var password = "adminpassword";
                cn.Bind(auth, who, password);

                IList<LdapEntry> entries;


                entries = await cn.SearchAsync(@base, "(objectClass=inetOrgPerson)");
                var result = new List<UserDto>();
                foreach (var entry in entries)
                {
                    var user = new UserDto();
                    user.Cn = (entry.Attributes.ContainsKey("cn") && entry.Attributes["cn"].Any()) ? entry.Attributes["cn"].FirstOrDefault() : null;
                    user.Sn = (entry.Attributes.ContainsKey("sn") && entry.Attributes["sn"].Any()) ? entry.Attributes["sn"].FirstOrDefault() : null;
                    user.Password = (entry.Attributes.ContainsKey("userPassword") && entry.Attributes["userPassword"].Any())  ? entry.Attributes["userPassword"].FirstOrDefault() : null;
                    result.Add(user);
                }

                return Ok(result);
            }
        }


        [HttpPut("{cn}")]
        public async Task<IActionResult> EditUser(string cn, [FromForm] UserDto userDto)
        {
            using (var con = new LdapConnection())
            {
                con.Connect(host, port);

                var who = "cn=admin,dc=example,dc=org";
                var password = "adminpassword";
                con.Bind(auth, who, password);

                var usersToEdit = await con.SearchAsync(@base, $"(&(objectClass=inetOrgPerson)(cn={cn}))");
                var userToEdit = usersToEdit.FirstOrDefault();

                if (userToEdit == null)
                    return NotFound();

                try
                {
                    con.Modify(new LdapModifyEntry
                    {
                        Dn = userToEdit.Dn,
                        Attributes = new List<LdapModifyAttribute>
                    {
                        new LdapModifyAttribute
                        {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Type = "sn",
                        Values = new List<string> {userDto.Sn}
                        },
                            new LdapModifyAttribute
                        {
                        LdapModOperation = LdapModOperation.LDAP_MOD_REPLACE,
                        Type = "userPassword",
                        Values = new List<string> {userDto.Password}
                        },
                    }
                    });

                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }

        }

        [HttpDelete("{cn}")]
        public async Task<IActionResult> DeleteUser(string cn)
        {
            using (var con = new LdapConnection())
            {
                con.Connect(host, port);

                var who = "cn=admin,dc=example,dc=org";
                var password = "adminpassword";
                con.Bind(auth, who, password);

                var usersToDelete = await con.SearchAsync(@base, $"(&(objectClass=inetOrgPerson)(cn={cn}))");
                var userToDelete = usersToDelete.FirstOrDefault();

                if (userToDelete == null)
                    return NotFound();

                try
                {
                    await con.DeleteAsync(userToDelete.Dn);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }

        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromForm] UserDto userDto)
        {
            using (var cn = new LdapConnection())
            {
                cn.Connect(host, port);

                cn.Bind(Native.LdapAuthMechanism.SIMPLE, who, password);
                Random rnd = new Random();

                try
                {
                    await cn.AddAsync(new LdapEntry
                    {
                        Dn = $"cn={userDto.Cn},ou=users,dc=example,dc=org",
                        Attributes = new Dictionary<string, List<string>>
                    {
                        {"uid", new List<string>() {userDto.Cn } },
                        {"userPassword", new List<string>() {userDto.Password } },
                        {"uidNumber", new List<string>() { rnd.Next(100000).ToString()  } },
                        {"gidNumber", new List<string>() { rnd.Next(100000).ToString() } },
                        {"homeDirectory", new List<string>() { $"/home/${userDto.Cn}" } },
                        {"sn", new List<string> { userDto.Sn }},
                        {"objectclass", new List<string> {"inetOrgPerson", "posixAccount", "shadowAccount"}},
                    }
                    });
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();
            }
        }
    }
}
