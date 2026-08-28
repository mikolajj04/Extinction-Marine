using System;
using System.Collections.Generic;
using System.Text;

namespace GameLogic.Core.Models
{
    public class DilophosaurusEntity : DinosaurEntity
    {
        public DilophosaurusEntity() : base(35f, 25f, 11f, 60f)
        {
            IsSneaky = true;
            
        }


       
    }
}
