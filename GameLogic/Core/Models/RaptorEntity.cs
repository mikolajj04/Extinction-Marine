using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class RaptorEntity : DinosaurEntity
    {
        public RaptorEntity() : base(20f, 15f, 6.5f, 15f)
        {

        }

        protected override void Die() { }
    }
}
