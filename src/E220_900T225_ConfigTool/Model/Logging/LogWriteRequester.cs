using Prism.Events;
using System;
using System.Windows;

namespace E220_900T225_ConfigTool.Model.Logging
{
    public class LogWriteRequester : ILogWriteRequester
    {
        IEventAggregator _ea;

        public LogWriteRequester(IEventAggregator ea)
        {
            _ea = ea;
        }

        public void WriteRequest(string message)
        {
            try
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _ea.GetEvent<LogEvent>().Publish(
                    new Log()
                    {
                        dateTime = DateTime.Now,
                        content = message
                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
