using System.Collections.Generic;
using UnityEngine;

namespace SchizoQuest.Helpers
{
    public static class Collision2DHelpers
    {
        public static IEnumerable<ContactPoint2D> GetContacts(this Collision2D collision)
        {
            int count = collision.contactCount;
            for (int i = 0; i < count; i++)
            {
                yield return collision.GetContact(i);
            }
        }
    }
}