using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureAppToDeploy.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        IDbRepository _dbRepository;
        IQueueManager _queueManager;

        public ValuesController(IDbRepository dbRepository, IQueueManager queueManager)
        {
            _dbRepository = dbRepository;
            _queueManager = queueManager;
        }

        [HttpGet("GetMessages")]
        public List<MessageEntity> GetMessages()
        {
            return _dbRepository.GetAllMessagesFromDb();
        }

        [HttpGet("PutMessage")]
        public async Task<IActionResult> GetCreateMessage(string message)
        {
            _dbRepository.CreateMessage(message);
            var msgId = _dbRepository.GetLastMessageId();

            QueueMessageModel queueMessage = new QueueMessageModel() 
            { 
                MessageDbId = msgId,
                Message = message
            };

            await _queueManager.AddMessageToQueue(queueMessage);

            return Ok(msgId);
        }
    }
}
