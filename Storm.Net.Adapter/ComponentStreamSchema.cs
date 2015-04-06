using System;
using System.Collections.Generic;

namespace Storm
{
    public class ComponentStreamSchema
    {
        public Dictionary<string, List<Type>> InputStreamSchema
        {
            get;
            set;
        }
        public Dictionary<string, List<Type>> OutputStreamSchema
        {
            get;
            set;
        }
        public ComponentStreamSchema(Dictionary<string, List<Type>> input, Dictionary<string, List<Type>> output)
        {
            this.InputStreamSchema = input;
            this.OutputStreamSchema = output;
        }
    }
}