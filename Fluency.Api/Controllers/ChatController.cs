using Fluency.Api.Models;
using Fluency.Engine;
using Fluency.Engine.Models;
using Microsoft.AspNetCore.Mvc;
using TravelBot;

namespace Fluency.Api.Controllers;

[ApiController]
[Route("/")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly ChatEngine _chatEngine;
    private readonly IServiceProvider _serviceProvider;

    public ChatController(ILogger<ChatController> logger, ChatEngine chatEngine, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _chatEngine = chatEngine;
        _serviceProvider = serviceProvider;
    }

    [HttpPost("CreateChat")]
    public async Task<CreateChatResponse> CreateChat(CreateChatRequest request)
    {
        var chatId = Guid.NewGuid().ToString();
        var bot = _serviceProvider.GetRequiredService<Harry>();
        var response = await _chatEngine.PerformChatAsync(bot,
            new UserMessage
                { Text = request.Text, Variables = request.Variables }, chatId);
        return new CreateChatResponse
            { ChatId = chatId, Text = response.Text, Variables = response.Variables };
    }

    [HttpPost("PerformStep")]
    public async Task<PerformStepResponse> PerformStep(PerformStepRequest request)
    {
        var bot = _serviceProvider.GetRequiredService<Harry>();
        var response = await _chatEngine.PerformChatAsync(bot,
            new UserMessage { Text = request.Text, Variables = request.Variables }, request.ChatId);
        return new PerformStepResponse
            { Text = response.Text, Variables = response.Variables };
    }
}