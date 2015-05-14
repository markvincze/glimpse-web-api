using System;
using Glimpse.Core.Extensibility;
using Glimpse.Core.Framework;
using Glimpse.Core.Message;

namespace Glimpse.Core
{
    public class TimelineMessage : ITimelineMessage
    {
        public TimelineMessage()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public TimeSpan Offset { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime StartTime { get; set; }
        public string EventName { get; set; }
        public TimelineCategoryItem EventCategory { get; set; }
        public string EventSubText { get; set; }
    }

    public static class GlimpseTimeline
    {
        private static readonly TimelineCategoryItem DefaultCategory = new TimelineCategoryItem("User", "green", "blue");

        public static OngoingCapture Capture(string eventName)
        {
            return Capture(eventName, null, DefaultCategory, new TimelineMessage());
        }

        public static OngoingCapture Capture(string eventName, string eventSubText)
        {
            return Capture(eventName, eventSubText, DefaultCategory, new TimelineMessage());
        }

        internal static OngoingCapture Capture(string eventName, TimelineCategoryItem category)
        {
            return Capture(eventName, null, category, new TimelineMessage());
        }

        internal static OngoingCapture Capture(string eventName, TimelineCategoryItem category, ITimelineMessage message)
        {
            return Capture(eventName, null, category, message);
        }

        internal static OngoingCapture Capture(string eventName, ITimelineMessage message)
        {
            return Capture(eventName, null, DefaultCategory, message);
        }

        internal static OngoingCapture Capture(string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

#pragma warning disable 618
            var executionTimer = GlimpseConfiguration.GetConfiguredTimerStrategy()();
            var messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker();
#pragma warning restore 618

            if (executionTimer == null || messageBroker == null)
            {
                return OngoingCapture.Empty();
            }

            return new OngoingCapture(executionTimer, messageBroker, eventName, eventSubText, category, message);
        }

        public static void CaptureMoment(string eventName)
        {
            CaptureMoment(eventName, null, DefaultCategory, new TimelineMessage());
        }

        public static void CaptureMoment(string eventName, string eventSubText)
        {
            CaptureMoment(eventName, eventSubText, DefaultCategory, new TimelineMessage());
        }

        internal static void CaptureMoment(string eventName, TimelineCategoryItem category)
        {
            CaptureMoment(eventName, null, category, new TimelineMessage());
        }

        internal static void CaptureMoment(string eventName, TimelineCategoryItem category, ITimelineMessage message)
        {
            CaptureMoment(eventName, null, category, message);
        }

        internal static void CaptureMoment(string eventName, ITimelineMessage message)
        {
            CaptureMoment(eventName, null, DefaultCategory, message);
        }

        internal static void CaptureMoment(string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentNullException("eventName");
            }

#pragma warning disable 618
            var executionTimer = GlimpseConfiguration.GetConfiguredTimerStrategy()();
            var messageBroker = GlimpseConfiguration.GetConfiguredMessageBroker();
#pragma warning restore 618

            if (executionTimer == null || messageBroker == null)
            {
                return;
            }

            message
                .AsTimelineMessage(eventName, category, eventSubText)
                .AsTimedMessage(executionTimer.Point());

            messageBroker.Publish(message);
        }

        public class OngoingCapture : IDisposable
        {
            public static OngoingCapture Empty()
            {
                return new NullOngoingCapture();
            }

            private OngoingCapture()
            {
            }

            public OngoingCapture(IExecutionTimer executionTimer, IMessageBroker messageBroker, string eventName, string eventSubText, TimelineCategoryItem category, ITimelineMessage message)
            {
                Offset = executionTimer.Start();
                ExecutionTimer = executionTimer;
                Message = message.AsTimelineMessage(eventName, category, eventSubText);
                MessageBroker = messageBroker;
            }

            private ITimelineMessage Message { get; set; }

            private TimeSpan Offset { get; set; }

            private IExecutionTimer ExecutionTimer { get; set; }

            private IMessageBroker MessageBroker { get; set; }

            public virtual void Stop()
            {
                var timerResult = ExecutionTimer.Stop(Offset);

                MessageBroker.Publish(Message.AsTimedMessage(timerResult));
            }

            public void Dispose()
            {
                Stop();
            }

            private class NullOngoingCapture : OngoingCapture
            {
                public override void Stop()
                {
                }
            }
        }
    }
}
