using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class MicroRaptorEntity : DinosaurEntity
    {
        public MicroRaptorEntity() : base(5f, 8f, 10f, 7f)
        {

        }

        protected override void Die() { }
    }
}
