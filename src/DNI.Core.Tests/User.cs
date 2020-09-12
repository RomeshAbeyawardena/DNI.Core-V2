using MessagePack;

using System;

namespace DNI.Core.Tests
{
    public partial class DistributedCacheServiceTests
    {
        [MessagePackObject(true)]
        public class User
        {
            public int Id { get; set; }
            public string EmailAddress { get; set; }
            public string Surname { get; set; }

            public override bool Equals(object obj)
            {
                if(obj is User user)
                {
                    return Equals(user);
                }
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Id, EmailAddress, Surname);
            }

            private bool Equals(User user)
            {
                return user.EmailAddress.Equals(EmailAddress)
                    && user.Id == Id
                    && user.Surname.Equals(Surname);
            }
        }
    }
}
