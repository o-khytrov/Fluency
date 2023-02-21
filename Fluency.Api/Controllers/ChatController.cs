using Fluency.Api.Models;
using Fluency.Engine;
using Fluency.Engine.Models;
using Fluency.Engine.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Fluency.Api.Controllers;

[ApiController]
[Route("/")]
public class ChatController : ControllerBase
{
    private readonly ILogger<ChatController> _logger;
    private readonly ChatEngine _chatEngine;
    private readonly IServiceProvider _serviceProvider;
    private readonly IChatContextStorage _storage;

    public ChatController(ILogger<ChatController> logger, ChatEngine chatEngine, IServiceProvider serviceProvider,
        IChatContextStorage storage)
    {
        _logger = logger;
        _chatEngine = chatEngine;
        _serviceProvider = serviceProvider;
        _storage = storage;
    }

    [HttpPost("CreateChat")]
    public async Task<IActionResult> CreateChat(CreateChatRequest request)
    {
        var chatId = Guid.NewGuid().ToString();

        var botType = GetTypeByName(request.DialogueName);
        if (botType is null)
        {
            return BadRequest("No such bot");
        }

        var userMessage = new UserMessage
            { Text = request.Text, Variables = request.Variables };

        var response = await PerformChat(botType, userMessage, chatId);

        var apiResponse = new CreateChatResponse
            { ChatId = chatId, Text = response.Text, Variables = response.Variables };
        return Ok(apiResponse);
    }


    [HttpPost("PerformStep")]
    public async Task<IActionResult> PerformStep(PerformStepRequest request)
    {
        var conversation = await _storage.GetConversation(request.ChatId);
        if (conversation is null)
        {
            return BadRequest("Conversation not found");
        }

        var botType = GetTypeByName(conversation.BotName);
        if (botType is null)
        {
            return BadRequest("No such bot");
        }

        var userMessage = new UserMessage { Text = request.Text, Variables = request.Variables };
        var response = await PerformChat(botType, userMessage, request.ChatId);
        var performStepResponse = new PerformStepResponse
            { Text = response.Text, Variables = response.Variables };
        return Ok(performStepResponse);
    }

    private static Type? GetTypeByName(string name)
    {
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies().Reverse())
        {
            var tt = assembly.GetType(name);
            if (tt != null)
            {
                return tt;
            }
        }

        return null;
    }

    private async Task<BotMessage> PerformChat(Type botType, UserMessage userMessage, string chatId)
    {
        var bot = _serviceProvider.GetRequiredService(botType);
        var chatContextType = GetChatContextType(bot);
        var performChatMethod = _chatEngine.GetType().GetMethod(nameof(ChatEngine.PerformChatAsync));
        var responseTask = performChatMethod!.MakeGenericMethod(chatContextType)
            .Invoke(_chatEngine, new[] { bot, userMessage, chatId }) as Task<BotMessage>;
        if (responseTask is null)
        {
            throw new("Something is wrong");
        }

        var response = await responseTask;
        return response;
    }

    private static Type GetChatContextType(object bot)
    {
        var baseType = bot.GetType().BaseType;
        if (baseType is null)
        {
            throw new Exception("no such bot");
        }

        var chatContextType = baseType.GetGenericArguments()[0];
        return chatContextType;
    }
}