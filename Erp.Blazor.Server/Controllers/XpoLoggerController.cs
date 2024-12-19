using Microsoft.AspNetCore.Mvc;
using DevExpress.Xpo.Logger;
using DevExpress.Xpo.Logger.Transport;

namespace SBT.Apps.Erp.Blazor.Server.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class XpoLoggerController : ControllerBase
    {
        readonly ILogSource logger;
        
        public XpoLoggerController(ILogSource logger)
        {
            this.logger = logger;
        }
        [HttpGet]
        public LogMessage[] GetCompleteLog()
        {
            return logger.GetCompleteLog();
        }
        [HttpGet]
        public LogMessage GetMessage()
        {
            return logger.GetMessage();
        }
        [HttpGet]
        public LogMessage[] GetMessages(int messagesAmount)
        {
            return logger.GetMessages(messagesAmount);
        }
    }
}
