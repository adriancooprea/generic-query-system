using System;

namespace SimpleUse
{
    public class SimpleModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public bool Enabled { get; set; }

        public DateTime Date { get; set; }
        
        public TimeSpan TimeSpan { get; set; }

        public SimpleEnum SimpleEnum { get; set; }

    }
}
