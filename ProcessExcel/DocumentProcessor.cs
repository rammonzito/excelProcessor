﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public abstract class DocumentProcessor
    {
        public abstract void Execute(string path);
    }
}
