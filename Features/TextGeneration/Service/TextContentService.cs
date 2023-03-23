using Our.Umbraco.Synthscribe.OpenAi.Models;
using Our.Umbraco.Synthscribe.OpenAi.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Our.Umbraco.Synthscribe.Features.TextGeneration.Service
{
    internal class TextContentService : ITextContentService
    {

        private readonly IChatGptService _chatGptService;

        public TextContentService(IChatGptService chatGptService)
        {
            _chatGptService = chatGptService;
        }

        public async Task<string> GetText(string context)
        {
            var response = await _chatGptService.CreateCompletion(new ChatGptCompletion()
            {
                Messages = new List<ChatGptCompletionMessage>()
                {
                    //System context
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.system.ToString(),
                        Content = "You're a text generator for websites."
                    },
                    //Conversation context
                    //new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.user.ToString(),
                    //    Content = $"Write me a title for my website about dogs"
                    //},
                    //new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.assistant.ToString(),
                    //    Content = $"Everything You Need to Know About Dogs"
                    //},
                    // new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.user.ToString(),
                    //    Content = $"Short description about dogs being man's best friend."
                    //},
                    //new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.assistant.ToString(),
                    //    Content = $"Dogs are often referred to as \"man's best friend\" because of their unwavering loyalty, affectionate nature, and ability to form strong emotional bonds with humans. They are highly social animals that thrive on companionship and enjoy spending time with their owners. Dogs have been domesticated for thousands of years and have evolved to become excellent protectors, hunters, and helpers. Whether serving as a guide for the visually impaired, providing emotional support, or simply being a loving companion, dogs have a unique ability to bring joy, comfort, and security to their human counterparts, making them true members of the family."
                    //},
                    //new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.user.ToString(),
                    //    Content = $"Write me 3 paragraphs with titles about dogs, return in html."
                    //},
                    //new ChatGptCompletionMessage()
                    //{
                    //    Role = ChatGptRoles.assistant.ToString(),
                    //    Content = $"<h2>The Loyalty of Dogs</h2><p>Dogs are often referred to as \"man's best friend\" because of their unwavering loyalty. They are known to form strong emotional bonds with their owners and are always eager to please. Whether it's going for a walk, playing fetch, or just snuggling up on the couch, dogs are always up for spending time with their humans. They are highly social animals that thrive on companionship and are always ready to provide comfort and support when needed.</p><h2>The Intelligence of Dogs</h2><p>Dogs are highly intelligent animals that have been bred for various tasks over the centuries. They have the ability to learn complex commands and can be trained to perform a wide range of activities, from herding sheep to assisting people with disabilities. Their intelligence also makes them excellent problem-solvers, and they are often used in search and rescue operations to help locate missing persons. Additionally, dogs have a keen sense of smell and are able to detect odors that are undetectable to humans, making them valuable assets in law enforcement and other fields.</p><h2>The Health Benefits of Owning a Dog</h2>\r\n<p>Owning a dog has been shown to have numerous health benefits. For one, dogs require regular exercise, which can help their owners stay active and lead a more healthy lifestyle. Additionally, studies have shown that spending time with a dog can reduce stress levels and lower blood pressure. Dogs also provide a sense of companionship and can help alleviate feelings of loneliness and depression. Overall, the benefits of owning a dog go beyond just having a furry friend to play with, and can have a positive impact on a person's physical and emotional well-being.</p>"
                    //},
                    //Prompt context
                    new ChatGptCompletionMessage()
                    {
                        Role = ChatGptRoles.user.ToString(),
                        Content = $"[Return only the main response. Remove pre-text and post-text] {context}"
                    }
                }
            });

            return response;
        }
    }
}
