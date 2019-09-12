using System;

namespace Library
{
    public static class CustomEvent<T>
    {
        public static EventGridEvent CreateCustomEvent(T obj)
        {
            return new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow,
                EventType = "MyCustomEventType",
                Subject = "MyCustomEventSubject",
                Data = obj
            };
        }
    }

    public class CustomData
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
    }


    public class CustomEvents<T>
    {

        public string Id { get; private set; }

        public string EventType { get; set; }

        public string Subject { get; set; }

        public string EventTime { get; private set; }

        public T Data { get; set; }

        public CustomEvents()
        {
            Id = Guid.NewGuid().ToString();

            DateTime localTime = DateTime.Now;
            DateTime utcTime = DateTime.UtcNow;
            DateTimeOffset localTimeAndOffset = new DateTimeOffset(localTime, TimeZoneInfo.Local.GetUtcOffset(localTime));
            EventTime = localTimeAndOffset.ToString("o");
        }

        public CustomEvents(T obj)
        {

            Id = Guid.NewGuid().ToString();
            EventTime = DateTime.UtcNow.ToShortDateString();
            EventType = "MyCustomEventType";
            Subject = "MyCustomEventSubject";
            Data = obj;
        }

    }
}
