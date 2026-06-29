using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class MicroRaptorEntity : DinosaurEntity
    {
        public MicroRaptorEntity() : base(6.5f, 10f, 10f, 10f)
        {

        }

        protected override void Die() { }
    }
}
