using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class TRexEntity : DinosaurEntity
    {
        
        public TRexEntity() : base(200f, 50f, 4f, 500f)
        {
        }

        protected override void Die() { }
    }

}
