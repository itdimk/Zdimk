﻿using System;

namespace Zdimk.Application.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(string message) : base(message)
        {
            
        }
    }
}