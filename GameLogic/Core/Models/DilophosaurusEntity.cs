using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class DilophosaurusEntity : DinosaurEntity
    {
        public DilophosaurusEntity() : base(30f, 25f, 9f, 60f)
        {
            IsSneaky = true;
            
        }

    }
}
