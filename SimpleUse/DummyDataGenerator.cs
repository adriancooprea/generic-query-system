using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleUse
{
    public class DummyDataGenerator
    {
        public IList<SimpleModel> GenerateDummyData()
        {
            var result = new List<SimpleModel>
            {
                new SimpleModel
                {
                    Id = 1,
                    Name = "Abc",
                    Enabled = true,
                    Date = new DateTime(2018, 1, 1),
                    TimeSpan = new DateTime(2018, 1, 1, 12, 0, 0).TimeOfDay,
                    SimpleEnum = SimpleEnum.Option0
                },
                new SimpleModel
                {
                    Id = 2,
                    Name = "",
                    Enabled = false,
                    Date = new DateTime(2017, 1, 1),
                    TimeSpan = new DateTime(2017, 1, 1, 1, 0, 0).TimeOfDay,
                    SimpleEnum = SimpleEnum.Option1
                },
                new SimpleModel
                {
                    Id = 3,
                    Name = "Dcf",
                    Enabled = true,
                    Date = new DateTime(2015, 1, 1),
                    TimeSpan = new DateTime(2015, 1, 1, 7, 0, 0).TimeOfDay,
                    SimpleEnum = SimpleEnum.Option0
                }
            };

            return result;
        }
    }
}