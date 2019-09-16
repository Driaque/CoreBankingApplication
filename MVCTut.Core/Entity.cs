using System;

namespace MVCTut.Core
{

    public class Entity
    {
        public virtual int Id { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual DateTime DateUpdated { get; set; }
    }
}