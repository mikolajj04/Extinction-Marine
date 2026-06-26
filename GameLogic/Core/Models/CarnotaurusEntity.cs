using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class CarnotaurusEntity : DinosaurEntity
    {
        public CarnotaurusEntity() : base(1000f, 70f, 7.5f, 500f)
        {

        }

        protected override void Die() { }
    }
}
