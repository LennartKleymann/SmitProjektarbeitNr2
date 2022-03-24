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
                return Ok(entries.Select(x => new UserDto() { Cn = x.Attributes["cn"].FirstOrDefault(), Sn = x.Attributes["sn"].FirstOrDefault() }));
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
